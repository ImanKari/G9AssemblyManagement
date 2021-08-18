namespace G9AssemblyManagement_NUnitTest.DataType
{
    public abstract class G9ATestGenericAbstractClass<T1, T2, T3, T4>
    {
        public T1 Field1 { set; get; }
        public T2 Field2 { set; get; }
        public T3 Field3 { set; get; }
        public T4 Field4 { set; get; }

        public abstract void Initialize();
    }
}