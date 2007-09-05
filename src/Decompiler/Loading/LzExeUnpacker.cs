/* 
 * Copyright (C) 1999-2007 John K�ll�n.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */

using Decompiler.Arch.Intel;
using Decompiler.Core;
using System;
using System.Collections;
using System.Diagnostics;

namespace Decompiler.Loading
{
	/// <summary>
	/// An image loader that understands the LzExe packing algorithms. 
	/// </summary>
	public class LzExeUnpacker : ImageLoader
	{
		private int lzHdrOffset;
		private bool isLz91;
		private ProgramImage imgLoaded;
		private ushort lzIp;
		private ushort lzCs;

		// Code insipired by unlzexe utility (unlzexe ver 0.8 (PC-VAN UTJ44266 Kou )

		public LzExeUnpacker(ExeImageLoader exe, ProgramImage rawImg) : base(rawImg)
		{
			this.lzHdrOffset = (exe.e_cparhdr + exe.e_cs) << 4;

			// Locate the LzExe header and verify signature.

			byte [] abC = RawImage.Bytes;
			int entry = lzHdrOffset + exe.e_ip;
			if (CompareEqual(abC, entry, s_sig90, s_sig90.Length)) 
			{
				// Untested binary version
				isLz91 = false;
				throw new NotImplementedException("Untested");
			}			
			else if (CompareEqual(abC, entry, s_sig91, s_sig91.Length))
			{
				isLz91 = true;
			}			
			else
			{
				throw new ApplicationException("Image is not an LzExe-compressed binary");
			}
		}

		// EXE header test (is it LZEXE file?) 

		static public bool IsCorrectUnpacker(ExeImageLoader exe, ProgramImage rawImg)
		{
			if (exe.e_ovno != 0 || exe.e_lfarlc != 0x1C)
				return false;

			int lzHdrOffset = ((int) exe.e_cparhdr + (int) exe.e_cs) << 4;
			int entry = lzHdrOffset + exe.e_ip;
			return (CompareEqual(rawImg.Bytes, entry, s_sig91, s_sig91.Length) ||
					CompareEqual(rawImg.Bytes, entry, s_sig90, s_sig90.Length));
		}

		// Fix up the relocations.

		public override void Relocate(Address addrLoad, ArrayList entryPoints)
		{
			if (isLz91)
			{
				Relocate91(RawImage, addrLoad.seg, imgLoaded);
			}
			else
			{
				Relocate90(RawImage, addrLoad.seg, imgLoaded);
			}

			// Seed the parser with the start location.

			EntryPoint ep =  new EntryPoint(new Address((ushort) (lzCs + addrLoad.seg), lzIp), new IntelState());
			entryPoints.Add(ep);
		}

		// for LZEXE ver 0.90 
		private  void Relocate90(ProgramImage pgmImg, ushort segReloc, ProgramImage pgmImgNew)
		{
			int ifile = lzHdrOffset + 0x19D;

			// 0x19d=compressed relocation table address 

			throw new NotImplementedException("NYI");
			/*
			unsigned int c;
				ushort rel_count=0;
				ushort rel_seg,rel_off;

				rel_seg=0;
				do
				{
					c=getw(ifile);
					for(;c>0;c--)
					{
						rel_off=getw(ifile);
						putw(rel_off,ofile);
						putw(rel_seg,ofile);
						rel_count++;
					}
					rel_seg += 0x1000;
				} while(rel_seg!=(0xf000+0x1000));
				ohead[3]=rel_count;
				return(SUCCESS);
			*/
		}

		// Unpacks the relocation entries in a LzExe 0.91 binary

		private void Relocate91(ProgramImage pgmImg, ushort segReloc, ProgramImage pgmImgNew)
		{
			ushort span;
			byte [] abUncompressed = pgmImg.Bytes;
			int ifile = lzHdrOffset + 0x158;

			// 0x158=compressed relocation table address 

			int rel_off=0;
			for (;;)
			{
				span = abUncompressed[ifile++];
				if (span == 0)
				{
					span = abUncompressed[ifile++];
					span |= (ushort) (abUncompressed[ifile++] << 8);
					if (span == 0)
					{
						rel_off += 0x0FFF0;
						continue;
					}
					else if (span == 1)
					{
						break;
					}
				}

				rel_off += span;
				ushort seg = (ushort) (pgmImgNew.ReadUShort(rel_off) + segReloc);
				pgmImgNew.WriteUShort(rel_off, seg);

				// This is a known segment!

				ImageMap.AddSegment(new Address(seg, 0), AccessMode.ReadWrite);
			}
		}


		public override ProgramImage Load(Address addrLoad)
		{
			Unpack(RawImage, addrLoad);
			return imgLoaded;
		}

		public override Address PreferredBaseAddress
		{
			get { return new Address(0x800, 0); }
		}

