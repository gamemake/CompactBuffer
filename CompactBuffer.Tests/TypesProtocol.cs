
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

    public struct RefStruct
    {
        public int i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public int[] array;
    }

    [Protocol(0)]
    public interface IServerApi : IProtocol
    {
        void Call();
        void CallInt(int a);
        void CallString(string a);
        void CallTypeClass(TypeClass pa);
        void CallVariant([VariantInt] int v1, [VariantInt] long v2, [VariantInt] uint v3, int v4, int v5);
        void CallGuid(Guid _guid);
        void CallEnum(EnumTypes _enum);
        void CallFloat16([Float16(1)] float v);
        void CallReadOnlySpan(ReadOnlySpan<byte> aaaaaaaa);
        void CallVariantType(VaiantType vv);
        void CallTypeClassRefReadonly(in TypeClass a);
        void CallTypeClassRef(ref TypeClass a);
        void CallTypeStructRefReadonly(in RefStruct a);
        void CallTypeStructRef(ref RefStruct a);
        void CallIntArray(int[] array);
        void CallIntSpan(Span<int> span);
        void CallIntReadOnlySpan(ReadOnlySpan<int> span);
    }
}
