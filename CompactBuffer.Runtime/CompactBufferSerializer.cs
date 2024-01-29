
using System.IO;

namespace CompactBuffer
{
    public interface ICompactBufferSerializer
    {
    }

    public interface ICompactBufferSerializer<T> : ICompactBufferSerializer
    {
        void Read(BufferReader reader, ref T target);
        void Write(BufferWriter writer, ref T target);
        void Copy(ref T src, ref T dst);
    }
}
