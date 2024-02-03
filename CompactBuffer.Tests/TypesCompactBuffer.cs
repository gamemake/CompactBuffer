
using System;
using System.Collections.Generic;
using CompactBuffer;

namespace Tests
{
    public enum EnumTypes
    {
        Int,
        Long,
    }

    [CompactBufferGenCode]
    public class AAA
    {
        public sbyte _sbyte;
        public short _short;
        public int _int;
        public long _long;
        public byte _byte;
        public ushort _ushort;
        public uint _uint;
        public ulong _ulong;
        public bool _bool;
        public string _string;
        public int i = 0;
        public int[] vvv = null;
        public int[] vvv0 = null;
        public int[] vvv1 = null;
        public int[] vvv10 = null;
        public List<int> list = null;
        public List<int> list0 = null;
        public List<int> list1 = null;
        public List<int> list10 = null;
        [Variant]
        public int variantInt = 0;
        [Variant]
        public long variantLong = 10;
        [Variant]
        public uint variantUInt = 999;
        [Float16(10)]
        public float floatTwoByte = 0f;
        public Guid guid;
        public EnumTypes enum0;
    }

    [CompactBufferGenCode]
    public class BBB
    {
        public int i = 0;
    }

    [CompactBufferGenCode]
    public struct CCC
    {
        public int i;

        [CustomSerializer(typeof(CustomFloatSerializer))]
        public float customFloat;
    }

    public class CustomFloatSerializer : ICompactBufferSerializer<float>
    {
        public static void Read(BufferReader reader, ref float target)
        {
            target = (float)reader.ReadInt32();
        }

        public static void Write(BufferWriter writer, in float target)
        {
            writer.Write((int)target);
        }

        public static void Copy(in float src, ref float dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<float>.Read(BufferReader reader, ref float target)
        {
            target = (float)reader.ReadInt32();
        }

        void ICompactBufferSerializer<float>.Write(BufferWriter writer, in float target)
        {
            writer.Write((int)target);
        }

        void ICompactBufferSerializer<float>.Copy(in float src, ref float dst)
        {
            dst = src;
        }
    }
}
