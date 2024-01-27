// Generate by CompactBuffer.Generator

namespace CompactBufferAutoGen
{
    [CompactBuffer.CompactBuffer(typeof(Test.AAA))]
    public class Test_AAA_Serializer : CompactBuffer.ICompactBufferSerializer<Test.AAA>
    {
        public void Read(System.IO.BinaryReader reader, ref Test.AAA target)
        {
            var length = CompactBuffer.CompactBufferUtils.ReadLength(reader);
            if (length == 0) { target = null; return; }
            if (length != 3) { throw new System.Exception("aaaa"); }
            if (target == null) { target = new Test.AAA(); }
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Read(reader, ref target.i);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Read(reader, ref target.vvv);
        }

        public void Write(System.IO.BinaryWriter writer, ref Test.AAA target)
        {
            if (target == null)
            {
                CompactBuffer.CompactBufferUtils.WriteLength(writer, 0);
                return;
            }
            CompactBuffer.CompactBufferUtils.WriteLength(writer, 3);
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Write(writer, ref target.i);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Write(writer, ref target.vvv);
        }

        public void Copy(ref Test.AAA src, ref Test.AAA dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Test.AAA();
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Copy(ref src.i, ref dst.i);
            CompactBuffer.CompactBuffer.GetArraySerializer<System.Int32>().Copy(ref src.vvv, ref dst.vvv);
        }
    }

    [CompactBuffer.CompactBuffer(typeof(Test.BBB))]
    public class Test_BBB_Serializer : CompactBuffer.ICompactBufferSerializer<Test.BBB>
    {
        public void Read(System.IO.BinaryReader reader, ref Test.BBB target)
        {
            var length = CompactBuffer.CompactBufferUtils.ReadLength(reader);
            if (length == 0) { target = null; return; }
            if (length != 2) { throw new System.Exception("aaaa"); }
            if (target == null) { target = new Test.BBB(); }
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Read(reader, ref target.i);
        }

        public void Write(System.IO.BinaryWriter writer, ref Test.BBB target)
        {
            if (target == null)
            {
                CompactBuffer.CompactBufferUtils.WriteLength(writer, 0);
                return;
            }
            CompactBuffer.CompactBufferUtils.WriteLength(writer, 2);
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Write(writer, ref target.i);
        }

        public void Copy(ref Test.BBB src, ref Test.BBB dst)
        {
            if (src == null) { dst = null; return; }
            if (dst == null) dst = new Test.BBB();
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Copy(ref src.i, ref dst.i);
        }
    }

    [CompactBuffer.CompactBuffer(typeof(Test.CCC))]
    public class Test_CCC_Serializer : CompactBuffer.ICompactBufferSerializer<Test.CCC>
    {
        public void Read(System.IO.BinaryReader reader, ref Test.CCC target)
        {
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Read(reader, ref target.i);
            CompactBuffer.CompactBuffer.GetCustomSerializer<Test.CustomFloatSerializer, System.Single>().Read(reader, ref target.customFloat);
        }

        public void Write(System.IO.BinaryWriter writer, ref Test.CCC target)
        {
            CompactBuffer.CompactBufferUtils.WriteLength(writer, 3);
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Write(writer, ref target.i);
            CompactBuffer.CompactBuffer.GetCustomSerializer<Test.CustomFloatSerializer, System.Single>().Write(writer, ref target.customFloat);
        }

        public void Copy(ref Test.CCC src, ref Test.CCC dst)
        {
            CompactBuffer.CompactBuffer.GetSerializer<System.Int32>().Copy(ref src.i, ref dst.i);
            CompactBuffer.CompactBuffer.GetCustomSerializer<Test.CustomFloatSerializer, System.Single>().Copy(ref src.customFloat, ref dst.customFloat);
        }
    }
}
