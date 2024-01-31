
using System;
using System.Collections.Generic;
using CompactBuffer;

namespace Tests
{
    public class PA
    {
        public int kkk;
        public List<int> list;
        public HashSet<int> hashset;
        public Dictionary<string, int> dict;
    }

    [Protocol(0)]
    public interface IServerApi : IProtocol
    {
        void Call();
        void CallInt(int a);
        void CallPA(PA pa);
        void CallVariant([VariantInt] int v1, [VariantInt] long v2, [VariantInt] uint v3, int v4, int v5);
        void CallGuid(Guid _guid);
        void CallEnum(EnumTypes _enum);
        void CallFloat16([Float16(1)] float v);
        void CallReadOnlySpan(ReadOnlySpan<byte> aaaaaaaa);
        void CallVaiantType(VaiantType vv);
    }
}
