
using System.IO;
using System.Collections.Generic;

namespace CompactBuffer.Internal
{
    public class ArraySerializer<TElement> : ICompactBufferSerializer<TElement[]>
    {
        private static ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();

        public void Read(BinaryReader reader, ref TElement[] target)
        {
            var length = reader.ReadLength();
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

        public void Write(BinaryWriter writer, ref TElement[] target)
        {
            if (target == null)
            {
                writer.WriteLength(0);
                return;
            }

            writer.WriteLength(target.Length + 1);
            for (var i = 0; i < target.Length; i++)
            {
                m_ElementSerializer.Write(writer, ref target[i]);
            }
        }

        public void Copy(ref TElement[] src, ref TElement[] dst)
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
                m_ElementSerializer.Copy(ref src[i], ref dst[i]);
            }
        }
    }

    public class ListSerializer<TElement> : ICompactBufferSerializer<List<TElement>>
    {
        private ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();
        private bool m_IsValueType = typeof(TElement).IsValueType;

        public void Read(BinaryReader reader, ref List<TElement> target)
        {
            var length = reader.ReadLength();
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

        public void Write(BinaryWriter writer, ref List<TElement> target)
        {
            if (target == null)
            {
                writer.WriteLength(0);
                return;
            }

            writer.WriteLength(target.Count + 1);
            for (var i = 0; i < target.Count; i++)
            {
                var element = target[i];
                m_ElementSerializer.Write(writer, ref element);
                if (m_IsValueType) target[i] = element;
            }
        }

        public void Copy(ref List<TElement> src, ref List<TElement> dst)
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
                m_ElementSerializer.Copy(ref srcElement, ref dstElement);
                if (m_IsValueType) dst[i] = dstElement;
            }
        }
    }

    public class HashSetSerializer<TElement> : ICompactBufferSerializer<HashSet<TElement>>
    {
        private ICompactBufferSerializer<TElement> m_ElementSerializer = CompactBuffer.GetSerializer<TElement>();

        public void Read(BinaryReader reader, ref HashSet<TElement> target)
        {
            var length = reader.ReadLength();
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

        public void Write(BinaryWriter writer, ref HashSet<TElement> target)
        {
            if (target == null)
            {
                writer.WriteLength(0);
                return;
            }

            writer.WriteLength(target.Count + 1);
            foreach (var element in target)
            {
                var _element = element;
                m_ElementSerializer.Write(writer, ref _element);
            }
        }

        public void Copy(ref HashSet<TElement> src, ref HashSet<TElement> dst)
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
                m_ElementSerializer.Copy(ref srcElement, ref dstElement);
                dst.Add(dstElement);
            }
        }
    }

    public class DictionarySerializer<TKey, TValue> : ICompactBufferSerializer<Dictionary<TKey, TValue>>
    {
        private ICompactBufferSerializer<TKey> m_KeySerializer = CompactBuffer.GetSerializer<TKey>();
        private ICompactBufferSerializer<TValue> m_ValueSerializer = CompactBuffer.GetSerializer<TValue>();

        public void Read(BinaryReader reader, ref Dictionary<TKey, TValue> target)
        {
            var length = reader.ReadLength();
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

        public void Write(BinaryWriter writer, ref Dictionary<TKey, TValue> target)
        {
            if (target == null)
            {
                writer.WriteLength(0);
            }

            writer.WriteLength(target.Count + 1);
            foreach (var item in target)
            {
                var _key = item.Key;
                var _value = item.Value;
                m_KeySerializer.Write(writer, ref _key);
                m_ValueSerializer.Write(writer, ref _value);
            }
        }

        public void Copy(ref Dictionary<TKey, TValue> src, ref Dictionary<TKey, TValue> dst)
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
                m_KeySerializer.Copy(ref srcKey, ref dstKey);
                m_ValueSerializer.Copy(ref srcValue, ref dstValue);
                dst.Add(dstKey, dstValue);
            }
        }
    }
}
