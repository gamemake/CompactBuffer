
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
                        else
                        {
                            var name = $"{typeof(T).FullName}";
                            throw new CompactBufferExeption($"{typeof(T).FullName}");
                        }
                    }
                    return m_Serializer;
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
            if (m_Serializers.TryGetValue(typeof(T[]), out var serializer))
            {
                return (ICompactBufferSerializer<T[]>)serializer;
            }
            else
            {
                serializer = new Internal.ArraySerializer<T>();
                m_Serializers.TryAdd(typeof(T[]), serializer);
                return (ICompactBufferSerializer<T[]>)serializer;
            }
        }

        public static ICompactBufferSerializer<List<T>> GetListSerializer<T>()
        {
            if (m_Serializers.TryGetValue(typeof(List<T>), out var serializer))
            {
                return (ICompactBufferSerializer<List<T>>)serializer;
            }
            else
            {
                serializer = new Internal.ListSerializer<T>();
                m_Serializers.TryAdd(typeof(List<T>), serializer);
                return (ICompactBufferSerializer<List<T>>)serializer;
            }
        }

        public static ICompactBufferSerializer<HashSet<T>> GetHashSetSerializer<T>()
        {
            if (m_Serializers.TryGetValue(typeof(HashSet<T>), out var serializer))
            {
                return (ICompactBufferSerializer<HashSet<T>>)serializer;
            }
            else
            {
                serializer = new Internal.HashSetSerializer<T>();
                m_Serializers.TryAdd(typeof(HashSet<T>), serializer);
                return (ICompactBufferSerializer<HashSet<T>>)serializer;
            }
        }

        public static ICompactBufferSerializer<Dictionary<TKey, TValue>> GetDictionarySerializer<TKey, TValue>()
        {
            if (m_Serializers.TryGetValue(typeof(Dictionary<TKey, TValue>), out var serializer))
            {
                return (ICompactBufferSerializer<Dictionary<TKey, TValue>>)serializer;
            }
            else
            {
                serializer = new Internal.DictionarySerializer<TKey, TValue>();
                m_Serializers.TryAdd(typeof(Dictionary<TKey, TValue>), serializer);
                return (ICompactBufferSerializer<Dictionary<TKey, TValue>>)serializer;
            }
        }

        public static ICompactBufferSerializer<T> GetCustomSerializer<TSerializer, T>()
            where TSerializer : ICompactBufferSerializer<T>, new()
        {
            return CustomSerializerGetter<TSerializer, T>.m_Serializer;
        }
    }
}
