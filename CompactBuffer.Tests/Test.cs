
using System.Collections.Generic;
using System.IO;
using CompactBuffer;

namespace Test
{
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
        public static void Read(BinaryReader reader, ref float target)
        {
            target = (float)reader.ReadInt32();
        }

        public static void Write(BinaryWriter writer, ref float target)
        {
            writer.Write((int)target);
        }

        public static void Copy(ref float src, ref float dst)
        {
            dst = src;
        }

        void ICompactBufferSerializer<float>.Read(BinaryReader reader, ref float target)
        {
            target = (float)reader.ReadInt32();
        }

        void ICompactBufferSerializer<float>.Write(BinaryWriter writer, ref float target)
        {
            writer.Write((int)target);
        }

        void ICompactBufferSerializer<float>.Copy(ref float src, ref float dst)
        {
            dst = src;
        }
    }
}
