using System;
using System.Net;

namespace G9AssemblyManagement_NUnitTest.ObjectMembers
{
    public class G9CObjectMembersTest
    {
        public string StringTest1 = "A";
#pragma warning disable CS0414
        private string StringTest2 = "B";
#pragma warning restore CS0414

        public decimal DecimalTest1 { set; get; } = 999.999m;
        private decimal DecimalTest2 { get; } = 369.963m;

        public static IPAddress StaticIpAddressTest1 = IPAddress.Any;
        private static IPAddress StaticIpAddressTest2 { set; get; } = IPAddress.None;

        public int TestMethod1(int a, int b)
        {
            return a + b;
        }

        protected void TestMethod2(out string a, ref DateTime b)
        {
            a = "OKAY";
            b = DateTime.MaxValue;
        }

        public static TType TestGenericMethod<TType>()
        {
            return default;
        }

#if NETCOREAPP2_0_OR_GREATER
        private (TType1, TType2, TType3) TestGenericMethod<TType1, TType2, TType3>(TType1 a, TType2 b, TType3 c)
        {
            return (a, b, c);
        }
#else
        private TType1 TestGenericMethod<TType1>(TType1 a)
        {
            return a;
        }
#endif

    }
}