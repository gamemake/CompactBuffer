
using System.IO;

namespace CompactBuffer
{
    public interface IProtocol
    {
    }

    public interface IProtocolSender
    {
        BinaryWriter GetStreamWriter();
        void Send(BinaryWriter writer);
    }

    public interface IProtocolStub
    {
        void Dispatch(BinaryReader reader);
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
