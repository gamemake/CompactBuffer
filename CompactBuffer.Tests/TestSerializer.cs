
using System.IO;
using System.Text.Json;

namespace CompactBuffer.Tests;

public class TestSerializer
{
    private readonly MemoryStream m_ReaderStream;
    private readonly MemoryStream m_WriterStream;
    private readonly BinaryReader m_Reader;
    private readonly BinaryWriter m_Writer;

    public TestSerializer()
    {
        var buffer = new byte[400 * 1024];
        m_ReaderStream = new MemoryStream(buffer);
        m_WriterStream = new MemoryStream(buffer);
        m_Reader = new BinaryReader(m_ReaderStream);
        m_Writer = new BinaryWriter(m_WriterStream);
    }

    [Fact]
    public void Test1()
    {
        var src = new Test.AAA();
        var dst = default(Test.AAA);
        CompactBuffer.GetSerializer<Test.AAA>().Write(m_Writer, ref src);
        CompactBuffer.GetSerializer<Test.AAA>().Read(m_Reader, ref dst);
        Assert.Equal(m_ReaderStream.Position, m_WriterStream.Position);

        var srcJson = JsonSerializer.Serialize(src);
        var dstJson = JsonSerializer.Serialize(dst);
        Assert.Equal(srcJson, dstJson);
    }
}
