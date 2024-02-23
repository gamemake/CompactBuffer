// Generate by CompactBuffer.CodeGen

#if !PROTOCOL_CLIENT && !PROTOCOL_SERVER
#define PROTOCOL_CLIENT
#define PROTOCOL_SERVER
#endif

namespace ProtocolAutoGen
{
    [CompactBuffer.Protocol(typeof(Tests.IServerApi))]
    public class Tests_IServerApi_Proxy : CompactBuffer.ProtocolProxy, Tests.IServerApi
    {
        public Tests_IServerApi_Proxy(CompactBuffer.IProtocolSender sender) : base(sender)
        {
        }

        void Tests.IServerApi.Call()
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(0);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallInt(int ___a)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(1);
            writer.Write(___a);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallIntArray(int[] ___array)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(2);
            CompactBuffer.Internal.ArraySerializer<int>.Write(writer, in ___array);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallString(string ___a)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(3);
            writer.Write(___a);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.Call7BitEncoded(int ___v1, long ___v2, uint ___v3, int ___v4, int ___v5)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(4);
            writer.Write7BitEncodedInt32(___v1);
            writer.Write7BitEncodedInt64(___v2);
            writer.Write(___v3);
            writer.Write(___v4);
            writer.Write(___v5);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallGuid(System.Guid ____guid)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(5);
            writer.Write(____guid);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallEnum(Tests.EnumTypes ____enum)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(6);
            writer.Write7BitEncodedInt32((int)____enum);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallFloat16(float ___v)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(7);
            writer.WriteFloat16(___v, 1);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallTypeClass(Tests.TypeClass ___pa)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(8);
            CompactBufferAutoGen.Tests_TypeClass_Serializer.Write(writer, in ___pa);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallTypeClassArray(Tests.TypeClass[] ___pa)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(9);
            CompactBuffer.Internal.ArraySerializer<Tests.TypeClass>.Write(writer, in ___pa);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallTypeClassIn(in Tests.TypeClass ___a)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(10);
            CompactBufferAutoGen.Tests_TypeClass_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallTypeClassRef(ref Tests.TypeClass ___a)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(11);
            CompactBufferAutoGen.Tests_TypeClass_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallTypeStruct(Tests.TypeStruct ___a)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(12);
            CompactBufferAutoGen.Tests_TypeStruct_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallTypeStructIn(in Tests.TypeStruct ___a)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(13);
            CompactBufferAutoGen.Tests_TypeStruct_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallTypeStructRef(ref Tests.TypeStruct ___a)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(14);
            CompactBufferAutoGen.Tests_TypeStruct_Serializer.Write(writer, in ___a);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallIntSpan(System.Span<int> ___span)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(15);
            CompactBuffer.Internal.SpanSerializer<int>.Write(writer, in ___span);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallIntReadOnlySpan(System.ReadOnlySpan<int> ___span)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(16);
            CompactBuffer.Internal.ReadOnlySpanSerializer<int>.Write(writer, in ___span);
            m_Sender.Send(writer, 255);
        }

        void Tests.IServerApi.CallReadOnlySpan(System.ReadOnlySpan<byte> ___aaaaaaaa)
        {
            var writer = m_Sender.GetStreamWriter(0);
            writer.Write7BitEncodedInt32(17);
            CompactBuffer.Internal.ReadOnlySpanSerializer<byte>.Write(writer, in ___aaaaaaaa);
            m_Sender.Send(writer, 255);
        }
    }
}
namespace ProtocolAutoGen
{
    [CompactBuffer.Protocol(typeof(Tests.IServerApi))]
    public class Tests_IServerApi_Stub : CompactBuffer.IProtocolStub
    {
        protected readonly Tests.IServerApi m_Target;

        public Tests_IServerApi_Stub(Tests.IServerApi target = null)
        {
            m_Target = target;
        }

