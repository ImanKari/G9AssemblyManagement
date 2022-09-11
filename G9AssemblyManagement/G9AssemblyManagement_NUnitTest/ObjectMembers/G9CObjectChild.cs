using System.Runtime.Serialization;

namespace G9AssemblyManagement_NUnitTest.ObjectMembers
{
    public class G9CObjectChild : G9CObjectParent
    {
        private static int _staticAge = 39;

        [IgnoreDataMember] private readonly int _age = 39;

        [IgnoreDataMember] private string FullName { set; get; } = "G9TM";

        [OnSerialized]
        private int GetAge()
        {
            return _age;
        }

        [OnSerialized]
        private TType GetFakeTypeValue<TType>(TType value)
        {
            return value;
        }
    }
}