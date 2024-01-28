
using CompactBuffer;
using System.Collections.Generic;

namespace Test
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
    }
}
