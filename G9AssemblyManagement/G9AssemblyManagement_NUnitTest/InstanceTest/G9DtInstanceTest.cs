using System.Diagnostics;
using System.Reflection;
using G9AssemblyManagement;
using G9AssemblyManagement.Abstract;

namespace G9AssemblyManagement_NUnitTest.InstanceTest
{
    public readonly struct G9DtInstanceTest
    {
        public G9DtInstanceTest(string firstName)
        {
            FirstName = firstName;
            G9CAssemblyManagement.AssignInstanceOfType(this);
        }

        public readonly string FirstName;

        public string GetFirstName()
        {
            return FirstName;
        }
    }
}