		public ProgramImage Unpack(ProgramImage pgmImg, Address addrLoad)
		{
			// Extract the LZ stuff.

			ImageReader rdr = pgmImg.CreateReader(lzHdrOffset);
			lzIp = rdr.ReadUShort();
			lzCs = rdr.ReadUShort();
			ushort lzSp = rdr.ReadUShort();
			ushort lzSs = rdr.ReadUShort();
			ushort lzcpCompressed = rdr.ReadUShort();
			ushort lzcpDecompressed = rdr.ReadUShort();

			// Find the start of the compressed stream.

			byte [] abC = pgmImg.Bytes;
			int ifile = lzHdrOffset - (lzcpCompressed << 4);

			// Allocate space for the decompressed goo.

			int cbUncompressed = ((int) lzcpDecompressed + lzcpDecompressed) << 4;
			byte [] abU = new byte[cbUncompressed];

			// Decompress this sorry mess.

			int len;
			int span;
			int p = 0;
			BitStream bits = new BitStream(abC, ifile);
			for (;;)
			{
				if (bits.GetBit() != 0)
				{
					// 1....
					abU[p++] = bits.GetByte();
					continue;
				}

				if (bits.GetBit() == 0) 
				{
					// 00.....
					len = bits.GetBit() << 1;
					len |= bits.GetBit();
					len += 2;
					span = bits.GetByte() | ~0xFF;
				} 
				else
				{
					// 01.....

					span = bits.GetByte();
					len = bits.GetByte();;
					span |= ((len & ~0x07)<<5) | ~0x1FFF;
					len = (len & 0x07) + 2;
					if (len == 2)
					{
						len = bits.GetByte();

						if (len == 0)
							break;    // end mark of compressed load module 

						if (len == 1)
							continue; // segment change 
						else
							++len;
					}
				}
				for( ;len > 0; --len, ++p)
				{
					abU[p] = abU[p+span];
				}
			}

			// Create a new image based on the uncompressed data.

			this.imgLoaded = new ProgramImage(addrLoad, abU);
			this.ImageMap = new ImageMap(imgLoaded);

			return imgLoaded;
		}


		static byte [] s_sig90 = 			
		{
			0x06, 0x0E, 0x1F, 0x8B, 0x0E, 0x0C, 0x00, 0x8B,
			0xF1, 0x4E, 0x89, 0xF7, 0x8C, 0xDB, 0x03, 0x1E,
			0x0A, 0x00, 0x8E, 0xC3, 0xB4, 0x00, 0x31, 0xED,
			0xFD, 0xAC, 0x01, 0xC5, 0xAA, 0xE2, 0xFA, 0x8B,
			0x16, 0x0E, 0x00, 0x8A, 0xC2, 0x29, 0xC5, 0x8A,
			0xC6, 0x29, 0xC5, 0x39, 0xD5, 0x74, 0x0C, 0xBA,
			0x91, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0xFF,
			0x4C, 0xCD, 0x21, 0x53, 0xB8, 0x53, 0x00, 0x50,
			0xCB, 0x2E, 0x8B, 0x2E, 0x08, 0x00, 0x8C, 0xDA,
			0x89, 0xE8, 0x3D, 0x00, 0x10, 0x76, 0x03, 0xB8,
			0x00, 0x10, 0x29, 0xC5, 0x29, 0xC2, 0x29, 0xC3,
			0x8E, 0xDA, 0x8E, 0xC3, 0xB1, 0x03, 0xD3, 0xE0,
			0x89, 0xC1, 0xD1, 0xE0, 0x48, 0x48, 0x8B, 0xF0,
			0x8B, 0xF8, 0xF3, 0xA5, 0x09, 0xED, 0x75, 0xD8,
			0xFC, 0x8E, 0xC2, 0x8E, 0xDB, 0x31, 0xF6, 0x31,
			0xFF, 0xBA, 0x10, 0x00, 0xAD, 0x89, 0xC5, 0xD1,
			0xED, 0x4A, 0x75, 0x05, 0xAD, 0x89, 0xC5, 0xB2,
			0x10, 0x73, 0x03, 0xA4, 0xEB, 0xF1, 0x31, 0xC9,
			0xD1, 0xED, 0x4A, 0x75, 0x05, 0xAD, 0x89, 0xC5,
			0xB2, 0x10, 0x72, 0x22, 0xD1, 0xED, 0x4A, 0x75,
			0x05, 0xAD, 0x89, 0xC5, 0xB2, 0x10, 0xD1, 0xD1,
			0xD1, 0xED, 0x4A, 0x75, 0x05, 0xAD, 0x89, 0xC5,
			0xB2, 0x10, 0xD1, 0xD1, 0x41, 0x41, 0xAC, 0xB7,
			0xFF, 0x8A, 0xD8, 0xE9, 0x13, 0x00, 0xAD, 0x8B,
			0xD8, 0xB1, 0x03, 0xD2, 0xEF, 0x80, 0xCF, 0xE0,
			0x80, 0xE4, 0x07, 0x74, 0x0C, 0x88, 0xE1, 0x41,
			0x41, 0x26, 0x8A, 0x01, 0xAA, 0xE2, 0xFA, 0xEB,
			0xA6, 0xAC, 0x08, 0xC0, 0x74, 0x40, 0x3C, 0x01,
			0x74, 0x05, 0x88, 0xC1, 0x41, 0xEB, 0xEA, 0x89
		};

