
using System;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public abstract class Generator
    {
        private Dictionary<Type, string> m_TypesShortName = new Dictionary<Type, string>();
        protected HashSet<Assembly> m_Assemblies = new HashSet<Assembly>();
        protected Dictionary<Type, Type> m_CustomSerializerTypes = new Dictionary<Type, Type>();

        protected Generator()
        {
            m_TypesShortName.Add(typeof(sbyte), "sbyte");
            m_TypesShortName.Add(typeof(short), "short");
            m_TypesShortName.Add(typeof(int), "int");
            m_TypesShortName.Add(typeof(long), "long");
            m_TypesShortName.Add(typeof(byte), "byte");
            m_TypesShortName.Add(typeof(ushort), "ushort");
            m_TypesShortName.Add(typeof(uint), "uint");
            m_TypesShortName.Add(typeof(ulong), "ulong");
            m_TypesShortName.Add(typeof(float), "float");
            m_TypesShortName.Add(typeof(double), "double");
            m_TypesShortName.Add(typeof(bool), "bool");
            m_TypesShortName.Add(typeof(string), "string");
            m_TypesShortName.Add(typeof(Guid), "System.Guid");
        }

        public bool IsBaseType(Type type)
        {
            return m_TypesShortName.ContainsKey(type);
        }

        public string GetTypeName(Type type)
        {
            var originType = type;
            if (type.IsByRef) originType = type.GetElementType();

            if (m_TypesShortName.TryGetValue(originType, out var name))
            {
                return name;
            }
            else if (originType.IsArray)
            {
                return $"{GetTypeName(originType.GetElementType())}[]";
            }
            else if (originType.IsGenericType)
            {
                var paramsName = string.Join(", ", Array.ConvertAll(originType.GetGenericArguments(), (x) => GetTypeName(x)));
                var className = originType.GetGenericTypeDefinition().FullName;
                className = className.Substring(0, className.IndexOf("`"));
                return $"{className}<{paramsName}>";
            }
            else
            {
                return $"{originType.FullName}";
            }
        }

        public bool Is7BitEncoded(Type type)
        {
            if (type == typeof(int)) return true;
            if (type == typeof(long)) return true;
            return false;
        }

        public void AddAssembly(Assembly assembly)
        {
            m_Assemblies.Add(assembly);

            foreach (var type in assembly.GetTypes())
            {
                if (!typeof(ICompactBufferSerializer).IsAssignableFrom(type)) continue;
                var attribute = type.GetCustomAttribute<OverwriteAttribute>();
                if (attribute == null) continue;
                if (attribute.IsAutoGen) continue;
                m_CustomSerializerTypes.Add(attribute.SerializerType, type);
            }
        }

        protected string GetSerializerName(Type type)
        {
            var originType = type;
            if (originType.IsByRef) originType = originType.GetElementType();

            if (m_CustomSerializerTypes.TryGetValue(originType, out var serializerType))
            {
                return GetTypeName(serializerType);
            }
            if (originType.IsArray)
            {
                return $"CompactBuffer.Internal.ArraySerializer<{GetTypeName(originType.GetElementType())}>";
            }
            if (!originType.IsGenericType)
            {
                return $"CompactBufferAutoGen.{originType.FullName.Replace(".", "_")}_Serializer";
            }
            var args = string.Join(",", Array.ConvertAll(originType.GetGenericArguments(), (x) => GetTypeName(x)));
            if (originType.GetGenericTypeDefinition() == typeof(Span<>))
            {
                return $"CompactBuffer.Internal.SpanSerializer<{args}>";
            }
            if (originType.GetGenericTypeDefinition() == typeof(ReadOnlySpan<>))
            {
                return $"CompactBuffer.Internal.ReadOnlySpanSerializer<{args}>";
            }
            if (originType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return $"CompactBuffer.Internal.ListSerializer<{args}>";
            }
            if (originType.GetGenericTypeDefinition() == typeof(HashSet<>))
            {
                return $"CompactBuffer.Internal.HashSetSerializer<{args}>";
            }
            if (originType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return $"CompactBuffer.Internal.DictionarySerializer<{args}>";
            }
            throw new CompactBufferException($"{GetTypeName(originType)} unsupport generic type");
        }
    }
}
