using System;
using System.ComponentModel.Design;

namespace G9AssemblyManagement_NUnitTest.ObjectMembers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class G9AttrCustomForTestAttribute : Attribute
    {
        public G9AttrCustomForTestAttribute(string hint)
        {
            Hint = hint;
        }

        public string Hint { get; }
    }

    public class G9DtObjectForAttributes
    {
        [HelpKeyword("Test1")] public string Name { get; set; } = "Iman";

        [G9AttrCustomForTest("Test 1")]
        [G9AttrCustomForTest("Test 2")]
        [G9AttrCustomForTest("Test 3")]
        public string Family { get; set; } = "Kari";
    }
}