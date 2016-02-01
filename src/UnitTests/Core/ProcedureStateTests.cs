﻿#region License
/* 
 * Copyright (C) 1999-2016 John Källén.
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

using Reko.Scanning;
using Reko.Core;
using Reko.Core.Code;
using Reko.Core.Expressions;
using Reko.Core.Machine;
using Reko.Core.Rtl;
using Reko.Core.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Reko.Core.Serialization;

namespace Reko.UnitTests.Core
{
    [TestFixture]
    public class ProcedureStateTests
    {
        private RegisterStorage sp;
        private FakeArchitecture arch;
        private FakeProcessorState sce;
        private Identifier idSp;
        private ExpressionEmitter m;

        [SetUp]
        public void Setup()
        {
            sp = new RegisterStorage("sp", 42, 0, PrimitiveType.Pointer32);
            arch = new FakeArchitecture();
            arch.StackRegister = sp;

            sce = new FakeProcessorState(arch);

            idSp = new Identifier(sp.Name, sp.DataType, sp);
            m = new ExpressionEmitter();
        }

        [Test]
        public void SetValue()
        {
            sce.SetValue(idSp, m.ISub(idSp, 4));

            Assert.AreEqual("sp - 0x00000004", sce.GetValue(idSp).ToString());
        }

        [Test]
        public void PushValueOnstack()
        {
            sce.SetValue(idSp, m.ISub(idSp, 4));
            sce.SetValueEa(idSp, Constant.Word32(0x12345678));

            Assert.AreEqual("0x12345678", sce.GetValue(m.LoadDw(idSp)).ToString());
        }

        public class FakeArchitecture : IProcessorArchitecture
        {
            #region IProcessorArchitecture Members

            public string Name { get; set; }
            public string Description { get; set; }

            public IEnumerable<MachineInstruction> CreateDisassembler(ImageReader imageReader)
            {
                throw new NotImplementedException();
            }

            public IEqualityComparer<MachineInstruction> CreateInstructionComparer(Normalize norm)
            {
                throw new NotImplementedException();
            }

            public ProcessorState CreateProcessorState()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Address> CreatePointerScanner(ImageMap map, ImageReader rdr, IEnumerable<Address> knownLinAddrs, PointerScannerFlags flags)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<RtlInstructionCluster> CreateRewriter(ImageReader rdr, ProcessorState state, Frame frame, IRewriterHost host)
            {
                throw new NotImplementedException();
            }

            public Frame CreateFrame()
            {
                throw new NotImplementedException();
            }

            public ImageReader CreateImageReader(MemoryArea image, Address addr)
            {
                return new LeImageReader(image, addr);
            }

            public ImageReader CreateImageReader(MemoryArea image, ulong offset)
            {
                return new LeImageReader(image, offset);
            }

            public ProcedureSerializer CreateProcedureSerializer(ISerializedTypeVisitor<DataType> typeLoader, string defaultCc)
            {
                throw new NotImplementedException();
            }

            public ProcedureBase GetTrampolineDestination(ImageReader rdr, IRewriterHost host)
            {
                return null;
            }


            public RegisterStorage GetRegister(int i)
            {
                throw new NotImplementedException();
            }

            public RegisterStorage GetRegister(string name)
            {
                throw new NotImplementedException();
            }

            public RegisterStorage[] GetRegisters()
            {
                throw new NotImplementedException();
            }

            public bool TryGetRegister(string name, out RegisterStorage reg)
            {
                throw new NotImplementedException();
            }

            public FlagGroupStorage GetFlagGroup(uint grf)
            {
                throw new NotImplementedException();
            }

            public FlagGroupStorage GetFlagGroup(string name)
            {
                throw new NotImplementedException();
            }

            public Expression CreateStackAccess(Frame frame, int cbOffset, DataType dataType)
            {
                throw new NotImplementedException();
            }

            public Address MakeAddressFromConstant(Constant c)
            {
                return Address.Ptr32(c.ToUInt32());
            }

            public Address ReadCodeAddress(int size, ImageReader rdr, ProcessorState state)
            {
                throw new NotImplementedException();
            }

            public int InstructionBitSize { get { return 32; } }

            public string GrfToString(uint grf)
            {
                throw new NotImplementedException();
            }

            public PrimitiveType FramePointerType
            {
                get { throw new NotImplementedException(); }
            }

            public PrimitiveType PointerType
            {
                get { throw new NotImplementedException(); }
            }

            public PrimitiveType WordWidth
            {
                get { throw new NotImplementedException(); }
            }

            public RegisterStorage StackRegister { get; set; }

            public uint CarryFlagMask
            {
                get { throw new NotImplementedException(); }
            }

            public bool TryParseAddress(string txtAddress, out Address addr)
            {
                return Address.TryParse32(txtAddress, out addr);
            }

            public Address MakeSegmentedAddress(Constant seg, Constant offset)
            {
                throw new NotImplementedException();
            }

            public RegisterStorage GetSubregister(RegisterStorage reg, int offset, int width)
            {
                throw new NotImplementedException();
            }

            public RegisterStorage GetPart(RegisterStorage reg, DataType width)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<RegisterStorage> GetAliases(RegisterStorage reg)
            {
                throw new NotImplementedException();
            }

            public RegisterStorage GetWidestSubregister(RegisterStorage reg, HashSet<RegisterStorage> bits)
            {
                throw new NotImplementedException();
            }

            public void RemoveAliases(ISet<RegisterStorage> ids, RegisterStorage reg)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class FakeProcessorState : ProcessorState
        {
            private IProcessorArchitecture arch;
            private Dictionary<RegisterStorage, Constant> regs = new Dictionary<RegisterStorage, Constant>();
            private SortedList<int, Constant> stack = new SortedList<int, Constant>();

            public FakeProcessorState(IProcessorArchitecture arch)
            {
                this.arch = arch;
            }

            public override IProcessorArchitecture Architecture { get { return arch; } }

            #region ProcessorState Members

            public override ProcessorState Clone()
            {
                throw new NotImplementedException();
            }

            public override Constant GetRegister(RegisterStorage r)
            {
                Constant c;
                if (!regs.TryGetValue(r, out c))
                    c = Constant.Invalid;
                return c;
            }

            public override void SetRegister(RegisterStorage r, Constant v)
            {
                throw new NotImplementedException();
            }

            public override void SetInstructionPointer(Address addr)
            {
                throw new NotImplementedException();
            }

            public override void OnProcedureEntered()
            {
                throw new NotImplementedException();
            }

            public override void OnProcedureLeft(ProcedureSignature procedureSignature)
            {
                throw new NotImplementedException();
            }

            public override CallSite OnBeforeCall(Identifier stackReg, int returnAddressSize)
            {
                throw new NotImplementedException();
            }

            public override void OnAfterCall(ProcedureSignature sigCallee)
            {
            }

            #endregion
        }
    }
}
