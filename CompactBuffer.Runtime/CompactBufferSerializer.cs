
namespace CompactBuffer
{
    public interface ICompactBufferSerializer
    {
    }

    public interface ICompactBufferSerializer<T> : ICompactBufferSerializer
    {
        void Read(BufferReader reader, ref T target);
        void Write(BufferWriter writer, in T target);
        void Copy(in T src, ref T dst);
    }
}
