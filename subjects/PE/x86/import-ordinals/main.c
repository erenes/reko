// main.c
// Generated by decompiling main.exe
// using Reko decompiler version 0.8.0.2.

#include "main.h"

// 00401000: void fn00401000()
void fn00401000()
{
}

// 00401010: void fn00401010(Stack word32 dwArg04)
void fn00401010(word32 dwArg04)
{
	word32 esp_13;
	word32 ebp_14;
	word32 esi_15;
	byte SCZO_16;
	word32 ecx_17;
	word32 eax_18;
	_acrt_iob_func();
	fn00401000();
	word32 esp_37;
	word32 ebp_38;
	word32 esi_39;
	byte SCZO_40;
	word32 ecx_41;
	word32 eax_42;
	_stdio_common_vfprintf();
}

// 00401040: Register word32 fn00401040()
word32 fn00401040()
{
	word32 esp_10;
	word32 ebp_11;
	word32 ecx_12;
	word32 eax_13;
	byte SCZO_14;
	byte SZO_15;
	bool C_16;
	driver.dll!Ordinal_1();
	word32 esp_20;
	word32 ebp_21;
	word32 ecx_22;
	word32 eax_23;
	byte SCZO_24;
	byte SZO_25;
	bool C_26;
	driver.dll!Ordinal_2();
	word32 esp_30;
	word32 ebp_31;
	word32 ecx_32;
	word32 eax_33;
	byte SCZO_34;
	byte SZO_35;
	bool C_36;
	driver.dll!Ordinal_4();
	word32 esp_40;
	word32 ebp_41;
	word32 ecx_42;
	word32 eax_43;
	byte SCZO_44;
	byte SZO_45;
	bool C_46;
	driver.dll!Ordinal_3();
	fn00401010(0x00402118);
	return 0x00;
}

