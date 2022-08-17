namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     A custom tuple data type
    /// </summary>
    /// <typeparam name="TType">Specifies a custom type for tuple</typeparam>
    public struct G9DtTuple<TType>
    {
        /// <summary>
        ///     First item of tuple
        /// </summary>
        public TType Item1;

        /// <summary>
        ///     Second item of tuple
        /// </summary>
        public TType Item2;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="item1">Specifies the first item of tuple</param>
        /// <param name="item2">Specifies the second item of tuple</param>
        public G9DtTuple(TType item1, TType item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}