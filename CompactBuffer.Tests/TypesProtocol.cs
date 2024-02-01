
using System;
using System.Collections.Generic;
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
    }

    [Protocol(0)]
    public interface IServerApi : IProtocol
    {
        void Call();
        void CallInt(int a);
        void CallPA(TypeClass pa);
        void CallVariant([VariantInt] int v1, [VariantInt] long v2, [VariantInt] uint v3, int v4, int v5);
        void CallGuid(Guid _guid);
        void CallEnum(EnumTypes _enum);
        void CallFloat16([Float16(1)] float v);
        void CallReadOnlySpan(ReadOnlySpan<byte> aaaaaaaa);
        void CallVaiantType(VaiantType vv);
        void CallTypeClassRefReadonly(ref readonly TypeClass a);
        void CallTypeClassRef(ref TypeClass a);
        void CallTypeStructRefReadonly(ref readonly TypeStruct a);
        void CallTypeStructRef(ref TypeStruct a);
        void CallIntArray(int[] array);
        void CallIntSpan(Span<int> span);
        void CallIntReadOnlySpan(ReadOnlySpan<int> span);
    }
}
