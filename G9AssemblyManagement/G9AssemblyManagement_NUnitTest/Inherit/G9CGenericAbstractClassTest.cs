using System;
using System.Net;
using G9AssemblyManagement_NUnitTest.DataType;

namespace G9AssemblyManagement_NUnitTest.Inherit
{
    public class G9CGenericAbstractClassTest : G9ATestGenericAbstractClass<int, string, DateTime, IPAddress>
    {
        public override void Initialize()
        {
            Field1 = 9;
            Field2 = "G9TM.Com";
            Field3 = DateTime.MaxValue;
            Field4 = IPAddress.Any;
        }
    }
}