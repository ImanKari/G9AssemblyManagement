using System;

namespace G9AssemblyManagement_NUnitTest.MismatchTypeTest
{
    public class G9CMismatchTypeC2
    {
        public string Age = "nine";

        public string IpAddress = "192.168.1.1";
        public string Name = "G9TM 3";

        public string Percent = "69.3";

        private TimeSpan Time = new TimeSpan(3, 6, 9);

        public DateTime ExDateTime { set; get; } = DateTime.Parse("1999-03-09");
    }
}