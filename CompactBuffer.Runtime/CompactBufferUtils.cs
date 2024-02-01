
using System;
using System.Linq;
using System.Collections.Generic;

namespace CompactBuffer
{
    public static class CompactBufferUtils
    {
        public static IEnumerable<Type> EnumAllTypes(Type parentType)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (parentType.IsInterface)
                    {
                        foreach (var itype in type.GetInterfaces())
                        {
                            if (itype == parentType)
                            {
                                yield return type;
                            }
                        }
                    }
                    else
                    {
                        if (type.IsSubclassOf(parentType))
                        {
                            yield return type;
                        }
                    }
                }
            }
        }

        public static void Resize<T>(this List<T> list, int sz, T c)
        {
            int cur = list.Count;
            if (sz < cur)
                list.RemoveRange(sz, cur - sz);
            else if (sz > cur)
            {
                if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                    list.Capacity = sz;
                list.AddRange(Enumerable.Repeat(c, sz - cur));
            }
        }

        public static void Resize<T>(this List<T> list, int sz) where T : new()
        {
            Resize(list, sz, new T());
        }
    }
}
