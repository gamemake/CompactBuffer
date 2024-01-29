// Generate by CompactBuffer.CodeGen

#if !PROTOCOL_CLIENT && !PROTOCOL_SERVER
#define PROTOCOL_CLIENT
#define PROTOCOL_SERVER
#endif

namespace ProtocolAutoGen
{
    public class Test_IServerApi_Proxy : Test.IServerApi
    {
        private readonly CompactBuffer.IProtocolSender m_Sender;

        public Test_IServerApi_Proxy(CompactBuffer.IProtocolSender sender)
        {
            m_Sender = sender;
        }

        void Test.IServerApi.Call()
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)0);
            m_Sender.Send(writer);
        }

        void Test.IServerApi.CallInt(int ___a)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)1);
            writer.Write(___a);
            m_Sender.Send(writer);
        }

        void Test.IServerApi.CallPA(Test.PA ___pa)
        {
            var writer = m_Sender.GetStreamWriter();
            writer.Write((ushort)2);
            CompactBufferAutoGen.Test_PA_Serializer.Write(writer, ref ___pa);
            m_Sender.Send(writer);
        }

        void Test.IServerApi.CallVariant(int ___v1, long ___v2, uint ___v3, int ___v4, int ___v5)
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
    }
}
namespace ProtocolAutoGen
{
    public class Test_IServerApi_Stub : CompactBuffer.IProtocolStub
    {
        protected readonly Test.IServerApi m_Target;

        public Test_IServerApi_Stub(Test.IServerApi target = null)
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
                Test.PA ___pa = default;
                CompactBufferAutoGen.Test_PA_Serializer.Read(reader, ref ___pa);
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
            throw new System.Exception("Test.IServerApi invalid method index" + index);
        }
    }
}
