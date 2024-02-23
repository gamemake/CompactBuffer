
namespace CompactBuffer
{
    public interface IProtocol
    {
    }

    public interface IProtocolSender
    {
        BufferWriter GetStreamWriter(int protocolId);
        void Send(BufferWriter writer, byte channel);
    }

    public interface IProtocolStub
    {
        void Dispatch(BufferReader reader);
    }

    public abstract class ProtocolProxy
    {
        protected readonly IProtocolSender m_Sender;

        public ProtocolProxy(IProtocolSender sender)
        {
            m_Sender = sender;
        }
    }
}