// 004012C9: Register Eq_64 Win32CrtStartup()
DWORD Win32CrtStartup()
{
	fn0040165E();
	struct Eq_68 * ebp_10 = fn00401980(ebx, esi, edi, dwLoc0C, 0x00402508, 0x14);
	word32 * esp_16 = fp - 0x08;
	if (fn0040146F(edx, 0x01) == 0x00)
	{
l00401172:
		esp_16 -= 0x04;
		*esp_16 = 0x07;
		fn0040176D(0x14, dwArg00);
	}
	*(ebp_10 - 0x19) = 0x00;
	*(ebp_10 - 0x04) &= 0x00;
	*(ebp_10 - 0x24) = fn0040143A();
	ebx = DPB(ebx, 0x00, 0);
	word32 eax_35 = globals->dw403334;
	if (eax_35 != 0x01)
	{
		Eq_64 eax_186;
		if (eax_35 == 0x00)
		{
			globals->dw403334 = 0x01;
			PVFV ** esp_273 = esp_16 - 0x04;
			*esp_273 = (PVFV **) &globals->t4020F0;
			*(esp_273 - 0x04) = 0x004020E4;
			if (_initterm_e(*(esp_273 - 0x04), *esp_273) != 0x00)
			{
				*(ebp_10 - 0x04) = ~0x01;
				eax_186 = 0xFF;
				goto l004012C3;
			}
			*esp_273 = (PVFV **) &globals->t4020E0;
			*(esp_273 - 0x04) = 0x004020D8;
			_initterm(*(esp_273 - 0x04), *esp_273);
			globals->dw403334 = 0x02;
		}
		else
		{
			*(ebp_10 - 0x19) = 0x01;
			ebx = DPB(ebx, 0x01, 0);
		}
		ptr32 * esp_72 = esp_16 - 0x04;
		*esp_72 = *(ebp_10 - 0x24);
		fn004015C9(bArg00);
		fn00401761();
		ptr32 esp_245 = esp_72 + 0x01;
		ptr32 edi_241 = 0x00;
		if (globals->dw403384 != 0x00)
		{
			*esp_72 = 0x00403384;
			word32 ebp_239;
			<anonymous> ** esi_240;
			word32 eax_242 = fn0040153F(ebx, 0x00403384, 0x00, out ebx, out ebp_239, out esi_240, out edi_241);
			byte al_243 = (byte) eax_242;
			esp_245 = esp_72 + 0x01;
			if (al_243 != 0x00)
			{
				*esp_72 = edi_241;
				*(esp_72 - 0x04) = 0x02;
				*(esp_72 - 0x08) = edi_241;
				<anonymous> * esi_255 = *esi_240;
				fn00401972();
				word32 ecx_258;
				byte al_259;
				byte SZO_260;
				bool C_261;
				bool Z_262;
				byte bl_263;
				word32 ebp_264;
				word32 eax_265;
				byte SCZO_266;
				byte cl_267;
				word32 esi_268;
				word32 edx_271;
				esi_255();
			}
		}
		word32 eax_91 = fn00401767();
		if (globals->ptr403380 != edi_241)
		{
			ptr32 * esp_203 = esp_245 - 0x04;
			*esp_203 = 0x00403380;
			word32 ebx_205;
			word32 ebp_206;
			ptr32 * esi_207;
			word32 edi_208;
			word32 eax_209 = fn0040153F(ebx, 0x00403380, edi_241, out ebx_205, out ebp_206, out esi_207, out edi_208);
			byte al_210 = (byte) eax_209;
			if (al_210 != 0x00)
			{
				*esp_203 = *esi_207;
				word32 esp_219;
				word32 ecx_220;
				byte al_221;
				byte SZO_222;
				bool C_223;
				bool Z_224;
				byte bl_225;
				word32 ebp_226;
				word32 eax_227;
				byte SCZO_228;
				byte cl_229;
				word32 esi_230;
				word32 edi_231;
				word32 ebx_232;
				word32 edx_233;
				register_thread_local_exe_atexit_callback();
			}
		}
		word32 esp_96;
		word32 ecx_97;
		byte al_98;
		byte SZO_99;
		bool C_100;
		bool Z_101;
		byte bl_102;
		word32 ebp_103;
		word32 eax_104;
		byte SCZO_105;
		byte cl_106;
		word32 esi_107;
		word32 edi_108;
		word32 ebx_109;
		word32 edx_110;
		_p___argv();
		word32 esp_112;
		word32 ecx_113;
		byte al_114;
		byte SZO_115;
		bool C_116;
		bool Z_117;
		byte bl_118;
		word32 ebp_119;
		word32 eax_120;
		byte SCZO_121;
		byte cl_122;
		word32 esi_123;
		word32 edi_124;
		word32 ebx_125;
		word32 edx_126;
		_p___argc();
		ptr32 esp_128;
		word32 ecx_129;
		byte al_130;
		byte SZO_131;
		bool C_132;
		bool Z_133;
		byte bl_134;
		word32 eax_136;
		byte SCZO_137;
		byte cl_138;
		word32 * esi_139;
		word32 * edi_140;
		word32 ebx_141;
		word32 edx_142;
		get_initial_narrow_environment();
		*(esp_128 - 0x04) = eax_136;
		*(esp_128 - 0x08) = *edi_140;
		*(esp_128 - 0x0C) = *esi_139;
		Eq_64 eax_151 = fn00401040();
		ptr32 esp_152 = esp_128;
		Eq_64 esi_154 = eax_151;
		byte al_155 = fn0040188B();
		if (al_155 == 0x00)
		{
			*(esp_128 - 0x04) = (uint32) eax_151;
			exit(*(esp_128 - 0x04));
		}
		if (bl_134 == 0x00)
		{
			word32 ecx_189;
			byte al_190;
			byte SZO_191;
			bool C_192;
			bool Z_193;
			byte bl_194;
			word32 eax_196;
			byte SCZO_197;
			byte cl_198;
			word32 edi_200;
			word32 ebx_201;
			word32 edx_202;
			cexit();
		}
		word32 * esp_176 = esp_152 - 0x04;
		*esp_176 = 0x00;
		*(esp_176 - 0x04) = 0x01;
		fn004015E6(dwArg00, bArg04);
		*(ebp_10 - 0x04) = ~0x01;
		eax_186 = esi_154;
l004012C3:
		word32 ebp_61;
		word32 esi_62;
		word32 edi_63;
		fn004019C6(ebp_10, 0x14, dwArg00, dwArg04, dwArg08, dwArg0C, out ebp_61, out esi_62, out edi_63);
		return eax_186;
	}
	goto l00401172;
}

