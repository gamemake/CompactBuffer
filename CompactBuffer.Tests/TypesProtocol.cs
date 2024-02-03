
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CompactBuffer;

namespace Tests
{
    public class TypeClass
    {
        public int kkk;
        public List<int> list;
        public HashSet<int> hashset;
        public Dictionary<string, int> dict;
    }

    public struct TypeStruct
    {
        public int i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public int[] array;
    }

    [ProtocolId(0)]
    public interface IServerApi : IProtocol
    {
        void Call();
        void CallInt(int a);
        void CallIntArray(int[] array);
        void CallString(string a);
        void CallVariant([Variant] int v1, [Variant] long v2, [Variant] uint v3, int v4, int v5);
        void CallGuid(Guid _guid);
        void CallEnum(EnumTypes _enum);
        void CallFloat16([Float16(1)] float v);
        void CallTypeClass(TypeClass pa);
        void CallTypeClassArray(TypeClass[] pa);
        void CallTypeClassIn(in TypeClass a);
        void CallTypeClassRef(ref TypeClass a);
        void CallTypeStruct(TypeStruct a);
        void CallTypeStructIn(in TypeStruct a);
        void CallTypeStructRef(ref TypeStruct a);
        void CallIntSpan(Span<int> span);
        void CallIntReadOnlySpan(ReadOnlySpan<int> span);
        void CallReadOnlySpan(ReadOnlySpan<byte> aaaaaaaa);
    }
}
