// Generate by CompactBuffer.CodeGen

namespace CompactBufferAutoGen
{
    [CompactBuffer.CompactBuffer(typeof(Test.AAA))]
    public class Test_AAA_Serializer : CompactBuffer.ICompactBufferSerializer<Test.AAA>
    {
        public static void Read(System.IO.BinaryReader reader, ref Test.AAA target)
        {
            var length = reader.Read7BitEncodedInt();
            if (length == 0) { target = null; return; }
            if (length != 24) { throw new System.Exception("aaaa"); }
            if (target == null) { target = new Test.AAA(); }
            target._sbyte = reader.ReadSByte();
            target._short = reader.ReadInt16();
            target._int = reader.ReadInt32();
            target._long = reader.ReadInt64();
            target._byte = reader.ReadByte();
            target._ushort = reader.ReadUInt16();
            target._uint = reader.ReadUInt32();
            target._ulong = reader.ReadUInt64();
            target._bool = reader.ReadBoolean();
            target._string = reader.ReadString();
            target.i = reader.ReadInt32();
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Read(reader, ref target.vvv);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Read(reader, ref target.vvv0);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Read(reader, ref target.vvv1);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Read(reader, ref target.vvv10);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Read(reader, ref target.list);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Read(reader, ref target.list0);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Read(reader, ref target.list1);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Read(reader, ref target.list10);
            target.variantInt = reader.Read7BitEncodedInt();
            target.variantLong = reader.Read7BitEncodedInt64();
            target.variantUInt = reader.ReadUInt32();
            target.floatTwoByte = CompactBuffer.CompactBufferUtils.ReadFloatTwoByte(reader.ReadInt16(), 10);
        }

        public static void Write(System.IO.BinaryWriter writer, ref Test.AAA target)
        {
            if (target == null)
            {
                writer.Write7BitEncodedInt(0);
                return;
            }
            writer.Write7BitEncodedInt(24);
            writer.Write(target._sbyte);
            writer.Write(target._short);
            writer.Write(target._int);
            writer.Write(target._long);
            writer.Write(target._byte);
            writer.Write(target._ushort);
            writer.Write(target._uint);
            writer.Write(target._ulong);
            writer.Write(target._bool);
            writer.Write(target._string);
            writer.Write(target.i);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Write(writer, ref target.vvv);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Write(writer, ref target.vvv0);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Write(writer, ref target.vvv1);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Write(writer, ref target.vvv10);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Write(writer, ref target.list);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Write(writer, ref target.list0);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Write(writer, ref target.list1);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Write(writer, ref target.list10);
            writer.Write7BitEncodedInt(target.variantInt);
            writer.Write7BitEncodedInt64(target.variantLong);
            writer.Write(target.variantUInt);
            writer.Write(CompactBuffer.CompactBufferUtils.WriteFloatTwoByte(target.floatTwoByte, 10));
        }

        public static void Copy(ref Test.AAA src, ref Test.AAA dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Test.AAA();
            dst._sbyte = src._sbyte;
            dst._short = src._short;
            dst._int = src._int;
            dst._long = src._long;
            dst._byte = src._byte;
            dst._ushort = src._ushort;
            dst._uint = src._uint;
            dst._ulong = src._ulong;
            dst._bool = src._bool;
            dst._string = src._string;
            dst.i = src.i;
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Copy(ref src.vvv, ref dst.vvv);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Copy(ref src.vvv0, ref dst.vvv0);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Copy(ref src.vvv1, ref dst.vvv1);
            CompactBuffer.CompactBuffer.GetArraySerializer<int>().Copy(ref src.vvv10, ref dst.vvv10);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Copy(ref src.list, ref dst.list);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Copy(ref src.list0, ref dst.list0);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Copy(ref src.list1, ref dst.list1);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Copy(ref src.list10, ref dst.list10);
            dst.variantInt = src.variantInt;
            dst.variantLong = src.variantLong;
            dst.variantUInt = src.variantUInt;
            dst.floatTwoByte = src.floatTwoByte;
        }

