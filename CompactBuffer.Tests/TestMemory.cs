

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CompactBuffer.Tests;

public struct Position{
    public float x;
    public float y;
    public float z;
}

public class TestMemory
{
    [Fact]
    unsafe void SpanFromUnmanagedMemory()
    {
        var count = 100;
        var memorySize = Marshal.SizeOf<Position>() * count;
        var ptr = Marshal.AllocHGlobal(memorySize);
        var span = new Span<Position>((void*)ptr, count);
        for(var i=0; i<span.Length;i++)
        {
            /*
            span[i].x = 100;
            span[i].y = 101;
            span[i].z = 102;
            */

            ref Position p = ref span[i];
            p.x = 100;
            p.y = 101;
            p.z = 102;
        }
        var pp = Marshal.PtrToStructure<Position>(ptr);
        Assert.Equal(100, pp.x);
        Assert.Equal(101, pp.y);
        Assert.Equal(102, pp.z);

        span = new Span<Position>((void*)(ptr + 4), count - 1);
        Assert.Equal(101, span[0].x);
        Assert.Equal(102, span[0].y);
        Assert.Equal(100, span[0].z);

        Marshal.FreeHGlobal(ptr);
    }

    [Fact]
    unsafe public void StructFromUnmanagedMemory()
    {
        var count = 100;
        var memorySize = Marshal.SizeOf<Position>() * count;
        var ptr = Marshal.AllocHGlobal(memorySize);
        ref var pp = ref Unsafe.AsRef<Position>((void*)ptr);
        pp.x = 1;
        pp.y = 2;
        pp.z = 3;
        Marshal.FreeHGlobal(ptr);
        var pp1 = Marshal.PtrToStructure<Position>(ptr);
        Assert.Equal(1, pp1.x);
        Assert.Equal(2, pp1.y);
        Assert.Equal(3, pp1.z);
    }
}
