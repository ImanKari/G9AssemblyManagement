using G9AssemblyManagement.Abstract;

namespace G9AssemblyManagement_NUnitTest.InstanceListener
{
    public class G9CInstanceListenerTest : G9AClassInitializer
    {
        private int _useCounter;

        public int GetUseCounter()
        {
            return ++_useCounter;
        }
    }
}