
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace CompactBuffer.Internal
{
    public class SpanAllocator
    {
        private static ThreadLocal<SpanAllocator> m_TLS = new ThreadLocal<SpanAllocator>();
        private static SpanAllocator TLS
        {
            get
            {
                var tls = m_TLS.Value;
                if (tls == null)
                {
                    tls = new SpanAllocator();
                    m_TLS.Value = tls;
                }
                return tls;
            }
        }

        private List<IntPtr> m_Buffers = new List<IntPtr>();
        private ushort m_Slot = 0;
        private ushort m_Used = 0;

        private SpanAllocator()
        {
            m_Buffers.Add(Marshal.AllocHGlobal(ushort.MaxValue));
        }

        public static uint Begin()
        {
            var tls = TLS;
            return ((uint)tls.m_Slot << 16) | tls.m_Used;
        }

        unsafe public static Span<T> Alloc<T>(int count)
            where T : new()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                return new Span<T>(new T[count]);
            }

            var size = Marshal.SizeOf<T>() * count;
            if (size > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException("size", size, $"size ({size}) must be between 0 and {ushort.MaxValue}");
            }
            var tls = TLS;
            if (tls.m_Used + size > ushort.MaxValue)
            {
                tls.m_Slot += 1;
                tls.m_Used = 0;
                if (tls.m_Slot == tls.m_Buffers.Count)
                {
                    tls.m_Buffers.Add(Marshal.AllocHGlobal(ushort.MaxValue));
                }
            }

            var ptr = tls.m_Buffers[tls.m_Slot] + tls.m_Used;
            return new Span<T>((void*)ptr, count);
        }

        public static void End(ulong top)
        {
            var tls = TLS;
            tls.m_Slot = (ushort)(top >> 16);
            tls.m_Used = (ushort)(top & 0xffff);
        }
    }
}
