
using CompactBuffer;

namespace Test
{
    public class PA
    {
        public int kkk;
    }

    [Protocol(0)]
    public interface IServerApi : IProtocol
    {
        void Call();
        void CallInt(int a);
        void CallPA(PA pa);
    }
}
