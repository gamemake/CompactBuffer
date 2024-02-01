
using System;

namespace CompactBuffer.Internal
{
    [CompactBuffer(typeof(sbyte))]
    public class SByteSerializer : ICompactBufferSerializer<sbyte>
    {
        public static void Read(BufferReader reader, ref sbyte target)
        {
            target = reader.ReadSByte();
        }

        public static void Write(BufferWriter writer, ref readonly sbyte target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly sbyte src, ref sbyte dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<sbyte>.Read(BufferReader reader, ref sbyte target)
        {
            target = reader.ReadSByte();
        }

        void ICompactBufferSerializer<sbyte>.Write(BufferWriter writer, ref readonly sbyte target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<sbyte>.Copy(ref readonly sbyte src, ref sbyte dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(short))]
    public class ShortSerializer : ICompactBufferSerializer<short>
    {
        public static void Read(BufferReader reader, ref short target)
        {
            target = reader.ReadInt16();
        }

        public static void Write(BufferWriter writer, ref readonly short target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly short src, ref short dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<short>.Read(BufferReader reader, ref short target)
        {
            target = reader.ReadInt16();
        }

        void ICompactBufferSerializer<short>.Write(BufferWriter writer, ref readonly short target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<short>.Copy(ref readonly short src, ref short dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(int))]
    public class IntSerializer : ICompactBufferSerializer<int>
    {
        public static void Read(BufferReader reader, ref int target)
        {
            target = reader.ReadInt32();
        }

        public static void Write(BufferWriter writer, ref readonly int target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly int src, ref int dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<int>.Read(BufferReader reader, ref int target)
        {
            target = reader.ReadInt32();
        }

        void ICompactBufferSerializer<int>.Write(BufferWriter writer, ref readonly int target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<int>.Copy(ref readonly int src, ref int dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(long))]
    public class LongSerializer : ICompactBufferSerializer<long>
    {
        public static void Read(BufferReader reader, ref long target)
        {
            target = reader.ReadInt64();
        }

        public static void Write(BufferWriter writer, ref readonly long target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly long src, ref long dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<long>.Read(BufferReader reader, ref long target)
        {
            target = reader.ReadInt64();
        }

        void ICompactBufferSerializer<long>.Write(BufferWriter writer, ref readonly long target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<long>.Copy(ref readonly long src, ref long dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(byte))]
    public class ByteSerializer : ICompactBufferSerializer<byte>
    {
        public static void Read(BufferReader reader, ref byte target)
        {
            target = reader.ReadByte();
        }

        public static void Write(BufferWriter writer, ref readonly byte target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly byte src, ref byte dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<byte>.Read(BufferReader reader, ref byte target)
        {
            target = reader.ReadByte();
        }

        void ICompactBufferSerializer<byte>.Write(BufferWriter writer, ref readonly byte target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<byte>.Copy(ref readonly byte src, ref byte dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(ushort))]
    public class UShortSerializer : ICompactBufferSerializer<ushort>
    {
        public static void Read(BufferReader reader, ref ushort target)
        {
            target = reader.ReadUInt16();
        }

        public static void Write(BufferWriter writer, ref readonly ushort target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly ushort src, ref ushort dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<ushort>.Read(BufferReader reader, ref ushort target)
        {
            target = reader.ReadUInt16();
        }

        void ICompactBufferSerializer<ushort>.Write(BufferWriter writer, ref readonly ushort target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<ushort>.Copy(ref readonly ushort src, ref ushort dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(uint))]
    public class UIntSerializer : ICompactBufferSerializer<uint>
    {
        public static void Read(BufferReader reader, ref uint target)
        {
            target = reader.ReadUInt32();
        }

        public static void Write(BufferWriter writer, ref readonly uint target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly uint src, ref uint dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<uint>.Read(BufferReader reader, ref uint target)
        {
            target = reader.ReadUInt32();
        }

        void ICompactBufferSerializer<uint>.Write(BufferWriter writer, ref readonly uint target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<uint>.Copy(ref readonly uint src, ref uint dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(ulong))]
    public class ULongSerializer : ICompactBufferSerializer<ulong>
    {
        public static void Read(BufferReader reader, ref ulong target)
        {
            target = reader.ReadUInt64();
        }

        public static void Write(BufferWriter writer, ref readonly ulong target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly ulong src, ref ulong dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<ulong>.Read(BufferReader reader, ref ulong target)
        {
            target = reader.ReadUInt64();
        }

        void ICompactBufferSerializer<ulong>.Write(BufferWriter writer, ref readonly ulong target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<ulong>.Copy(ref readonly ulong src, ref ulong dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(float))]
    public class FloatSerializer : ICompactBufferSerializer<float>
    {
        public static void Read(BufferReader reader, ref float target)
        {
            target = reader.ReadSingle();
        }

        public static void Write(BufferWriter writer, ref readonly float target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly float src, ref float dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<float>.Read(BufferReader reader, ref float target)
        {
            target = reader.ReadSingle();
        }


        void ICompactBufferSerializer<float>.Write(BufferWriter writer, ref readonly float target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<float>.Copy(ref readonly float src, ref float dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(double))]
    public class DoubleSerializer : ICompactBufferSerializer<double>
    {
        public static void Read(BufferReader reader, ref double target)
        {
            target = reader.ReadDouble();
        }

        public static void Write(BufferWriter writer, ref readonly double target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly double src, ref double dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<double>.Read(BufferReader reader, ref double target)
        {
            target = reader.ReadDouble();
        }

        void ICompactBufferSerializer<double>.Write(BufferWriter writer, ref readonly double target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<double>.Copy(ref readonly double src, ref double dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(bool))]
    public class BoolSerializer : ICompactBufferSerializer<bool>
    {
        public static void Read(BufferReader reader, ref bool target)
        {
            target = reader.ReadBoolean();
        }

        public static void Write(BufferWriter writer, ref readonly bool target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly bool src, ref bool dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<bool>.Read(BufferReader reader, ref bool target)
        {
            target = reader.ReadBoolean();
        }

        void ICompactBufferSerializer<bool>.Write(BufferWriter writer, ref readonly bool target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<bool>.Copy(ref readonly bool src, ref bool dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(string))]
    public class StringSerializer : ICompactBufferSerializer<string>
    {
        public static void Read(BufferReader reader, ref string target)
        {
            target = reader.ReadString();
        }

        public static void Write(BufferWriter writer, ref string target)
        {
            writer.Write(target);
        }

        public static void Copy(ref string src, ref string dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<string>.Read(BufferReader reader, ref string target)
        {
            target = reader.ReadString();
        }

        void ICompactBufferSerializer<string>.Write(BufferWriter writer, ref readonly string target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<string>.Copy(ref readonly string src, ref string dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(Guid))]
    public class GuidSerializer : ICompactBufferSerializer<Guid>
    {
        public static void Read(BufferReader reader, ref Guid target)
        {
            target = reader.ReadGuid();
        }

        public static void Write(BufferWriter writer, ref readonly Guid target)
        {
            writer.Write(target);
        }

        public static void Copy(ref readonly Guid src, ref Guid dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<Guid>.Read(BufferReader reader, ref Guid target)
        {
            target = reader.ReadGuid();
        }

        void ICompactBufferSerializer<Guid>.Write(BufferWriter writer, ref readonly Guid target)
        {
            writer.Write(target);
        }

        void ICompactBufferSerializer<Guid>.Copy(ref readonly Guid src, ref Guid dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(ReadOnlySpan<byte>))]
    public class ReadOnlySpanByteSerializer : ICompactBufferSerializer
    {
        public static void Read(BufferReader reader, ref ReadOnlySpan<byte> target)
        {
            var length = reader.ReadVariantInt32();
            target = reader.ReadBytes(length);
        }

        public static void Write(BufferWriter writer, ref readonly ReadOnlySpan<byte> target)
        {
            writer.WriteVariantInt32(target.Length);
            writer.Write(target);
        }

        public static void Copy(ref readonly ReadOnlySpan<byte> src, ref readonly ReadOnlySpan<byte> dst)
        {
            throw new NotImplementedException();
        }
    }
}
