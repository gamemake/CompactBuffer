
namespace CompactBuffer
{
    public interface IProtocol
    {
    }

    public interface IProtocolSender
    {
        BufferWriter GetStreamWriter();
        void Send(BufferWriter writer);
    }

    public interface IProtocolStub
    {
        void Dispatch(BufferReader reader);
    }

    public interface IProtocolStub<T> : IProtocolStub
        where T : IProtocol
    {
    }

    public class Protocol
    {
        public static T GetProxy<T>()
            where T : IProtocol
        {
            return default(T);
        }

        public static IProtocolStub<T> GetStub<T>()
            where T : IProtocol
        {
            return default(IProtocolStub<T>);
        }

        public static IProtocolStub<T> GetDispacher<T>()
            where T : IProtocol
        {
            return default(IProtocolStub<T>);
        }
    }
}