// 004013F6: Register (ptr32 Eq_524) fn004013F6(Stack (ptr32 Eq_525) dwArg04, Stack uint32 dwArg08)
Eq_524 * fn004013F6(Eq_525 * dwArg04, uint32 dwArg08)
{
	struct Eq_524 * eax_26;
	struct Eq_528 * ecx_13 = dwArg04 + dwArg04->dw003C / 0x0040;
	struct Eq_524 * edx_16 = &ecx_13->w0014 + 0x02 + (word32) ecx_13->w0014 / 22;
	struct Eq_524 * esi_19 = edx_16 + (word32) ecx_13->w0006;
	for (; edx_16 != esi_19; ++edx_16)
	{
		if (dwArg08 >= edx_16->dw000C && dwArg08 < edx_16->dw0008 + edx_16->dw000C)
		{
			eax_26 = edx_16;
			return eax_26;
		}
	}
	eax_26 = null;
	return eax_26;
}

// 0040143A: Register byte fn0040143A()
byte fn0040143A()
{
	if (fn00401B98() == 0x00)
		return 0x00;
	word32 edx_32 = fs->ptr0018->dw0004;
	do
	{
		__lock();
		word32 eax_37;
		__cmpxchg(globals->dw403338, edx_32, 0x00, out eax_37);
		if (eax_37 == 0x00)
			return 0x00;
	} while (edx_32 != eax_37);
	return 0x01;
}

// 0040146F: Register byte fn0040146F(Register word32 edx, Stack word32 dwArg04)
byte fn0040146F(word32 edx, word32 dwArg04)
{
	if (dwArg04 == 0x00)
		globals->b403354 = 0x01;
	fn004019FE(edx);
	fn00401C46();
	if (false)
		return 0x00;
	fn00401C46();
	if (true)
		return 0x01;
	fn00401C46();
	return 0x00;
}

// 0040153F: Register (ptr32 Eq_623) fn0040153F(Register word32 ebx, Register ptr32 esi, Register ptr32 edi, Register out ptr32 ebxOut, Register out ptr32 ebpOut, Register out ptr32 esiOut, Register out ptr32 ediOut)
Eq_623 * fn0040153F(word32 ebx, ptr32 esi, ptr32 edi, ptr32 & ebxOut, ptr32 & ebpOut, ptr32 & esiOut, ptr32 & ediOut)
{
	struct Eq_623 * eax_34;
	struct Eq_68 * ebp_10 = fn00401980(ebx, esi, edi, dwLoc0C, 0x00402528, 0x08);
	*(ebp_10 - 0x04) &= 0x00;
	struct Eq_643 * eax_15 = (struct Eq_643 *) 23117;
	if (globals->t400000 == 23117)
	{
		eax_15 = globals->ptr40003C;
		if (eax_15->dw400000 == 0x4550 && eax_15->w400018 == 0x010B)
		{
			ui32 eax_71 = ebp_10->dw0008;
			dwLoc0C = eax_71 - 0x00400000;
			eax_15 = fn004013F6(&globals->t400000, eax_71 - 0x00400000);
			if (eax_15 != null && eax_15->dw0024 >= 0x00)
			{
				*(ebp_10 - 0x04) = ~0x01;
				eax_34 = (struct Eq_623 *) DPB(eax_15, 0x01, 0);
l004015C3:
				word32 ebp_38;
				word32 esi_39;
				word32 edi_40;
				word32 ebx_41;
				*ebxOut = fn004019C6(ebp_10, dwLoc0C, 0x00402528, 0x08, dwArg00, dwArg04, out ebp_38, out esi_39, out edi_40);
				return eax_34;
			}
		}
	}
	*(ebp_10 - 0x04) = ~0x01;
	eax_34 = (struct Eq_623 *) DPB(eax_15, 0x00, 0);
	goto l004015C3;
}

