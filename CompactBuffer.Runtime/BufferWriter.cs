
using System.IO;
using System.Text;

namespace CompactBuffer
{
    public class BufferWriter : BinaryWriter
    {
        public BufferWriter(Stream input) : base(input)
        {

        }
        public BufferWriter(Stream input, Encoding encoding) : base(input, encoding)
        {

        }

        public BufferWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public void WriteVariantInt32(int value)
        {
            uint uValue = (uint)value;

            // Write out an int 7 bits at a time. The high bit of the byte,
            // when on, tells reader to continue reading more bytes.
            //
            // Using the constants 0x7F and ~0x7F below offers smaller
            // codegen than using the constant 0x80.

            while (uValue > 0x7Fu)
            {
                Write((byte)(uValue | ~0x7Fu));
                uValue >>= 7;
            }

            Write((byte)uValue);
        }

        public void WriteVariantInt64(long value)
        {
            ulong uValue = (ulong)value;

            // Write out an int 7 bits at a time. The high bit of the byte,
            // when on, tells reader to continue reading more bytes.
            //
            // Using the constants 0x7F and ~0x7F below offers smaller
            // codegen than using the constant 0x80.

            while (uValue > 0x7Fu)
            {
                Write((byte)((uint)uValue | ~0x7Fu));
                uValue >>= 7;
            }

            Write((byte)uValue);
        }

        public void WriteFloat16(float floatValue, int integerMax)
        {
            Write((short)(floatValue / integerMax * short.MaxValue));
        }
    }
}
