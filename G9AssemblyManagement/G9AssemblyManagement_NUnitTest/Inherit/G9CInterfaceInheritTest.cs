using System;
using G9AssemblyManagement_NUnitTest.DataType;

namespace G9AssemblyManagement_NUnitTest.Inherit
{
    public class G9CInterfaceInheritTest : G9ITestType
    {
        public DateTime CurrentDateTime;
        public string NickName { get; set; }
        public string LastName { get; set; }

        public void Initialize()
        {
            NickName = "Iman";
            LastName = "Kari";
            CurrentDateTime = DateTime.Now;
        }
    }
}