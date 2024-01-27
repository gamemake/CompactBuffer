// Generate by CompactBuffer.Generator

namespace CompactBufferAutoGen
{
    [CompactBuffer.CompactBuffer(typeof(Test.AAA))]
    public class Test_AAA_Serializer : CompactBuffer.ICompactBufferSerializer<Test.AAA>
    {
        public static void Read(System.IO.BinaryReader reader, ref Test.AAA target)
        {
            var length = CompactBuffer.CompactBufferUtils.ReadLength(reader);
            if (length == 0) { target = null; return; }
            if (length != 20) { throw new System.Exception("aaaa"); }
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
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Read(reader, ref target.vvv);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Read(reader, ref target.vvv0);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Read(reader, ref target.vvv1);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Read(reader, ref target.vvv10);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Read(reader, ref target.list);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Read(reader, ref target.list0);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Read(reader, ref target.list1);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Read(reader, ref target.list10);
        }

        public static void Write(System.IO.BinaryWriter writer, ref Test.AAA target)
        {
            if (target == null)
            {
                CompactBuffer.CompactBufferUtils.WriteLength(writer, 0);
                return;
            }
            CompactBuffer.CompactBufferUtils.WriteLength(writer, 20);
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
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Write(writer, ref target.vvv);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Write(writer, ref target.vvv0);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Write(writer, ref target.vvv1);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Write(writer, ref target.vvv10);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Write(writer, ref target.list);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Write(writer, ref target.list0);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Write(writer, ref target.list1);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Write(writer, ref target.list10);
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
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Copy(ref src.vvv, ref dst.vvv);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Copy(ref src.vvv0, ref dst.vvv0);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Copy(ref src.vvv1, ref dst.vvv1);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Copy(ref src.vvv10, ref dst.vvv10);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Copy(ref src.list, ref dst.list);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Copy(ref src.list0, ref dst.list0);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Copy(ref src.list1, ref dst.list1);
            CompactBuffer.CompactBuffer.GetListSerializer<System.Int32>().Copy(ref src.list10, ref dst.list10);
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
            var length = CompactBuffer.CompactBufferUtils.ReadLength(reader);
            if (length == 0) { target = null; return; }
            if (length != 2) { throw new System.Exception("aaaa"); }
            if (target == null) { target = new Test.BBB(); }
            target.i = reader.ReadInt32();
        }

        public static void Write(System.IO.BinaryWriter writer, ref Test.BBB target)
        {
            if (target == null)
            {
                CompactBuffer.CompactBufferUtils.WriteLength(writer, 0);
                return;
            }
            CompactBuffer.CompactBufferUtils.WriteLength(writer, 2);
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
            CompactBuffer.CompactBufferUtils.WriteLength(writer, 3);
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
}

