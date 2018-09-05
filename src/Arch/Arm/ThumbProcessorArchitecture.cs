﻿#region License
/* 
 * Copyright (C) 1999-2018 John Källén.
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
#endregion

using Reko.Arch.Arm.AArch32;
using Reko.Core;
using Reko.Core.Expressions;
using Reko.Core.Lib;
using Reko.Core.Machine;
using Reko.Core.NativeInterface;
using Reko.Core.Rtl;
using Reko.Core.Serialization;
using Reko.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Reko.Arch.Arm
{
    public class ThumbArchitecture : ProcessorArchitecture
    {
        private INativeArchitecture native;
        private Dictionary<string, RegisterStorage> regsByName;
        private Dictionary<int, RegisterStorage> regsByNumber;
        private Dictionary<uint, FlagGroupStorage> flagGroups;

        public ThumbArchitecture(string archId) : base(archId)
        {
            this.FramePointerType = PrimitiveType.Ptr32;
            this.PointerType = PrimitiveType.Ptr32;
            this.WordWidth = PrimitiveType.Word32;
            this.InstructionBitSize = 16;

            this.flagGroups = new Dictionary<uint, FlagGroupStorage>();
#if NATIVE

            var unk = CreateNativeArchitecture("arm-thumb");
            this.native = (INativeArchitecture)Marshal.GetObjectForIUnknown(unk);
            GetRegistersFromNative();
#else
            this.regsByNumber = Registers.GpRegs.ToDictionary(r => r.Number);
            this.regsByName = regsByNumber.ToDictionary(r => r.Value.Name, r => r.Value);
#endif
            StackRegister = regsByName["sp"];
        }

        private void GetRegistersFromNative()
        {
            int cRegs;
            IntPtr aRegs;
            native.GetAllRegisters(0, out cRegs, out aRegs);
            if (aRegs == null)
                throw new OutOfMemoryException();
            this.regsByName = new Dictionary<string, RegisterStorage>();
            this.regsByNumber = new Dictionary<int, RegisterStorage>();
            NativeRegister nReg = new NativeRegister();
            int cb = Marshal.SizeOf(nReg);
            while (cRegs > 0)
            {
                nReg = (NativeRegister)Marshal.PtrToStructure(aRegs, typeof(NativeRegister));
                if (nReg.Name != null)
                {
                    var n = nReg.Name;
                    var i = nReg.Number;
                    var b = nReg.BitSize;
                    var reg = new RegisterStorage(n, i, 0, PrimitiveType.CreateWord(b));
                    regsByName.Add(reg.Name, reg);
                    regsByNumber.Add(reg.Number, reg);
        }
                aRegs += cb;
                --cRegs;
            }
        }

        public override IEnumerable<MachineInstruction> CreateDisassembler(EndianImageReader rdr)
        {
            return new T32Disassembler(this, rdr);
            /*
            var bytes = rdr.Bytes;
            ulong uAddr = rdr.Address.ToLinear();
            var hBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            INativeDisassembler ndasm = null;

            try
            {
                ndasm = native.CreateDisassembler(hBytes.AddrOfPinnedObject(), bytes.Length, (int)rdr.Offset, uAddr);
                for (;;)
                {
                    INativeInstruction nInstr = ndasm.NextInstruction();
                    if (nInstr == null)
                        yield break;
                    else
                        yield return new Arm32Instruction(nInstr);
                }
            }
            finally
            {
                if (ndasm != null)
                {
                    ndasm = null;
                }
                if (hBytes != null && hBytes.IsAllocated)
                {
                    hBytes.Free();
                }
            }*/
        }

        public override IEnumerable<RtlInstructionCluster> CreateRewriter(EndianImageReader rdr, ProcessorState state, IStorageBinder binder, IRewriterHost host)
        {
            return new ThumbRewriter(this, rdr, host, binder);
            //return new ThumbRewriterRetired(regsByNumber, this.native, rdr, (ArmProcessorState)state, binder, host);
        }

        public override EndianImageReader CreateImageReader(MemoryArea img, Address addr)
        {
            return new LeImageReader(img, addr);
        }

        public override EndianImageReader CreateImageReader(MemoryArea image, Address addrBegin, Address addrEnd)
        {
            return new LeImageReader(image, addrBegin, addrEnd);
        }

        public override EndianImageReader CreateImageReader(MemoryArea img, ulong off)
        {
            throw new NotImplementedException();
        }

        public override ImageWriter CreateImageWriter()
        {
            return new LeImageWriter();
        }

        public override ImageWriter CreateImageWriter(MemoryArea mem, Address addr)
        {
            return new LeImageWriter(mem, addr);
        }

        public override IEqualityComparer<MachineInstruction> CreateInstructionComparer(Normalize norm)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Address> CreatePointerScanner(SegmentMap map, EndianImageReader rdr, IEnumerable<Address> knownAddresses, PointerScannerFlags flags)
        {
            throw new NotImplementedException();
        }

        public override ProcessorState CreateProcessorState()
        {
            return new AArch32ProcessorState(this);
        }
 

        public override SortedList<string, int> GetOpcodeNames()
        {
            return new SortedList<string, int>();
        }

        public override int? GetOpcodeNumber(string name)
        {
                return null;
        }

        public override RegisterStorage GetRegister(int i)
        {
            RegisterStorage reg;
            if (regsByNumber.TryGetValue(i, out reg))
                return reg;
            else
                return null;
        }

        public override RegisterStorage GetRegister(string name)
        {
            RegisterStorage reg;
            if (regsByName.TryGetValue(name, out reg))
                return reg;
            else
                return null;
        }

        public override RegisterStorage[] GetRegisters()
        {
            return regsByNumber.Values.OrderBy(r => r.Number).ToArray();
        }

        public override RegisterStorage GetSubregister(RegisterStorage reg, int offset, int width)
        {
            throw new NotSupportedException();
        }

        public override bool TryGetRegister(string name, out RegisterStorage reg)
        {
            throw new NotImplementedException();
        }

        public override FlagGroupStorage GetFlagGroup(RegisterStorage flagRegister, uint grf)
        {
            if (flagGroups.TryGetValue(grf, out FlagGroupStorage f))
            {
                return f;
            }

            var dt = Bits.IsSingleBitSet(grf) ? PrimitiveType.Bool : PrimitiveType.Byte;
            var fl = new FlagGroupStorage(regsByName["CPSRQQQQ"], grf, GrfToString(flagRegister, "", grf), dt);
            flagGroups.Add(grf, fl);
            return fl;
        }

        public override FlagGroupStorage GetFlagGroup(string name)
        {
            throw new NotImplementedException();
        }

        public override Address ReadCodeAddress(int size, EndianImageReader rdr, ProcessorState state)
        {
            throw new NotImplementedException();
        }

        public override string GrfToString(RegisterStorage flagregister, string prefix, uint grf)
        {
            StringBuilder s = new StringBuilder();
            if ((grf & (uint)FlagM.NF) != 0) s.Append('N');
            if ((grf & (uint)FlagM.ZF) != 0) s.Append('Z');
            if ((grf & (uint)FlagM.CF) != 0) s.Append('C');
            if ((grf & (uint)FlagM.VF) != 0) s.Append('V');
            return s.ToString();
        }

        public override bool TryParseAddress(string txtAddr, out Address addr)
        {
            return Address.TryParse32(txtAddr, out addr);
        }

        public override Address MakeAddressFromConstant(Constant c)
        {
            throw new NotImplementedException();
        }

        public override bool TryRead(MemoryArea mem, Address addr, PrimitiveType dt, out Constant value)
        {
            return mem.TryReadLe(addr, dt, out value);
        }

        [DllImport("ArmNative", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "CreateNativeArchitecture")]
        public static extern IntPtr CreateNativeArchitecture(
            [MarshalAs(UnmanagedType.LPStr)] string archName);
    }
}
