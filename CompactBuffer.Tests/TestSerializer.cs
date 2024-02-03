
using System;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Collections.Generic;
using Tests;

namespace CompactBuffer.Tests;

public class TestSerializer
{
    private readonly BufferReader m_Reader;
    private readonly BufferWriter m_Writer;
    private JsonSerializerOptions m_JsonOptions = new JsonSerializerOptions { IncludeFields = true };

    public TestSerializer()
    {
        var buffer = new byte[400 * 1024];
        m_Reader = new BufferReader(buffer);
        m_Writer = new BufferWriter(buffer);
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
        Serializers.Get<AAA>().Write(m_Writer, in src);
        Serializers.Get<AAA>().Read(m_Reader, ref dst);
        Assert.Equal(m_Reader.Position, m_Writer.Position);

        var srcJson = JsonSerializer.Serialize(src, m_JsonOptions);
        var dstJson = JsonSerializer.Serialize(dst, m_JsonOptions);
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

    [Fact]
    public void BucketIndexAndSize()
    {
        uint[] sizes = { 1, 2, 4, 100, 254, 1000, 2000, 4000 };
        foreach (var size in sizes)
        {
            var bucketIndex = BitOperations.Log2(size - 1 | 15) - 3;
            var bucketMaxSize = 16 << bucketIndex;
            Assert.NotEqual(0, bucketMaxSize);
        }
    }
}
