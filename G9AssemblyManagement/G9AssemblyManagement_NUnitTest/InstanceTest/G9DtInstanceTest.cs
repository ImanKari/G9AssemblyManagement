using G9AssemblyManagement;

namespace G9AssemblyManagement_NUnitTest.InstanceTest
{
    public readonly struct G9DtInstanceTest
    {
        public G9DtInstanceTest(string firstName)
        {
            FirstName = firstName;
            G9CAssemblyManagement.Instances.AssignInstanceOfType(this);
        }

        public readonly string FirstName;

        public string GetFirstName()
        {
            return FirstName;
        }
    }
}