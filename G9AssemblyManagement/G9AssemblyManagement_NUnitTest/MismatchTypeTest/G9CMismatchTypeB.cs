using System;
using System.Net;

namespace G9AssemblyManagement_NUnitTest.MismatchTypeTest
{
    public class G9CMismatchTypeB
    {
        public int Age = 99;

        public IPAddress IpAddress = IPAddress.IPv6Loopback;
        public string Name = "G9TM 2";

        public float Percent = 99.9f;

        private readonly TimeSpan _time = new(3, 6, 9);

        public DateTime ExDateTime { set; get; } = DateTime.Parse("1999-03-06");

        public TimeSpan GetTime()
        {
            return _time;
        }
    }
}