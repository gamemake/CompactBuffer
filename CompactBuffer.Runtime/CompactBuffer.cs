
using System;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public static class CompactBuffer
    {
        private static Dictionary<Type, ICompactBufferSerializer> m_Serializers = GetAllSerializers();
        private static Dictionary<Type, ICompactBufferSerializer> GetAllSerializers()
        {
            var retval = new Dictionary<Type, ICompactBufferSerializer>();
            foreach (var type in CompactBufferUtils.EnumAllTypes(typeof(ICompactBufferSerializer)))
            {
                if (type.IsInterface) continue;
                if (type.IsAbstract) continue;

                var compactBuffer = type.GetCustomAttribute<CompactBufferAttribute>();
                if (compactBuffer != null)
                {
                    var serializer = (ICompactBufferSerializer)Activator.CreateInstance(type);
                    retval.Add(compactBuffer.SerializerType, serializer);
                }
            }
            return retval;
        }

        internal static class SerializerGetter<T>
        {
            private static ICompactBufferSerializer<T> m_Serializer = null;

            public static ICompactBufferSerializer<T> Serializer
            {
                get
                {
                    if (m_Serializer == null)
                    {
                        if (m_Serializers.TryGetValue(typeof(T), out var serializer))
                        {
                            m_Serializer = (ICompactBufferSerializer<T>)serializer;
                        }
                    }
                    return m_Serializer;
                }
                set
                {
                    if (m_Serializer != null)
                    {
                        m_Serializer = value;
                    }
                }
            }
        }

        internal static class CustomSerializerGetter<TSerializer, T>
            where TSerializer : ICompactBufferSerializer<T>, new()
        {
            public static TSerializer m_Serializer = new TSerializer();
        }

        public static ICompactBufferSerializer GetSerializer(Type type)
        {
            m_Serializers.TryGetValue(type, out var serializer);
            return serializer;
        }

        public static ICompactBufferSerializer<T> GetSerializer<T>()
        {
            return SerializerGetter<T>.Serializer;
        }

        public static ICompactBufferSerializer<T[]> GetArraySerializer<T>()
        {
            var serializer = SerializerGetter<T[]>.Serializer;
            if (serializer == null)
            {
                serializer = new Internal.ArraySerializer<T>();
                SerializerGetter<T[]>.Serializer = serializer;
            }
            return serializer;
        }

        public static ICompactBufferSerializer<List<T>> GetListSerializer<T>()
        {
            var serializer = SerializerGetter<List<T>>.Serializer;
            if (serializer == null)
            {
                serializer = new Internal.ListSerializer<T>();
                SerializerGetter<List<T>>.Serializer = serializer;
            }
            return serializer;
        }

        public static ICompactBufferSerializer<HashSet<T>> GetHashSetSerializer<T>()
        {
            var serializer = SerializerGetter<HashSet<T>>.Serializer;
            if (serializer == null)
            {
                serializer = new Internal.HashSetSerializer<T>();
                SerializerGetter<HashSet<T>>.Serializer = serializer;
            }
            return serializer;
        }

        public static ICompactBufferSerializer<Dictionary<TKey, TValue>> GetDictionarySerializer<TKey, TValue>()
        {
            var serializer = SerializerGetter<Dictionary<TKey, TValue>>.Serializer;
            if (serializer == null)
            {
                serializer = new Internal.DictionarySerializer<TKey, TValue>();
                SerializerGetter<Dictionary<TKey, TValue>>.Serializer = serializer;
            }
            return serializer;
        }

        public static ICompactBufferSerializer<T> GetCustomSerializer<TSerializer, T>()
            where TSerializer : ICompactBufferSerializer<T>, new()
        {
            return CustomSerializerGetter<TSerializer, T>.m_Serializer;
        }
    }
}
