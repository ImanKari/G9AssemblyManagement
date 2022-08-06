using System;
using System.Net;

namespace G9AssemblyManagement_NUnitTest.ObjectMembers
{
    public struct G9DtObjectMembersTest
    {
        public G9DtObjectMembersTest(string stringTest1, string stringTest2, decimal decimalTest1, decimal decimalTest2,
            IPAddress staticIpAddressTest1, IPAddress staticIpAddressTest2)
        {
            StringTest1 = stringTest1;
            StringTest2 = stringTest2;
            DecimalTest1 = decimalTest1;
            DecimalTest2 = decimalTest2;
            StaticIpAddressTest1 = staticIpAddressTest1;
            StaticIpAddressTest2 = staticIpAddressTest2;
        }

        public string StringTest1;
        private string StringTest2;

        public decimal DecimalTest1 { set; get; }
        private decimal DecimalTest2 { get; }

        public static IPAddress StaticIpAddressTest1;
        private static IPAddress StaticIpAddressTest2 { set; get; }

        public int TestMethod1(int a, int b)
        {
            return a + b;
        }

        internal void TestMethod2(out string a, ref DateTime b)
        {
            a = "OKAY";
            b = DateTime.MaxValue;
        }

        public static TType TestGenericMethod<TType>()
        {
            return default;
        }

        private (TType1, TType2, TType3) TestGenericMethod<TType1, TType2, TType3>(TType1 a, TType2 b, TType3 c)
        {
            return (a, b, c);
        }
    }
}