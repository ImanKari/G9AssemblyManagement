namespace G9AssemblyManagement_NUnitTest.DataType
{
    public class G9CTestType
    {
        public string NickName;

        public string LastName { set; get; }

        public void Initialize()
        {
            NickName = "Iman";
            LastName = "Kari";
        }
    }
}