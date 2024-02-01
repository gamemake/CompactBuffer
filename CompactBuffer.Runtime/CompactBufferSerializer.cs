
namespace CompactBuffer
{
    public interface ICompactBufferSerializer
    {
    }

    public interface ICompactBufferSerializer<T> : ICompactBufferSerializer
    {
        void Read(BufferReader reader, ref T target);
        void Write(BufferWriter writer, ref readonly T target);
        void Copy(ref readonly T src, ref T dst);
    }
}
