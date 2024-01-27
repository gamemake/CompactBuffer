
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

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
        var src = new Test.AAA()
        {
            _sbyte = 12,
            _short = 15,
            _int = 17,
            _long = 19,
            _byte = 21,
            _ushort = 23,
            _uint = 27,
            _ulong = 29,
            _bool = true,
            _string = "",
            i = 1999,
            vvv = null,
            vvv0 = new int[0],
            vvv1 = new int[1],
            vvv10 = new int[10],
            list = null,
            list0 = new List<int>(),
            list1 = new List<int>(Enumerable.Repeat(10, 1)),
            list10 = new List<int>(Enumerable.Repeat(66, 10)),
        };
        var dst = default(Test.AAA);
        CompactBuffer.GetSerializer<Test.AAA>().Write(m_Writer, ref src);
        CompactBuffer.GetSerializer<Test.AAA>().Read(m_Reader, ref dst);
        Assert.Equal(m_ReaderStream.Position, m_WriterStream.Position);

        var srcJson = JsonSerializer.Serialize(src);
        var dstJson = JsonSerializer.Serialize(dst);
        Assert.Equal(srcJson, dstJson);
    }
}
