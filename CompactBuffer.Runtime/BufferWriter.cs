
using System;
using System.Buffers.Binary;
using System.Text;

namespace CompactBuffer
{
    public class BufferWriter
    {
        private byte[] _buffer;
        private int _start;
        private int _position;
        private int _length;
        private int _capacity;
        private Encoding _encoding;

        public BufferWriter(byte[] buffer, Encoding encoding = null) : this(buffer, 0, buffer.Length, encoding)
        {
        }

        public BufferWriter(byte[] buffer, int index, int count, Encoding encoding = null)
        {
            if (buffer is null)
                throw new ArgumentNullException("buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, $"index '{index}' must be a non-negative.");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, $"index '{count}' must be a non-negative.");
            if (buffer.Length - index < count)
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");

            if (encoding == null)
                _encoding = Encoding.UTF8;
            else
                _encoding = encoding;

            _buffer = buffer;
            _start = _position = _length = index;
            _capacity = index + count;
        }

        private Span<byte> InternalWrite(int size)
        {
            if (_position + size > _capacity)
                throw new OutOfMemoryException("Out of memory.");

            var origin = _position;
            _position += size;
            if (_position > _length)
                _length = _position;

            return new Span<byte>(_buffer, origin, size);
        }

        public int Position
        {
            get
            {
                return _position - _start;
            }
            set
            {
                if (value < 0 || value > _length - _start)
                    throw new ArgumentOutOfRangeException("value", value, $"value '{value}' must be between 0 and {_length - _start}.");

                _position = value;
            }
        }
        public int Length => _length - _start;

        public Span<byte> GetWriteBytes()
        {
            return new Span<byte>(_buffer, _start, _length - _start);
        }

        public void Write(ReadOnlySpan<byte> bytes)
        {
            var destination = InternalWrite(bytes.Length);
            bytes.CopyTo(destination);
        }

        public void Write(sbyte value) => InternalWrite(1)[0] = (byte)value;
        public void Write(byte value) => InternalWrite(1)[0] = value;

        public void Write(short value) => BinaryPrimitives.WriteInt16LittleEndian(InternalWrite(2), value);
        public void Write(int value) => BinaryPrimitives.WriteInt32LittleEndian(InternalWrite(4), value);
        public void Write(long value) => BinaryPrimitives.WriteInt64LittleEndian(InternalWrite(8), value);

        public void Write(ushort value) => BinaryPrimitives.WriteUInt16LittleEndian(InternalWrite(2), value);
        public void Write(uint value) => BinaryPrimitives.WriteUInt32LittleEndian(InternalWrite(4), value);
        public void Write(ulong value) => BinaryPrimitives.WriteUInt64LittleEndian(InternalWrite(8), value);

        public void Write(float value) => BitConverter.TryWriteBytes(InternalWrite(4), value);
        public void Write(double value) => BitConverter.TryWriteBytes(InternalWrite(8), value);

        public void Write(bool value) => InternalWrite(1)[0] = value ? (byte)1 : (byte)0;

        public void Write(string value)
        {
            var stringLength = _encoding.GetByteCount(value);
            WriteVariantInt32(stringLength);

            var span = InternalWrite(stringLength);
            var encoder = _encoding.GetEncoder();
            encoder.Convert(value, span, false, out var charsUsed, out var bytesUsed, out var completed);

            _position += stringLength;
            if (_position > _length)
                _length = _position;
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
            if (floatValue > integerMax || floatValue < -integerMax)
            {
                throw new ArgumentOutOfRangeException("floatValue", floatValue, $"floatValue ({floatValue}) must be between {-integerMax} and {integerMax}");
            }
            Write((short)(floatValue / integerMax * short.MaxValue));
        }

        public void Write(Guid value)
        {
            value.TryWriteBytes(InternalWrite(16));
        }
    }
}
