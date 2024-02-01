
using System;
using System.Collections.Generic;

namespace CompactBuffer.Internal
{
    public class ArraySerializer<TElement> : ICompactBufferSerializer<TElement[]>
    {
        private static ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();

        public static void Read(BufferReader reader, ref TElement[] target)
        {
            var length = reader.ReadVariantInt32();
            if (length <= 0)
            {
                target = null;
                return;
            }

            length -= 1;
            if (target == null || target.Length != length)
            {
                target = new TElement[length];
            }

            for (var i = 0; i < length; i++)
            {
                m_ElementSerializer.Read(reader, ref target[i]);
            }
        }

        public static void Write(BufferWriter writer, in TElement[] target)
        {
            if (target == null)
            {
                writer.WriteVariantInt32(0);
                return;
            }

            writer.WriteVariantInt32(target.Length + 1);
            for (var i = 0; i < target.Length; i++)
            {
                m_ElementSerializer.Write(writer, in target[i]);
            }
        }

        public static void Copy(in TElement[] src, ref TElement[] dst)
        {
            if (src == null)
            {
                dst = null;
            }
            if (dst == null || dst.Length != src.Length)
            {
                dst = new TElement[src.Length];
            }
            for (var i = 0; i < src.Length; i++)
            {
                m_ElementSerializer.Copy(in src[i], ref dst[i]);
            }
        }

        void ICompactBufferSerializer<TElement[]>.Read(BufferReader reader, ref TElement[] target)
        {
            Read(reader, ref target);
        }

        void ICompactBufferSerializer<TElement[]>.Write(BufferWriter writer, in TElement[] target)
        {
            Write(writer, in target);
        }

        void ICompactBufferSerializer<TElement[]>.Copy(in TElement[] src, ref TElement[] dst)
        {
            Copy(in src, ref dst);
        }
    }

    public class ListSerializer<TElement> : ICompactBufferSerializer<List<TElement>>
    {
        private static ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();
        private static bool m_IsValueType = typeof(TElement).IsValueType;

        public static void Read(BufferReader reader, ref List<TElement> target)
        {
            var length = reader.ReadVariantInt32();
            if (length <= 0)
            {
                target = null;
                return;
            }

            if (target == null)
            {
                target = new List<TElement>();
            }
            if (target.Count != --length)
            {
                target.Resize(length, default(TElement));
            }

            for (var i = 0; i < length; i++)
            {
                var element = target[i];
                m_ElementSerializer.Read(reader, ref element);
                if (m_IsValueType) target[i] = element;
            }
        }

        public static void Write(BufferWriter writer, in List<TElement> target)
        {
            if (target == null)
            {
                writer.WriteVariantInt32(0);
                return;
            }

            writer.WriteVariantInt32(target.Count + 1);
            for (var i = 0; i < target.Count; i++)
            {
                var element = target[i];
                m_ElementSerializer.Write(writer, in element);
                if (m_IsValueType) target[i] = element;
            }
        }

        public static void Copy(in List<TElement> src, ref List<TElement> dst)
        {
            if (src == null)
            {
                dst = null;
            }

            if (dst == null)
            {
                dst = new List<TElement>();
            }
            if (dst.Count != src.Count)
            {
                dst.Resize(src.Count, default(TElement));
            }

            for (var i = 0; i < src.Count; i++)
            {
                var srcElement = src[i];
                var dstElement = dst[i];
                m_ElementSerializer.Copy(in srcElement, ref dstElement);
                if (m_IsValueType) dst[i] = dstElement;
            }
        }

        void ICompactBufferSerializer<List<TElement>>.Read(BufferReader reader, ref List<TElement> target)
        {
            Read(reader, ref target);
        }

        void ICompactBufferSerializer<List<TElement>>.Write(BufferWriter writer, in List<TElement> target)
        {
            Write(writer, in target);
        }

        void ICompactBufferSerializer<List<TElement>>.Copy(in List<TElement> src, ref List<TElement> dst)
        {
            Copy(in src, ref dst);
        }
    }

    public class HashSetSerializer<TElement> : ICompactBufferSerializer<HashSet<TElement>>
    {
        private static ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();

        public static void Read(BufferReader reader, ref HashSet<TElement> target)
        {
            var length = reader.ReadVariantInt32();
            if (length-- <= 0)
            {
                target = null;
                return;
            }

            if (target == null)
            {
                target = new HashSet<TElement>();
            }
            else
            {
                target.Clear();
            }

            for (var i = 0; i < length; i++)
            {
                TElement element = default(TElement);
                m_ElementSerializer.Read(reader, ref element);
                target.Add(element);
            }
        }

        public static void Write(BufferWriter writer, in HashSet<TElement> target)
        {
            if (target == null)
            {
                writer.WriteVariantInt32(0);
                return;
            }

            writer.WriteVariantInt32(target.Count + 1);
            foreach (var element in target)
            {
                var _element = element;
                m_ElementSerializer.Write(writer, in _element);
            }
        }

        public static void Copy(in HashSet<TElement> src, ref HashSet<TElement> dst)
        {
            if (src == null)
            {
                dst = null;
            }

            if (dst == null)
            {
                dst = new HashSet<TElement>();
            }
            else
            {
                dst.Clear();
            }

            foreach (var element in src)
            {
                var srcElement = element;
                var dstElement = default(TElement);
                m_ElementSerializer.Copy(in srcElement, ref dstElement);
                dst.Add(dstElement);
            }
        }

