// Generate by CompactBuffer.CodeGen

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

        void CompactBuffer.IProtocolStub.Dispatch(System.IO.BinaryReader reader)
        {
            var index = reader.ReadUInt32();
            if (index == 0)
            {
                m_Target?.Call();
                return;
            }
            if (index == 1)
            {
                int ___a;
                ___a = reader.ReadInt32();
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
            throw new System.Exception("Test.IServerApi invalid method index" + index);
        }
    }
}
