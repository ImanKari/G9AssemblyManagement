namespace G9AssemblyManagement_NUnitTest.ObjectMembers
{
    public class G9CObjectParent
    {
        private string FullName { set; get; } = "G9TM-Parent";

        private int _age = 99;

        private static int _staticAge = 99;

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