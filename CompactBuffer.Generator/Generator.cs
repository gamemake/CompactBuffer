
using System;
using System.Collections.Generic;

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
                var paramsName = string.Join(", ", Array.ConvertAll(type.GetGenericArguments(), string (x) =>
                {
                    return GetTypeName(x);

                }));
                var className = type.GetGenericTypeDefinition().FullName;
                className = className.Substring(0, className.IndexOf("`"));
                className = $"{className}<{paramsName}>";
                return className;
            }

            return type.FullName;
        }

        public string GetVariantIntName(Type type)
        {
            if(type==typeof(int)) return "Int";
            if(type==typeof(long)) return "Int64";
            return "";
        }
    }
}
