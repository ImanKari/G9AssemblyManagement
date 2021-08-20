namespace G9AssemblyManagement_NUnitTest.DataType
{
    public interface
        G9ITestGenericInterfaceInheritInterface<T1, T2, T3, T4, T5> : G9ITestGenericInterface<T1, T2, T3, T4>
    {
        T5 IsGame { set; get; }
    }
}