namespace G9AssemblyManagement_NUnitTest.ObjectMembers
{
    public class G9CObjectChild : G9CObjectParent
    {
        private string FullName { set; get; } = "G9TM";

        private int _age = 39;

        private static int _staticAge = 39;

        private int GetAge()
        {
            return _age;
        }

        private TType GetFakeTypeValue<TType>(TType value)
        {
            return value;
        }
    }
}