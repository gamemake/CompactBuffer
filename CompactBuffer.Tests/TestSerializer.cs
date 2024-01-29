
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Tests;

namespace CompactBuffer.Tests;

public class TestSerializer
{
    private readonly MemoryStream m_ReaderStream;
    private readonly MemoryStream m_WriterStream;
    private readonly BufferReader m_Reader;
    private readonly BufferWriter m_Writer;

    public TestSerializer()
    {
        var buffer = new byte[400 * 1024];
        m_ReaderStream = new MemoryStream(buffer);
        m_WriterStream = new MemoryStream(buffer);
        m_Reader = new BufferReader(m_ReaderStream);
        m_Writer = new BufferWriter(m_WriterStream);
    }

    [Fact]
    public void ClassSerializer()
    {
        var src = new AAA()
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
        var dst = default(AAA);
        CompactBuffer.GetSerializer<AAA>().Write(m_Writer, ref src);
        CompactBuffer.GetSerializer<AAA>().Read(m_Reader, ref dst);
        Assert.Equal(m_ReaderStream.Position, m_WriterStream.Position);

        var srcJson = JsonSerializer.Serialize(src);
        var dstJson = JsonSerializer.Serialize(dst);
        Assert.Equal(srcJson, dstJson);
    }

    public static short WriteFloat16(float floatValue, int integerMax)
    {
        return (short)(floatValue / integerMax * short.MaxValue);
    }

    public static float ReadFloat16(short shortValue, int integerMax)
    {
        return shortValue / (float)short.MaxValue * integerMax;
    }

    [Fact]
    public void FloatTwoByte()
    {
        float[] srcs = { 0.1f, 1f, 0.5f, -1f, -0.5f };
        foreach (var src in srcs)
        {
            for (var integerMax = 1; integerMax < 1000; integerMax++)
            {
                var _short = WriteFloat16(src, integerMax);
                var dst = ReadFloat16(_short, integerMax);
                var diff = Math.Abs(dst - src);
                var diffMax = 0.0001f * integerMax;
                Assert.True(diff < diffMax);
            }
        }
    }
}
