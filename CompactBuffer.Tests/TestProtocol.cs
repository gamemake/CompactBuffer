
using Tests;

namespace CompactBuffer.Tests;

public class TestProtocol : IProtocolSender
{
    private byte[] m_Bytes = null;
    private int m_Length = 0;

    BufferWriter IProtocolSender.GetStreamWriter(int protocolId)
    {
        m_Bytes = new byte[100000];
        return new BufferWriter(m_Bytes);
    }

    void IProtocolSender.Send(BufferWriter writer)
    {
        m_Length = writer.Length;
    }

    [Fact]
    void ProxyAndStub()
    {
        var proxy = Protocol.GetProxy<IServerApi>(this);
        var stub = Protocol.GetStub<IServerApi>(null);
        proxy.Call();
        var reader = new BufferReader(m_Bytes, 0, m_Length);
        stub.Dispatch(reader);
        Assert.Equal(reader.Position, m_Length);
    }
}
