// Generate by CompactBuffer.CodeGen

namespace CompactBufferAutoGen
{
    [CompactBuffer.AutoGen(typeof(Tests.Vector3))]
    public class Tests_Vector3_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.Vector3>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.Vector3 target)
        {
            target.X = reader.ReadSingle();
            target.Y = reader.ReadSingle();
            target.Z = reader.ReadSingle();
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.Vector3 target)
        {
            writer.Write(target.X);
            writer.Write(target.Y);
            writer.Write(target.Z);
        }

        public static void Copy(in Tests.Vector3 src, ref Tests.Vector3 dst)
        {
            dst.X = src.X;
            dst.Y = src.Y;
            dst.Z = src.Z;
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.Vector3>.Read(CompactBuffer.BufferReader reader, ref Tests.Vector3 target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.Vector3>.Write(CompactBuffer.BufferWriter writer, in Tests.Vector3 target)
        {
            Write(writer, in target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.Vector3>.Copy(in Tests.Vector3 src, ref Tests.Vector3 dst)
        {
            Copy(in src, ref dst);
        }
    }

    [CompactBuffer.AutoGen(typeof(Tests.AAA))]
    public class Tests_AAA_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.AAA>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.AAA target)
        {
            var length = reader.Read7BitEncodedInt32();
            if (length == 0) { target = null; return; }
            if (length != 26) { throw new CompactBuffer.CompactBufferException("data version not match"); }
            if (target == null) { target = new Tests.AAA(); }
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
            CompactBuffer.Internal.ArraySerializer<int>.Read(reader, ref target.vvv);
            CompactBuffer.Internal.ArraySerializer<int>.Read(reader, ref target.vvv0);
            CompactBuffer.Internal.ArraySerializer<int>.Read(reader, ref target.vvv1);
            CompactBuffer.Internal.ArraySerializer<int>.Read(reader, ref target.vvv10);
            CompactBuffer.Internal.ListSerializer<int>.Read(reader, ref target.list);
            CompactBuffer.Internal.ListSerializer<int>.Read(reader, ref target.list0);
            CompactBuffer.Internal.ListSerializer<int>.Read(reader, ref target.list1);
            CompactBuffer.Internal.ListSerializer<int>.Read(reader, ref target.list10);
            target._7BitEncodedInt = reader.Read7BitEncodedInt32();
            target._7BitEncodedLong = reader.Read7BitEncodedInt64();
            target._7BitEncodedUInt = reader.ReadUInt32();
            target.floatTwoByte = reader.ReadFloat16(10);
            target.guid = reader.ReadGuid();
            target.enum0 = (Tests.EnumTypes)reader.Read7BitEncodedInt32();
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.AAA target)
        {
            if (target == null)
            {
                writer.Write7BitEncodedInt32(0);
                return;
            }
            writer.Write7BitEncodedInt32(26);
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
            CompactBuffer.Internal.ArraySerializer<int>.Write(writer, in target.vvv);
            CompactBuffer.Internal.ArraySerializer<int>.Write(writer, in target.vvv0);
            CompactBuffer.Internal.ArraySerializer<int>.Write(writer, in target.vvv1);
            CompactBuffer.Internal.ArraySerializer<int>.Write(writer, in target.vvv10);
            CompactBuffer.Internal.ListSerializer<int>.Write(writer, in target.list);
            CompactBuffer.Internal.ListSerializer<int>.Write(writer, in target.list0);
            CompactBuffer.Internal.ListSerializer<int>.Write(writer, in target.list1);
            CompactBuffer.Internal.ListSerializer<int>.Write(writer, in target.list10);
            writer.Write7BitEncodedInt32(target._7BitEncodedInt);
            writer.Write7BitEncodedInt64(target._7BitEncodedLong);
            writer.Write(target._7BitEncodedUInt);
            writer.WriteFloat16(target.floatTwoByte, 10);
            writer.Write(target.guid);
            writer.Write7BitEncodedInt32((int)target.enum0);
        }

        public static void Copy(in Tests.AAA src, ref Tests.AAA dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Tests.AAA();
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
            CompactBuffer.Internal.ArraySerializer<int>.Copy(in src.vvv, ref dst.vvv);
            CompactBuffer.Internal.ArraySerializer<int>.Copy(in src.vvv0, ref dst.vvv0);
            CompactBuffer.Internal.ArraySerializer<int>.Copy(in src.vvv1, ref dst.vvv1);
            CompactBuffer.Internal.ArraySerializer<int>.Copy(in src.vvv10, ref dst.vvv10);
            CompactBuffer.Internal.ListSerializer<int>.Copy(in src.list, ref dst.list);
            CompactBuffer.Internal.ListSerializer<int>.Copy(in src.list0, ref dst.list0);
            CompactBuffer.Internal.ListSerializer<int>.Copy(in src.list1, ref dst.list1);
            CompactBuffer.Internal.ListSerializer<int>.Copy(in src.list10, ref dst.list10);
            dst._7BitEncodedInt = src._7BitEncodedInt;
            dst._7BitEncodedLong = src._7BitEncodedLong;
            dst._7BitEncodedUInt = src._7BitEncodedUInt;
            dst.floatTwoByte = src.floatTwoByte;
            dst.guid = src.guid;
            dst.enum0 = src.enum0;
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.AAA>.Read(CompactBuffer.BufferReader reader, ref Tests.AAA target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.AAA>.Write(CompactBuffer.BufferWriter writer, in Tests.AAA target)
        {
            Write(writer, in target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.AAA>.Copy(in Tests.AAA src, ref Tests.AAA dst)
        {
            Copy(in src, ref dst);
        }
    }

    [CompactBuffer.AutoGen(typeof(Tests.BBB))]
    public class Tests_BBB_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.BBB>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.BBB target)
        {
            var length = reader.Read7BitEncodedInt32();
            if (length == 0) { target = null; return; }
            if (length != 2) { throw new CompactBuffer.CompactBufferException("data version not match"); }
            if (target == null) { target = new Tests.BBB(); }
            target.i = reader.ReadInt32();
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.BBB target)
        {
            if (target == null)
            {
                writer.Write7BitEncodedInt32(0);
                return;
            }
            writer.Write7BitEncodedInt32(2);
            writer.Write(target.i);
        }

        public static void Copy(in Tests.BBB src, ref Tests.BBB dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Tests.BBB();
            dst.i = src.i;
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.BBB>.Read(CompactBuffer.BufferReader reader, ref Tests.BBB target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.BBB>.Write(CompactBuffer.BufferWriter writer, in Tests.BBB target)
        {
            Write(writer, in target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.BBB>.Copy(in Tests.BBB src, ref Tests.BBB dst)
        {
            Copy(in src, ref dst);
        }
    }

    [CompactBuffer.AutoGen(typeof(Tests.CCC))]
    public class Tests_CCC_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.CCC>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.CCC target)
        {
            target.i = reader.ReadInt32();
            Tests.CustomFloatSerializer.Read(reader, ref target.customFloat);
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.CCC target)
        {
            writer.Write(target.i);
            Tests.CustomFloatSerializer.Write(writer, in target.customFloat);
        }

        public static void Copy(in Tests.CCC src, ref Tests.CCC dst)
        {
            dst.i = src.i;
            dst.customFloat = src.customFloat;
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.CCC>.Read(CompactBuffer.BufferReader reader, ref Tests.CCC target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.CCC>.Write(CompactBuffer.BufferWriter writer, in Tests.CCC target)
        {
            Write(writer, in target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.CCC>.Copy(in Tests.CCC src, ref Tests.CCC dst)
        {
            Copy(in src, ref dst);
        }
    }

    [CompactBuffer.AutoGen(typeof(Tests.TypeClass))]
    public class Tests_TypeClass_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.TypeClass>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.TypeClass target)
        {
            var length = reader.Read7BitEncodedInt32();
            if (length == 0) { target = null; return; }
            if (length != 5) { throw new CompactBuffer.CompactBufferException("data version not match"); }
            if (target == null) { target = new Tests.TypeClass(); }
            target.kkk = reader.ReadInt32();
            CompactBuffer.Internal.ListSerializer<int>.Read(reader, ref target.list);
            CompactBuffer.Internal.HashSetSerializer<int>.Read(reader, ref target.hashset);
            CompactBuffer.Internal.DictionarySerializer<string,int>.Read(reader, ref target.dict);
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.TypeClass target)
        {
            if (target == null)
            {
                writer.Write7BitEncodedInt32(0);
                return;
            }
            writer.Write7BitEncodedInt32(5);
            writer.Write(target.kkk);
            CompactBuffer.Internal.ListSerializer<int>.Write(writer, in target.list);
            CompactBuffer.Internal.HashSetSerializer<int>.Write(writer, in target.hashset);
            CompactBuffer.Internal.DictionarySerializer<string,int>.Write(writer, in target.dict);
        }

        public static void Copy(in Tests.TypeClass src, ref Tests.TypeClass dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Tests.TypeClass();
            dst.kkk = src.kkk;
            CompactBuffer.Internal.ListSerializer<int>.Copy(in src.list, ref dst.list);
            CompactBuffer.Internal.HashSetSerializer<int>.Copy(in src.hashset, ref dst.hashset);
            CompactBuffer.Internal.DictionarySerializer<string,int>.Copy(in src.dict, ref dst.dict);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.TypeClass>.Read(CompactBuffer.BufferReader reader, ref Tests.TypeClass target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.TypeClass>.Write(CompactBuffer.BufferWriter writer, in Tests.TypeClass target)
        {
            Write(writer, in target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.TypeClass>.Copy(in Tests.TypeClass src, ref Tests.TypeClass dst)
        {
            Copy(in src, ref dst);
        }
    }

    [CompactBuffer.AutoGen(typeof(Tests.TypeStruct))]
    public class Tests_TypeStruct_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.TypeStruct>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.TypeStruct target)
        {
            target.i = reader.ReadInt32();
            CompactBuffer.Internal.ArraySerializer<int>.Read(reader, ref target.array);
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.TypeStruct target)
        {
            writer.Write(target.i);
            CompactBuffer.Internal.ArraySerializer<int>.Write(writer, in target.array);
        }

        public static void Copy(in Tests.TypeStruct src, ref Tests.TypeStruct dst)
        {
            dst.i = src.i;
            CompactBuffer.Internal.ArraySerializer<int>.Copy(in src.array, ref dst.array);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.TypeStruct>.Read(CompactBuffer.BufferReader reader, ref Tests.TypeStruct target)
        {
            Read(reader, ref target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.TypeStruct>.Write(CompactBuffer.BufferWriter writer, in Tests.TypeStruct target)
        {
            Write(writer, in target);
        }

        void CompactBuffer.ICompactBufferSerializer<Tests.TypeStruct>.Copy(in Tests.TypeStruct src, ref Tests.TypeStruct dst)
        {
            Copy(in src, ref dst);
        }
    }
}

