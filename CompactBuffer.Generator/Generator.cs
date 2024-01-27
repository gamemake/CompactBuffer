
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;

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
        }

        public bool IsBaseType(Type type)
        {
            return m_TypesShortName.ContainsKey(type);
        }

        public string GetTypeName(Type type)
        {
            if (m_TypesShortName.TryGetValue(type, out var name))
            {
                return name;
            }

            if (type.IsArray)
            {
                return $"{GetTypeName(type.GetElementType())}[]";
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return $"System.Collections.Generic.List<{GetTypeName(type.GetGenericArguments()[0])}>()";
                }
                if (type.GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    return $"System.Collections.Generic.HashSet<{GetTypeName(type.GetGenericArguments()[0])}>()";
                }
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return $"System.Collections.Generic.Dictionary<{GetTypeName(type.GetGenericArguments()[0])}, {GetTypeName(type.GetGenericArguments()[1])}>()";
                }
            }

            return type.FullName;
        }
    }
}
