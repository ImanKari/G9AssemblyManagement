using G9AssemblyManagement_NUnitTest.DataType;

namespace G9AssemblyManagement_NUnitTest.Inherit
{
    public struct G9DtStructInheritTest : G9ITestType
    {
        public string NickName { get; set; }
        public string LastName { get; set; }

        public void Initialize()
        {
            NickName = "Iman";
            LastName = "Kari";
        }
    }
}