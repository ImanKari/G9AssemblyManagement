namespace G9AssemblyManagement_NUnitTest.Types
{
    public class G9DtGenericTypeByConstructor<TType1, TType2, TType3>
    {
        public G9DtGenericTypeByConstructor(TType1 objectType1, TType2 objectType2, TType3 objectType3)
        {
            ObjectType1 = objectType1;
            ObjectType2 = objectType2;
            ObjectType3 = objectType3;
        }

        public TType1 ObjectType1 { get; }

        public TType2 ObjectType2 { get; }

        public TType3 ObjectType3 { get; }
    }
}