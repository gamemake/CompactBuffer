
using System;
using System.IO;

namespace CompactBuffer.Internal
{
    [CompactBuffer(typeof(sbyte))]
    public class SByteSerializer : ICompactBufferSerializer<sbyte>
    {
        public void Read(BinaryReader reader, ref sbyte target)
        {
            target = reader.ReadSByte();
        }

        public void Write(BinaryWriter writer, ref sbyte target)
        {
            writer.Write(target);
        }

        public void Copy(ref sbyte src, ref sbyte dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(short))]
    public class ShortSerializer : ICompactBufferSerializer<short>
    {
        public void Read(BinaryReader reader, ref short target)
        {
            target = reader.ReadInt16();
        }

        public void Write(BinaryWriter writer, ref short target)
        {
            writer.Write(target);
        }

        public void Copy(ref short src, ref short dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(int))]
    public class IntSerializer : ICompactBufferSerializer<int>
    {
        public void Read(BinaryReader reader, ref int target)
        {
            target = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer, ref int target)
        {
            writer.Write(target);
        }

        public void Copy(ref int src, ref int dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(long))]
    public class LongSerializer : ICompactBufferSerializer<long>
    {
        public void Read(BinaryReader reader, ref long target)
        {
            target = reader.ReadInt64();
        }

        public void Write(BinaryWriter writer, ref long target)
        {
            writer.Write(target);
        }

        public void Copy(ref long src, ref long dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(byte))]
    public class ByteSerializer : ICompactBufferSerializer<byte>
    {
        public void Read(BinaryReader reader, ref byte target)
        {
            target = reader.ReadByte();
        }

        public void Write(BinaryWriter writer, ref byte target)
        {
            writer.Write(target);
        }

        public void Copy(ref byte src, ref byte dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(ushort))]
    public class UShortSerializer : ICompactBufferSerializer<ushort>
    {
        public void Read(BinaryReader reader, ref ushort target)
        {
            target = reader.ReadUInt16();
        }

        public void Write(BinaryWriter writer, ref ushort target)
        {
            writer.Write(target);
        }

        public void Copy(ref ushort src, ref ushort dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(uint))]
    public class UIntSerializer : ICompactBufferSerializer<uint>
    {
        public void Read(BinaryReader reader, ref uint target)
        {
            target = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writer, ref uint target)
        {
            writer.Write(target);
        }

        public void Copy(ref uint src, ref uint dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(ulong))]
    public class ULongSerializer : ICompactBufferSerializer<ulong>
    {
        public void Read(BinaryReader reader, ref ulong target)
        {
            target = reader.ReadUInt64();
        }

        public void Write(BinaryWriter writer, ref ulong target)
        {
            writer.Write(target);
        }

        public void Copy(ref ulong src, ref ulong dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(float))]
    public class FloatSerializer : ICompactBufferSerializer<float>
    {
        public void Read(BinaryReader reader, ref float target)
        {
            target = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer, ref float target)
        {
            writer.Write(target);
        }

        public void Copy(ref float src, ref float dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(double))]
    public class DoubleSerializer : ICompactBufferSerializer<double>
    {
        public void Read(BinaryReader reader, ref double target)
        {
            target = reader.ReadDouble();
        }

        public void Write(BinaryWriter writer, ref double target)
        {
            writer.Write(target);
        }

        public void Copy(ref double src, ref double dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(bool))]
    public class BoolSerializer : ICompactBufferSerializer<bool>
    {
        public void Read(BinaryReader reader, ref bool target)
        {
            target = reader.ReadBoolean();
        }

        public void Write(BinaryWriter writer, ref bool target)
        {
            writer.Write(target);
        }

        public void Copy(ref bool src, ref bool dst)
        {
            dst = src;
        }
    }

   [CompactBuffer(typeof(string))]
    public class StringSerializer : ICompactBufferSerializer<string>
    {
        public void Read(BinaryReader reader, ref string target)
        {
            target = reader.ReadString();
        }

        public void Write(BinaryWriter writer, ref string target)
        {
            writer.Write(target);
        }

        public void Copy(ref string src, ref string dst)
        {
            dst = src;
        }
    }

    [CompactBuffer(typeof(Guid))]
    public class GuidSerializer : ICompactBufferSerializer<Guid>
    {
        public void Read(BinaryReader reader, ref Guid target)
        {
            var bytes = new byte[]
            {
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte(),
            };
            target = new Guid(bytes);
        }

        public void Write(BinaryWriter writer, ref Guid target)
        {
            foreach (var v in target.ToByteArray())
            {
                writer.Write(v);
            }
        }

        public void Copy(ref Guid src, ref Guid dst)
        {
            dst = src;
        }
    }
}
