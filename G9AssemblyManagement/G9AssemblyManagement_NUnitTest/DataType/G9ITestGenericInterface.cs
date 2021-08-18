namespace G9AssemblyManagement_NUnitTest.DataType
{
    public interface G9ITestGenericInterface<T1, T2, T3, T4>
    {
        T1 Field1 { set; get; }
        T2 Field2 { set; get; }
        T3 Field3 { set; get; }
        T4 Field4 { set; get; }

        void Initialize();
    }
}