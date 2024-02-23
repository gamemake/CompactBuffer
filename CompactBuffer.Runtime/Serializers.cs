
using System;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public static class Serializers
    {
        private static Dictionary<Type, ICompactBufferSerializer> m_Serializers = GetAllSerializers();
        private static Dictionary<Type, ICompactBufferSerializer> GetAllSerializers()
        {
            var retval = new Dictionary<Type, ICompactBufferSerializer>();
            foreach (var type in CompactBufferUtils.EnumAllTypes(typeof(ICompactBufferSerializer)))
            {
                if (type.IsInterface) continue;
                if (type.IsAbstract) continue;

                var compactBuffer = type.GetCustomAttribute<OverwriteAttribute>();
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

        public static ICompactBufferSerializer Get(Type type)
        {
            m_Serializers.TryGetValue(type, out var serializer);
            return serializer;
        }

        public static ICompactBufferSerializer<T> Get<T>()
        {
            return SerializerGetter<T>.Serializer;
        }
    }
}
