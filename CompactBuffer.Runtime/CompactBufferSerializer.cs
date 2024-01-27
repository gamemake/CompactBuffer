
using System.IO;

namespace CompactBuffer
{
    public interface ICompactBufferSerializer
    {
    }

    public interface ICompactBufferSerializer<T> : ICompactBufferSerializer
    {
        void Read(BinaryReader reader, ref T target);
        void Write(BinaryWriter writer, ref T target);
        void Copy(ref T src, ref T dst);
    }
}
