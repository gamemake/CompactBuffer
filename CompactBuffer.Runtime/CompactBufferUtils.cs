
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CompactBuffer
{
    public static class CompactBufferUtils
    {
        public static IEnumerable<Type> EnumAllClass(Type parentType, bool instance = true)
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
                        if (type.IsAbstract && instance)
                        {
                            continue;
                        }

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

        public static int ReadLength(this BinaryReader reader)
        {
            var b0 = reader.ReadByte();
            if (b0 < 0x80) return b0;
            var b1 = reader.ReadByte();
            if (b1 < 0x80) return (b1 << 7) | (b0 & 0x7f);
            var b2 = reader.ReadByte();
            if (b2 < 0x80) return (b2 << 14) | (b1 & 0x7f) << 7 | (b0 & 0x7f);
            var b3 = reader.ReadByte();
            return b3 << 21 | (b2 & 0x7f) << 14 | (b1 & 0x7f) << 7 | (b0 & 0x7f);
        }

        public static void WriteLength(this BinaryWriter writer, int length)
        {
            if (length < 0 || length >= (1 << 29))
            {
                throw new ArgumentException("length");
            }

            if (length < (1 << 7))
            {
                writer.Write((byte)length);
            }
            else if (length < (1 << 14))
            {
                writer.Write((byte)((length & 0x7f) | 0x80));
                writer.Write((byte)(length >> 7));
            }
            else if (length < (1 << 21))
            {
                writer.Write((byte)((length & 0x7f) | 0x80));
                writer.Write((byte)(((length >> 7) & 0x7f) | 0x80));
                writer.Write((byte)(length >> 14));
            }
            else
            {
                writer.Write((byte)((length & 0x7f) | 0x80));
                writer.Write((byte)(((length >> 7) & 0x7f) | 0x80));
                writer.Write((byte)(((length >> 14) & 0x7f) | 0x80));
                writer.Write((byte)(length >> 21));
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