		static byte [] s_sig91 =
		{
			0x06, 0x0E, 0x1F, 0x8B, 0x0E, 0x0C, 0x00, 0x8B,
			0xF1, 0x4E, 0x89, 0xF7, 0x8C, 0xDB, 0x03, 0x1E,
			0x0A, 0x00, 0x8E, 0xC3, 0xFD, 0xF3, 0xA4, 0x53,
			0xB8, 0x2B, 0x00, 0x50, 0xCB, 0x2E, 0x8B, 0x2E,
			0x08, 0x00, 0x8C, 0xDA, 0x89, 0xE8, 0x3D, 0x00,
			0x10, 0x76, 0x03, 0xB8, 0x00, 0x10, 0x29, 0xC5,
			0x29, 0xC2, 0x29, 0xC3, 0x8E, 0xDA, 0x8E, 0xC3,
			0xB1, 0x03, 0xD3, 0xE0, 0x89, 0xC1, 0xD1, 0xE0,
			0x48, 0x48, 0x8B, 0xF0, 0x8B, 0xF8, 0xF3, 0xA5,
			0x09, 0xED, 0x75, 0xD8, 0xFC, 0x8E, 0xC2, 0x8E,
			0xDB, 0x31, 0xF6, 0x31, 0xFF, 0xBA, 0x10, 0x00,
			0xAD, 0x89, 0xC5, 0xD1, 0xED, 0x4A, 0x75, 0x05,
			0xAD, 0x89, 0xC5, 0xB2, 0x10, 0x73, 0x03, 0xA4,
			0xEB, 0xF1, 0x31, 0xC9, 0xD1, 0xED, 0x4A, 0x75,
			0x05, 0xAD, 0x89, 0xC5, 0xB2, 0x10, 0x72, 0x22,
			0xD1, 0xED, 0x4A, 0x75, 0x05, 0xAD, 0x89, 0xC5,
			0xB2, 0x10, 0xD1, 0xD1, 0xD1, 0xED, 0x4A, 0x75,
			0x05, 0xAD, 0x89, 0xC5, 0xB2, 0x10, 0xD1, 0xD1,
			0x41, 0x41, 0xAC, 0xB7, 0xFF, 0x8A, 0xD8, 0xE9,
			0x13, 0x00, 0xAD, 0x8B, 0xD8, 0xB1, 0x03, 0xD2,
			0xEF, 0x80, 0xCF, 0xE0, 0x80, 0xE4, 0x07, 0x74,
			0x0C, 0x88, 0xE1, 0x41, 0x41, 0x26, 0x8A, 0x01,
			0xAA, 0xE2, 0xFA, 0xEB, 0xA6, 0xAC, 0x08, 0xC0,
			0x74, 0x34, 0x3C, 0x01, 0x74, 0x05, 0x88, 0xC1,
			0x41, 0xEB, 0xEA, 0x89, 0xFB, 0x83, 0xE7, 0x0F,
			0x81, 0xC7, 0x00, 0x20, 0xB1, 0x04, 0xD3, 0xEB,
			0x8C, 0xC0, 0x01, 0xD8, 0x2D, 0x00, 0x02, 0x8E,
			0xC0, 0x89, 0xF3, 0x83, 0xE6, 0x0F, 0xD3, 0xEB,
			0x8C, 0xD8, 0x01, 0xD8, 0x8E, 0xD8, 0xE9, 0x72
		};
	}

	public class BitStream
	{
		private byte [] ab;
		private int p;
		private int cb;
		private ushort bitBuf;

		public BitStream(byte [] ab, int i) 
		{
			this.ab = ab;
			this.p = i;

			LoadNextWord();
		}

		public int AccumulateBit(int acc)
		{
			return (acc << 1) | GetBit();
		}

		public int GetBit()
		{
			int b = (bitBuf & 1);
			bitBuf >>= 1;
			if (--cb == 0)
			{
				LoadNextWord();
			}
			return b;
		}

		public byte GetByte()
		{
			return ab[p++];
		}

		public ushort GetWord()
		{
			ushort w = ab[p++];
			return (ushort) (w | (ushort) ab[p++] << 8);
		}

		private void LoadNextWord()
		{
			cb = 0x10;
			bitBuf = (ushort) (ab[p] + (((ushort) ab[p+1]) << 8));
			p += 2;
		}
	}
}
