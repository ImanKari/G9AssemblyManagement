using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace G9AssemblyManagement_NUnitTest.ObjectMembers
{
    public class G9CObjectParent
    {
        [IgnoreDataMember]
        private string FullName { set; get; } = "G9TM-Parent";

        [IgnoreDataMember]
        private int _age = 99;

        private static int _staticAge = 99;

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