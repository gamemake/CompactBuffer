
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CompactBuffer
{
    public abstract class Generator
    {
        private Dictionary<Type, string> m_TypesShortName = new Dictionary<Type, string>();

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

        public bool IsVariantable(Type type)
        {
            if (type == typeof(int)) return true;
            if (type == typeof(long)) return true;
            return false;
        }
    }
}