        void CompactBuffer.ICompactBufferSerializer<Test.AAA>.Read(System.IO.BinaryReader reader, ref Test.AAA target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.AAA>.Write(System.IO.BinaryWriter writer, ref Test.AAA target)
        {
            Write(writer, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.AAA>.Copy(ref Test.AAA src, ref Test.AAA dst)
        {
            Copy(ref src, ref dst);
        }
    }

    [CompactBuffer.CompactBuffer(typeof(Test.BBB))]
    public class Test_BBB_Serializer : CompactBuffer.ICompactBufferSerializer<Test.BBB>
    {
        public static void Read(System.IO.BinaryReader reader, ref Test.BBB target)
        {
            var length = reader.Read7BitEncodedInt();
            if (length == 0) { target = null; return; }
            if (length != 2) { throw new System.Exception("aaaa"); }
            if (target == null) { target = new Test.BBB(); }
            target.i = reader.ReadInt32();
        }

        public static void Write(System.IO.BinaryWriter writer, ref Test.BBB target)
        {
            if (target == null)
            {
                writer.Write7BitEncodedInt(0);
                return;
            }
            writer.Write7BitEncodedInt(2);
            writer.Write(target.i);
        }

        public static void Copy(ref Test.BBB src, ref Test.BBB dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Test.BBB();
            dst.i = src.i;
        }

        void CompactBuffer.ICompactBufferSerializer<Test.BBB>.Read(System.IO.BinaryReader reader, ref Test.BBB target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.BBB>.Write(System.IO.BinaryWriter writer, ref Test.BBB target)
        {
            Write(writer, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.BBB>.Copy(ref Test.BBB src, ref Test.BBB dst)
        {
            Copy(ref src, ref dst);
        }
    }

    [CompactBuffer.CompactBuffer(typeof(Test.CCC))]
    public class Test_CCC_Serializer : CompactBuffer.ICompactBufferSerializer<Test.CCC>
    {
        public static void Read(System.IO.BinaryReader reader, ref Test.CCC target)
        {
            target.i = reader.ReadInt32();
            Test.CustomFloatSerializer.Read(reader, ref target.customFloat);
        }

        public static void Write(System.IO.BinaryWriter writer, ref Test.CCC target)
        {
            writer.Write7BitEncodedInt(3);
            writer.Write(target.i);
            Test.CustomFloatSerializer.Write(writer, ref target.customFloat);
        }

        public static void Copy(ref Test.CCC src, ref Test.CCC dst)
        {
            dst.i = src.i;
            Test.CustomFloatSerializer.Copy(ref src.customFloat, ref dst.customFloat);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.CCC>.Read(System.IO.BinaryReader reader, ref Test.CCC target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.CCC>.Write(System.IO.BinaryWriter writer, ref Test.CCC target)
        {
            Write(writer, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.CCC>.Copy(ref Test.CCC src, ref Test.CCC dst)
        {
            Copy(ref src, ref dst);
        }
    }

    [CompactBuffer.CompactBuffer(typeof(Test.PA))]
    public class Test_PA_Serializer : CompactBuffer.ICompactBufferSerializer<Test.PA>
    {
        public static void Read(System.IO.BinaryReader reader, ref Test.PA target)
        {
            var length = reader.Read7BitEncodedInt();
            if (length == 0) { target = null; return; }
            if (length != 5) { throw new System.Exception("aaaa"); }
            if (target == null) { target = new Test.PA(); }
            target.kkk = reader.ReadInt32();
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Read(reader, ref target.list);
            CompactBuffer.CompactBuffer.GetHashSetSerializer<int>().Read(reader, ref target.hashset);
            CompactBuffer.CompactBuffer.GetDictionarySerializer<string, int>().Read(reader, ref target.dict);
        }

        public static void Write(System.IO.BinaryWriter writer, ref Test.PA target)
        {
            if (target == null)
            {
                writer.Write7BitEncodedInt(0);
                return;
            }
            writer.Write7BitEncodedInt(5);
            writer.Write(target.kkk);
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Write(writer, ref target.list);
            CompactBuffer.CompactBuffer.GetHashSetSerializer<int>().Write(writer, ref target.hashset);
            CompactBuffer.CompactBuffer.GetDictionarySerializer<string, int>().Write(writer, ref target.dict);
        }

        public static void Copy(ref Test.PA src, ref Test.PA dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Test.PA();
            dst.kkk = src.kkk;
            CompactBuffer.CompactBuffer.GetListSerializer<int>().Copy(ref src.list, ref dst.list);
            CompactBuffer.CompactBuffer.GetHashSetSerializer<int>().Copy(ref src.hashset, ref dst.hashset);
            CompactBuffer.CompactBuffer.GetDictionarySerializer<string, int>().Copy(ref src.dict, ref dst.dict);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.PA>.Read(System.IO.BinaryReader reader, ref Test.PA target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.PA>.Write(System.IO.BinaryWriter writer, ref Test.PA target)
        {
            Write(writer, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Test.PA>.Copy(ref Test.PA src, ref Test.PA dst)
        {
            Copy(ref src, ref dst);
        }
    }
}

