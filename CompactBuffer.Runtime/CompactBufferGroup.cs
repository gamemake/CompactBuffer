using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace CompactBuffer
{
    public abstract class CompactBufferGroup
    {
        private HashSet<Assembly> m_Assemblies = new HashSet<Assembly>();

        public CompactBufferGroup()
        {
            AddAssembly(GetType().Assembly);
        }

        protected void AddAssembly(Assembly assembly)
        {
            if (!m_Assemblies.Contains(assembly))
            {
                m_Assemblies.Add(assembly);
            }
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
    }
}