        void Dispatch(CompactBuffer.BufferReader reader)
        {
            var index = reader.Read7BitEncodedInt32();
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
                int[] ___array = default;
                CompactBuffer.Internal.ArraySerializer<int>.Read(reader, ref ___array);
                m_Target?.CallIntArray(___array);
                return;
            }
            if (index == 3)
            {
                var ___a = reader.ReadString();
                m_Target?.CallString(___a);
                return;
            }
            if (index == 4)
            {
                var ___v1 = reader.Read7BitEncodedInt32();
                var ___v2 = reader.Read7BitEncodedInt64();
                var ___v3 = reader.ReadUInt32();
                var ___v4 = reader.ReadInt32();
                var ___v5 = reader.ReadInt32();
                m_Target?.Call7BitEncoded(___v1, ___v2, ___v3, ___v4, ___v5);
                return;
            }
            if (index == 5)
            {
                var ____guid = reader.ReadGuid();
                m_Target?.CallGuid(____guid);
                return;
            }
            if (index == 6)
            {
                var ____enum = (Tests.EnumTypes)reader.Read7BitEncodedInt32();
                m_Target?.CallEnum(____enum);
                return;
            }
            if (index == 7)
            {
                var ___v = reader.ReadFloat16(1);
                m_Target?.CallFloat16(___v);
                return;
            }
            if (index == 8)
            {
                Tests.TypeClass ___pa = default;
                CompactBufferAutoGen.Tests_TypeClass_Serializer.Read(reader, ref ___pa);
                m_Target?.CallTypeClass(___pa);
                return;
            }
            if (index == 9)
            {
                Tests.TypeClass[] ___pa = default;
                CompactBuffer.Internal.ArraySerializer<Tests.TypeClass>.Read(reader, ref ___pa);
                m_Target?.CallTypeClassArray(___pa);
                return;
            }
            if (index == 10)
            {
                Tests.TypeClass ___a = default;
                CompactBufferAutoGen.Tests_TypeClass_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeClassIn(in ___a);
                return;
            }
            if (index == 11)
            {
                Tests.TypeClass ___a = default;
                CompactBufferAutoGen.Tests_TypeClass_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeClassRef(ref ___a);
                return;
            }
            if (index == 12)
            {
                Tests.TypeStruct ___a = default;
                CompactBufferAutoGen.Tests_TypeStruct_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeStruct(___a);
                return;
            }
            if (index == 13)
            {
                Tests.TypeStruct ___a = default;
                CompactBufferAutoGen.Tests_TypeStruct_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeStructIn(in ___a);
                return;
            }
            if (index == 14)
            {
                Tests.TypeStruct ___a = default;
                CompactBufferAutoGen.Tests_TypeStruct_Serializer.Read(reader, ref ___a);
                m_Target?.CallTypeStructRef(ref ___a);
                return;
            }
            if (index == 15)
            {
                System.Span<int> ___span = default;
                CompactBuffer.Internal.SpanSerializer<int>.Read(reader, ref ___span);
                m_Target?.CallIntSpan(___span);
                return;
            }
            if (index == 16)
            {
                System.ReadOnlySpan<int> ___span = default;
                CompactBuffer.Internal.ReadOnlySpanSerializer<int>.Read(reader, ref ___span);
                m_Target?.CallIntReadOnlySpan(___span);
                return;
            }
            if (index == 17)
            {
                System.ReadOnlySpan<byte> ___aaaaaaaa = default;
                CompactBuffer.Internal.ReadOnlySpanSerializer<byte>.Read(reader, ref ___aaaaaaaa);
                m_Target?.CallReadOnlySpan(___aaaaaaaa);
                return;
            }
            throw new CompactBuffer.CompactBufferExeption("Tests.IServerApi invalid method index" + index);
        }

        void CompactBuffer.IProtocolStub.Dispatch(CompactBuffer.BufferReader reader)
        {
            var top = CompactBuffer.Internal.SpanAllocator.Begin();
            try
            {
                Dispatch(reader);
            }
            finally
            {
                CompactBuffer.Internal.SpanAllocator.End(top);
            }
        }
    }
}
