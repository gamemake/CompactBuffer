
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace CompactBuffer
{
    public abstract class CompactBufferGroup
    {
        private HashSet<Assembly> m_Assemblies = new HashSet<Assembly>();
        private HashSet<Type> m_AdditionTypes = new HashSet<Type>();

        public CompactBufferGroup()
        {
            AddAssembly(GetType().Assembly);
        }

        public string GetAssemblyName()
        {
            return Path.GetFileNameWithoutExtension(GetType().Assembly.Location);
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            foreach (var assembly in m_Assemblies)
            {
                yield return assembly;
            }
        }

        public IEnumerable<Type> GetAdditionTypes()
        {
            foreach (var type in m_AdditionTypes)
            {
                yield return type;
            }
        }

        protected void AddAssembly(Assembly assembly)
        {
            if (!m_Assemblies.Contains(assembly))
            {
                m_Assemblies.Add(assembly);
            }
        }

        protected void AddAdditionType(Type type)
        {
            if (!m_AdditionTypes.Contains(type))
            {
                m_AdditionTypes.Add(type);
            }
        }
    }
}
