namespace G9AssemblyManagement_NUnitTest.DataType
{
    public interface G9ITestType
    {
        string NickName { set; get; }

        string LastName { set; get; }

        void Initialize();
    }
}