
using System;
using System.IO;
using System.Buffers.Binary;
using System.Text;

namespace CompactBuffer
{
    public class BufferReader
    {
        private byte[] _buffer;
        private int _start;
        private int _position;
        private int _length;
        private Encoding _encoding;

        public BufferReader(byte[] buffer, Encoding encoding = null) : this(buffer, 0, buffer.Length, encoding)
        {
        }

        public BufferReader(byte[] buffer, int index, int count, Encoding encoding = null)
        {
            if (buffer is null)
                throw new ArgumentNullException("buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, $"index '{index}' must be a non-negative and non-zero value.");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, $"index '{count}' must be a non-negative and non-zero value.");
            if (buffer.Length - index < count)
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");

            if (encoding == null)
                _encoding = Encoding.UTF8;
            else
                _encoding = encoding;

            _buffer = buffer;
            _start = _position = index;
            _length = index + count;
        }

        public int Position
        {
            get
            {
                return _position - _start; ;
            }
            set
            {
                if (value < 0 || value > _length - _start)
                    throw new ArgumentOutOfRangeException("value", value, $"value '{value}' must be between 0 and {_length - _start}.");

                _position = value;
            }
        }
        public int Lenght => _length - _position;

        public ReadOnlySpan<byte> ReadBytes(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size", size, $"size '{size}' must be a non-negative and non-zero value.");
            if (_position + size > _length)
                throw new EndOfStreamException();

            _position += size;
            return new ReadOnlySpan<byte>(_buffer, _position, size);
        }

        public sbyte ReadSByte()
        {
            if (_position >= _length)
                throw new EndOfStreamException();

            return (sbyte)_buffer[_position++];
        }

        public byte ReadByte()
        {
            if (_position >= _length)
                throw new EndOfStreamException();

            return _buffer[_position++];
        }

        public short ReadInt16() => BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
        public int ReadInt32() => BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(4));
        public long ReadInt64() => BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(8));

        public ushort ReadUInt16() => BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(2));
        public uint ReadUInt32() => BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(4));
        public ulong ReadUInt64() => BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(8));

        public float ReadSingle() => BitConverter.ToSingle(ReadBytes(4));
        public double ReadDouble() => BitConverter.ToDouble(ReadBytes(8));

        public bool ReadBoolean() => ReadByte() != 0;

        public string ReadString()
        {
            var stringLength = ReadVariantInt32();
            if (stringLength < 0)
                throw new IOException($"BinaryReader encountered an invalid string length of {stringLength} characters.");

            return _encoding.GetString(ReadBytes(stringLength));
        }

        private static readonly string Format_Bad7BitInt = "Too many bytes in what should have been a 7-bit encoded integer.";

        public int ReadVariantInt32()
        {
            // Unlike writing, we can't delegate to the 64-bit read on
            // 64-bit platforms. The reason for this is that we want to
            // stop consuming bytes if we encounter an integer overflow.

            uint result = 0;
            byte byteReadJustNow;

            // Read the integer 7 bits at a time. The high bit
            // of the byte when on means to continue reading more bytes.
            //
            // There are two failure cases: we've read more than 5 bytes,
            // or the fifth byte is about to cause integer overflow.
            // This means that we can read the first 4 bytes without
            // worrying about integer overflow.

            const int MaxBytesWithoutOverflow = 4;
            for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
            {
                // ReadByte handles end of stream cases for us.
                byteReadJustNow = ReadByte();
                result |= (byteReadJustNow & 0x7Fu) << shift;

                if (byteReadJustNow <= 0x7Fu)
                {
                    return (int)result; // early exit
                }
            }

            // Read the 5th byte. Since we already read 28 bits,
            // the value of this byte must fit within 4 bits (32 - 28),
            // and it must not have the high bit set.

            byteReadJustNow = ReadByte();
            if (byteReadJustNow > 0b_1111u)
            {
                throw new FormatException(Format_Bad7BitInt);
            }

            result |= (uint)byteReadJustNow << (MaxBytesWithoutOverflow * 7);
            return (int)result;
        }

        public long ReadVariantInt64()
        {
            ulong result = 0;
            byte byteReadJustNow;

            // Read the integer 7 bits at a time. The high bit
            // of the byte when on means to continue reading more bytes.
            //
            // There are two failure cases: we've read more than 10 bytes,
            // or the tenth byte is about to cause integer overflow.
            // This means that we can read the first 9 bytes without
            // worrying about integer overflow.

            const int MaxBytesWithoutOverflow = 9;
            for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
            {
                // ReadByte handles end of stream cases for us.
                byteReadJustNow = ReadByte();
                result |= (byteReadJustNow & 0x7Ful) << shift;

                if (byteReadJustNow <= 0x7Fu)
                {
                    return (long)result; // early exit
                }
            }

            // Read the 10th byte. Since we already read 63 bits,
            // the value of this byte must fit within 1 bit (64 - 63),
            // and it must not have the high bit set.

            byteReadJustNow = ReadByte();
            if (byteReadJustNow > 0b_1u)
            {
                throw new FormatException(Format_Bad7BitInt);
            }

            result |= (ulong)byteReadJustNow << (MaxBytesWithoutOverflow * 7);
            return (long)result;
        }

        public float ReadFloat16(int integerMax)
        {
            var shortValue = ReadInt16();
            return shortValue / (float)short.MaxValue * integerMax;
        }

        public Guid ReadGuid()
        {
            return new Guid(ReadBytes(16));
        }
    }
}
