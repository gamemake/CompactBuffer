
using System.IO;
using CompactBuffer;

namespace Test
{
    [CompactBufferGenCode]
    public class AAA
    {
        public int i = 0;
        public int[] vvv = null;
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
        public void Read(BinaryReader reader, ref float target)
        {
            target = (float)reader.ReadInt32();
        }

        public void Write(BinaryWriter writer, ref float target)
        {
            writer.Write((int)target);
        }

        public void Copy(ref float src, ref float dst)
        {
            dst = src;
        }
    }
}
