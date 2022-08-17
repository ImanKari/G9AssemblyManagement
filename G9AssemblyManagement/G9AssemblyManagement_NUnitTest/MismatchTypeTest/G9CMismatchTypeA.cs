using System;

namespace G9AssemblyManagement_NUnitTest.MismatchTypeTest
{
    public class G9CMismatchTypeA
    {
        public int Age = 32;
        public string Name = "G9TM";

        private readonly TimeSpan _time = new(9, 9, 9);

        public DateTime ExDateTime { set; get; } = DateTime.Parse("1990-09-01");

        public TimeSpan GetTime()
        {
            return _time;
        }
    }
}