// 004015C9: void fn004015C9(Stack byte bArg04)
void fn004015C9(byte bArg04)
{
	if (fn00401B98() != 0x00 && bArg04 == 0x00)
		globals->dw403338 = 0x00;
}

// 004015E6: void fn004015E6(Stack word32 dwArg04, Stack byte bArg08)
void fn004015E6(word32 dwArg04, byte bArg08)
{
	if (globals->b403354 == 0x00 || bArg08 == 0x00)
	{
		fn00401C46();
		fn00401C46();
	}
}

// 0040165E: void fn0040165E()
void fn0040165E()
{
	ui32 eax_16 = globals->dw403004;
	if (eax_16 != 0xBB40E64E && (eax_16 & 0xFFFF0000) != 0x00)
		globals->dw403000 = ~eax_16;
	else
	{
		GetSystemTimeAsFileTime(fp - 0x10);
		ui32 v14_55 = dwLoc0C & 0x00 ^ dwLoc10 & 0x00 ^ GetCurrentThreadId() ^ GetCurrentProcessId();
		QueryPerformanceCounter(fp - 0x18);
		ui32 ecx_69 = dwLoc14 ^ dwLoc18 ^ v14_55 ^ fp - 0x08;
		if (ecx_69 == 0xBB40E64E)
			ecx_69 = ~0x44BF19B0;
		else if ((ecx_69 & 0xFFFF0000) == 0x00)
			ecx_69 |= (ecx_69 | 0x4711) << 0x10;
		globals->dw403004 = ecx_69;
		globals->dw403000 = ~ecx_69;
	}
}

// 00401761: void fn00401761()
void fn00401761()
{
}

// 00401767: Register ptr32 fn00401767()
ptr32 fn00401767()
{
	return 0x00403380;
}

// 0040176D: void fn0040176D(Stack word32 dwArg00, Stack word32 dwArg04)
void fn0040176D(word32 dwArg00, word32 dwArg04)
{
	if (IsProcessorFeaturePresent(0x17) == 0x00)
	{
		globals->dw403368 = 0x00;
		memset(fp - 808, 0x00, 0x02CC);
		memset(fp - 0x5C, 0x00, 0x50);
		byte bl_90 = 0x00 - (0x01 - IsDebuggerPresent() == 0x00);
		SetUnhandledExceptionFilter(null);
		if (UnhandledExceptionFilter(fp - 0x0C) == 0x00)
			globals->dw403368 &= 0x00 - (-((word32) (bl_90 + 0x01)) == 0x00);
	}
	else
		__fastfail(dwArg04);
}

// 0040188B: Register Eq_883 fn0040188B()
Eq_883 fn0040188B()
{
	Eq_884 eax_4 = GetModuleHandleW(null);
	if (eax_4 == null || eax_4->unused != 23117)
		return 0x00;
	struct Eq_898 * eax_42 = eax_4 + eax_4->dw003C / 0x0040;
	if (eax_42->dw0000 != 0x4550 || (eax_42->w0018 != 0x010B || eax_42->dw0074 <= 0x0E))
		return 0x00;
	return eax_42->dw00E8 != 0x00;
}

// 00401972: void fn00401972()
void fn00401972()
{
	word32 esp_3;
	globals->ptr4020D4();
}

// 00401980: Register ptr32 fn00401980(Register word32 ebx, Register ptr32 esi, Register ptr32 edi, Stack ui32 dwArg00, Stack word32 dwArg04, Stack ui32 dwArg08)
ptr32 fn00401980(word32 ebx, ptr32 esi, ptr32 edi, ui32 dwArg00, word32 dwArg04, ui32 dwArg08)
{
	ptr32 esp_14 = fp - 0x08 - dwArg08;
	*(esp_14 - 0x04) = ebx;
	*(esp_14 - 0x08) = esi;
	*(esp_14 - 0x0C) = edi;
	*(esp_14 - 0x0010) = globals->dw403004 ^ fp + 0x08;
	*(esp_14 - 0x0014) = dwArg00;
	fs->ptr0000 = fp - 0x08;
	return fp + 0x08;
}

