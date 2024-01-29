
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;

namespace CompactBuffer
{
    public static class CompactBufferUtils
    {
        public static IEnumerable<Type> EnumAllTypes(Type parentType)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = null;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"failed to get types from {assembly.FullName} with excepetion : {e}");
                }

                if (types != null)
                {
                    foreach (var type in types)
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