// Generate by CompactBuffer.CodeGen

#if !PROTOCOL_CLIENT && !PROTOCOL_SERVER
#define PROTOCOL_CLIENT
#define PROTOCOL_SERVER
#endif

namespace ProtocolAutoGen
{
    public class Tests_IServerApi_Proxy : CompactBuffer.ProtocolProxy, Tests.IServerApi
    {
        public Tests_IServerApi_Proxy(CompactBuffer.IProtocolSender sender) : base(sender)
        {
        }

        void Tests.IServerApi.Call()
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)0);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallInt(int ___a)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)1);
            writer.Write(___a);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallPA(Tests.TypeClass ___pa)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)2);
            CompactBufferAutoGen.Tests_TypeClass_Serializer.Write(writer, in ___pa);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallVariant(int ___v1, long ___v2, uint ___v3, int ___v4, int ___v5)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)3);
            writer.WriteVariantInt32(___v1);
            writer.WriteVariantInt64(___v2);
            writer.Write(___v3);
            writer.Write(___v4);
            writer.Write(___v5);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallGuid(System.Guid ____guid)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)4);
            writer.Write(____guid);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallEnum(Tests.EnumTypes ____enum)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)5);
            writer.WriteVariantInt32((int)____enum);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallFloat16(float ___v)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)6);
            writer.WriteFloat16(___v, 1);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallReadOnlySpan(System.ReadOnlySpan<byte> ___aaaaaaaa)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)7);
            CompactBuffer.Internal.ReadOnlySpanByteSerializer.Write(writer, in ___aaaaaaaa);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallVaiantType(Tests.VaiantType ___vv)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)8);
            Tests.VaiantTypeSerializer.Write(writer, in ___vv);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallTypeClassRefReadonly(ref readonly Tests.TypeClass ___a)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)9);
            CompactBufferAutoGen.Tests_TypeClass_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer);
        }

        void Tests.IServerApi.CallTypeClassRef(ref Tests.TypeClass ___a)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)10);
            CompactBufferAutoGen.Tests_TypeClass_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer);
        }


        void Tests.IServerApi.CallTypeStructRefReadonly(ref readonly Tests.TypeStruct ___a)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)11);
            CompactBufferAutoGen.Tests_TypeStruct_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer);
        }


        void Tests.IServerApi.CallTypeStructRef(ref Tests.TypeStruct ___a)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)12);
            CompactBufferAutoGen.Tests_TypeStruct_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer);
        }
    }
}
namespace ProtocolAutoGen
{
    public class Tests_IServerApi_Stub : CompactBuffer.IProtocolStub
    {
        protected readonly Tests.IServerApi m_Target;

        public Tests_IServerApi_Stub(Tests.IServerApi target = null)
        {
            m_Target = target;
        }

        void CompactBuffer.IProtocolStub.Dispatch(CompactBuffer.BufferReader reader)
        {
            var index = reader.ReadUInt32();
            if (index == 0)
            {
                m_Target?.Call();
                return;
            }
            if (index == 1)
            {
                var ___a = reader.ReadInt32();
                m_Target?.CallInt(___a);
                return;
            }
            if (index == 2)
            {
                Tests.TypeClass ___pa = default;
                CompactBufferAutoGen.Tests_TypeClass_Serializer.Read(reader, ref ___pa);
                m_Target?.CallPA(___pa);
                return;
            }
            if (index == 3)
            {
                var ___v1 = reader.ReadVariantInt32();
                var ___v2 = reader.ReadVariantInt64();
                var ___v3 = reader.ReadUInt32();
                var ___v4 = reader.ReadInt32();
                var ___v5 = reader.ReadInt32();
                m_Target?.CallVariant(___v1, ___v2, ___v3, ___v4, ___v5);
                return;
            }
            if (index == 4)
            {
                var ____guid = reader.ReadGuid();
                m_Target?.CallGuid(____guid);
                return;
            }
            if (index == 5)
            {
                var ____enum = (Tests.EnumTypes)reader.ReadVariantInt32();
                m_Target?.CallEnum(____enum);
                return;
            }
            if (index == 6)
            {
                var ___v = reader.ReadFloat16(1);
                m_Target?.CallFloat16(___v);
                return;
            }
            if (index == 7)
            {
                System.ReadOnlySpan<byte> ___aaaaaaaa = default;
                CompactBuffer.Internal.ReadOnlySpanByteSerializer.Read(reader, ref ___aaaaaaaa);
                m_Target?.CallReadOnlySpan(___aaaaaaaa);
                return;
            }
            if (index == 8)
            {
                Tests.VaiantType ___vv = default;
                Tests.VaiantTypeSerializer.Read(reader, ref ___vv);
                m_Target?.CallVaiantType(___vv);
                return;
            }
            if (index == 9)
            {
                Tests.TypeClass ___a = default;
                CompactBufferAutoGen.Tests_TypeClass_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeClassRefReadonly(in ___a);
                return;
            }
            if (index == 10)
            {
                Tests.TypeClass ___a = default;
                CompactBufferAutoGen.Tests_TypeClass_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeClassRef(ref ___a);
                return;
            }
            if (index == 11)
            {
                Tests.TypeStruct ___a = default;
                CompactBufferAutoGen.Tests_TypeStruct_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeStructRefReadonly(in ___a);
                return;
            }
            if (index == 12)
            {
                Tests.TypeStruct ___a = default;
                CompactBufferAutoGen.Tests_TypeStruct_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeStructRef(ref ___a);
                return;
            }
            throw new CompactBuffer.CompactBufferExeption("Tests.IServerApi invalid method index" + index);
        }
    }
}
