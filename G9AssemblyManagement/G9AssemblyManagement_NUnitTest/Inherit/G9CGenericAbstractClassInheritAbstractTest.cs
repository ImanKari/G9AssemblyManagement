using System;
using System.Net;
using System.Reflection;
using G9AssemblyManagement_NUnitTest.DataType;

namespace G9AssemblyManagement_NUnitTest.Inherit
{
    public class G9CGenericAbstractClassInheritAbstractTest : G9ATestGenericAbstractClassInheritAbstractClass<int,
        string, DateTime, IPAddress, Type>
    {
        public override Type Field5 { get; set; }

        public override void Initialize()
        {
            Field1 = 9;
            Field2 = "G9TM.Com";
            Field3 = DateTime.MaxValue;
            Field4 = IPAddress.Any;
            Field5 = Assembly.GetExecutingAssembly().GetType();
        }
    }
}