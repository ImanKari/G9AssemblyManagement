using System.Diagnostics;
using G9AssemblyManagement.Abstract;

namespace G9AssemblyManagement_NUnitTest.InstanceTest
{
    public class G9CMultiInstanceTest : G9AClassInitializer
    {
        public string GetClassName()
        {
            return nameof(G9CMultiInstanceTest);
        }
    }
}