// 004019C6: Register word32 fn004019C6(Register (ptr32 Eq_68) ebp, Stack ui32 dwArg00, Stack word32 dwArg04, Stack word32 dwArg08, Stack word32 dwArg0C, Stack word32 dwArg10, Register out ptr32 ebpOut, Register out ptr32 esiOut, Register out ptr32 ediOut)
word32 fn004019C6(Eq_68 * ebp, ui32 dwArg00, word32 dwArg04, word32 dwArg08, word32 dwArg0C, word32 dwArg10, ptr32 & ebpOut, ptr32 & esiOut, ptr32 & ediOut)
{
	fs->dw0000 = *(ebp - 0x10);
	word32 ebp_22;
	*ebpOut = ebp->dw0000;
	ebp->dw0000 = dwArg00;
	word32 edi_14;
	*ediOut = dwArg08;
	word32 esi_17;
	*esiOut = dwArg0C;
	return dwArg10;
}

// 004019FE: void fn004019FE(Register word32 edx)
void fn004019FE(word32 edx)
{
	globals->dw40336C &= 0x00;
	globals->dw403010 |= 0x01;
	if (IsProcessorFeaturePresent(0x0A) != 0x00)
	{
		ui32 edi_135;
		globals->dw403010 |= 0x02;
		globals->dw40336C = 0x01;
		int32 eax_89;
		word32 ebx_90;
		word32 ecx_91;
		word32 edx_92;
		__cpuid(0x00, 0x00, &eax_89, &ebx_90, &ecx_91, &edx_92);
		ui32 eax_124;
		word32 ebx_125;
		ui32 ecx_126;
		word32 edx_127;
		__cpuid(0x01, 0x00, &eax_124, &ebx_125, &ecx_126, &edx_127);
		if ((edx_92 ^ 0x49656E69 | ecx_91 ^ 1818588270 | ebx_90 ^ 1970169159) == 0x00 && ((eax_124 & 0x0FFF3FF0) == 0x000106C0 || ((eax_124 & 0x0FFF3FF0) == 0x00020660 || ((eax_124 & 0x0FFF3FF0) == 0x00020670 || ((eax_124 & 0x0FFF3FF0) == 0x00030650 || ((eax_124 & 0x0FFF3FF0) == 0x00030660 || (eax_124 & 0x0FFF3FF0) == 0x00030670))))))
		{
			ui32 edi_245 = globals->dw403370;
			globals->dw403370 = edi_245 | 0x01;
			edi_135 = edi_245 | 0x01;
		}
		else
			edi_135 = globals->dw403370;
		if (eax_89 >= 0x07)
		{
			word32 eax_210;
			ui32 ebx_211;
			word32 ecx_212;
			word32 edx_213;
			__cpuid(0x07, 0x00, &eax_210, &ebx_211, &ecx_212, &edx_213);
			if ((ebx_211 & 0x0200) != 0x00)
				globals->dw403370 = edi_135 | 0x02;
		}
		if ((ecx_126 & 0x00100000) != 0x00)
		{
			globals->dw403010 |= 0x04;
			globals->dw40336C = 0x02;
			if ((ecx_126 & 0x08000000) != 0x00 && ((ecx_126 & 0x10000000) != 0x00 && (((word32) __xgetbv(0x00) & 0x06) == 0x06 && true)))
			{
				ui32 eax_189 = globals->dw403010;
				globals->dw40336C = 0x03;
				globals->dw403010 = eax_189 | 0x08;
				if ((bLoc14 & 0x20) != 0x00)
				{
					globals->dw40336C = 0x05;
					globals->dw403010 = eax_189 | 0x08 | 0x20;
				}
			}
		}
	}
}

// 00401B98: Register word32 fn00401B98()
word32 fn00401B98()
{
	return (word32) (globals->dw403014 != 0x00);
}

// 00401C46: void fn00401C46()
void fn00401C46()
{
}

