namespace G9AssemblyManagement.DataType
{
    public struct G9DtTuple<TType>
    {
        public TType Item1;
        public TType Item2;

        public G9DtTuple(TType item1, TType item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}