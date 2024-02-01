// Generate by CompactBuffer.CodeGen

namespace CompactBufferAutoGen
{
    [CompactBuffer.CompactBuffer(typeof(Tests.AAA), true)]
    public class Tests_AAA_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.AAA>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.AAA target)
        {
            var length = reader.ReadVariantInt32();
            if (length == 0) { target = null; return; }
            if (length != 2) { throw new CompactBuffer.CompactBufferExeption("data version not match"); }
            if (target == null) { target = new Tests.AAA(); }
            target.enum0 = (Tests.EnumTypes)reader.ReadVariantInt32();
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.AAA target)
        {
            if (target == null)
            {
                writer.WriteVariantInt32(0);
                return;
            }
            writer.WriteVariantInt32(2);
            writer.WriteVariantInt32((int)target.enum0);
        }

        public static void Copy(in Tests.AAA src, ref Tests.AAA dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Tests.AAA();
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

    [CompactBuffer.CompactBuffer(typeof(Tests.BBB), true)]
    public class Tests_BBB_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.BBB>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.BBB target)
        {
            var length = reader.ReadVariantInt32();
            if (length == 0) { target = null; return; }
            if (length != 1) { throw new CompactBuffer.CompactBufferExeption("data version not match"); }
            if (target == null) { target = new Tests.BBB(); }
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.BBB target)
        {
            if (target == null)
            {
                writer.WriteVariantInt32(0);
                return;
            }
            writer.WriteVariantInt32(1);
        }

        public static void Copy(in Tests.BBB src, ref Tests.BBB dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Tests.BBB();
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

    [CompactBuffer.CompactBuffer(typeof(Tests.CCC), true)]
    public class Tests_CCC_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.CCC>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.CCC target)
        {
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.CCC target)
        {
            writer.WriteVariantInt32(1);
        }

        public static void Copy(in Tests.CCC src, ref Tests.CCC dst)
        {
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

    [CompactBuffer.CompactBuffer(typeof(Tests.TypeClass), true)]
    public class Tests_TypeClass_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.TypeClass>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.TypeClass target)
        {
            var length = reader.ReadVariantInt32();
            if (length == 0) { target = null; return; }
            if (length != 1) { throw new CompactBuffer.CompactBufferExeption("data version not match"); }
            if (target == null) { target = new Tests.TypeClass(); }
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.TypeClass target)
        {
            if (target == null)
            {
                writer.WriteVariantInt32(0);
                return;
            }
            writer.WriteVariantInt32(1);
        }

        public static void Copy(in Tests.TypeClass src, ref Tests.TypeClass dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Tests.TypeClass();
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

    [CompactBuffer.CompactBuffer(typeof(Tests.TypeStruct), true)]
    public class Tests_TypeStruct_Serializer : CompactBuffer.ICompactBufferSerializer<Tests.TypeStruct>
    {
        public static void Read(CompactBuffer.BufferReader reader, ref Tests.TypeStruct target)
        {
        }

        public static void Write(CompactBuffer.BufferWriter writer, in Tests.TypeStruct target)
        {
            writer.WriteVariantInt32(1);
        }

        public static void Copy(in Tests.TypeStruct src, ref Tests.TypeStruct dst)
        {
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