        void ICompactBufferSerializer<HashSet<TElement>>.Read(BufferReader reader, ref HashSet<TElement> target)
        {
            Read(reader, ref target);
        }

        void ICompactBufferSerializer<HashSet<TElement>>.Write(BufferWriter writer, in HashSet<TElement> target)
        {
            Write(writer, in target);
        }

        void ICompactBufferSerializer<HashSet<TElement>>.Copy(in HashSet<TElement> src, ref HashSet<TElement> dst)
        {
            Copy(in src, ref dst);
        }
    }

    public class DictionarySerializer<TKey, TValue> : ICompactBufferSerializer<Dictionary<TKey, TValue>>
    {
        private static ICompactBufferSerializer<TKey> m_KeySerializer = CompactBuffer.GetSerializer<TKey>();
        private static ICompactBufferSerializer<TValue> m_ValueSerializer = CompactBuffer.GetSerializer<TValue>();

        public static void Read(BufferReader reader, ref Dictionary<TKey, TValue> target)
        {
            var length = reader.ReadVariantInt32();
            if (length-- <= 0)
            {
                target = null;
                return;
            }

            if (target == null)
            {
                target = new Dictionary<TKey, TValue>();
            }
            else
            {
                target.Clear();
            }

            for (var i = 0; i < length; i++)
            {
                var _key = default(TKey);
                var _value = default(TValue);
                m_KeySerializer.Read(reader, ref _key);
                m_ValueSerializer.Read(reader, ref _value);
                target.Add(_key, _value);
            }
        }

        public static void Write(BufferWriter writer, in Dictionary<TKey, TValue> target)
        {
            if (target == null)
            {
                writer.WriteVariantInt32(0);
            }

            writer.WriteVariantInt32(target.Count + 1);
            foreach (var item in target)
            {
                var _key = item.Key;
                var _value = item.Value;
                m_KeySerializer.Write(writer, in _key);
                m_ValueSerializer.Write(writer, in _value);
            }
        }

        public static void Copy(in Dictionary<TKey, TValue> src, ref Dictionary<TKey, TValue> dst)
        {
            if (src == null)
            {
                dst = null;
                return;
            }

            if (dst == null)
            {
                dst = new Dictionary<TKey, TValue>();
            }
            else
            {
                dst.Clear();
            }

            foreach (var item in src)
            {
                var srcKey = item.Key;
                var srcValue = item.Value;
                var dstKey = default(TKey);
                var dstValue = default(TValue);
                m_KeySerializer.Copy(in srcKey, ref dstKey);
                m_ValueSerializer.Copy(in srcValue, ref dstValue);
                dst.Add(dstKey, dstValue);
            }
        }

        void ICompactBufferSerializer<Dictionary<TKey, TValue>>.Read(BufferReader reader, ref Dictionary<TKey, TValue> target)
        {
            Read(reader, ref target);
        }

        void ICompactBufferSerializer<Dictionary<TKey, TValue>>.Write(BufferWriter writer, in Dictionary<TKey, TValue> target)
        {
            Write(writer, in target);
        }

        void ICompactBufferSerializer<Dictionary<TKey, TValue>>.Copy(in Dictionary<TKey, TValue> src, ref Dictionary<TKey, TValue> dst)
        {
            Copy(in src, ref dst);
        }
    }

    public class SpanSerializer<TElement> : ICompactBufferSerializer
    {
        private static ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();

        public static void Read(BufferReader reader, ref Span<TElement> target)
        {
            var length = reader.ReadVariantInt32();
            if (length < 0)
            {
                throw new FormatException($"Span length ({length}) must be a non-negative");
            }
            if (length == 0)
            {
                target = Span<TElement>.Empty;
                return;
            }

            target = new TElement[length];
            for (var i = 0; i < length; i++)
            {
                m_ElementSerializer.Read(reader, ref target[i]);
            }
        }

        public static void Write(BufferWriter writer, in Span<TElement> target)
        {
            writer.WriteVariantInt32(target.Length);
            for (var i = 0; i < target.Length; i++)
            {
                m_ElementSerializer.Write(writer, in target[i]);
            }
        }

        public static void Copy(in Span<TElement> src, ref Span<TElement> dst)
        {
            throw new NotImplementedException();
        }
    }

    public class ReadOnlySpanSerializer<TElement> : ICompactBufferSerializer
    {
        private static ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();

        public static void Read(BufferReader reader, ref ReadOnlySpan<TElement> target)
        {
            Span<TElement> span = default;
            SpanSerializer<TElement>.Read(reader, ref span);
            target = span;
        }

        public static void Write(BufferWriter writer, in ReadOnlySpan<TElement> target)
        {
            writer.WriteVariantInt32(target.Length);
            for (var i = 0; i < target.Length; i++)
            {
                m_ElementSerializer.Write(writer, in target[i]);
            }
        }

        public static void Copy(in ReadOnlySpan<TElement> src, ref ReadOnlySpan<TElement> dst)
        {
            throw new NotImplementedException();
        }
    }
}
