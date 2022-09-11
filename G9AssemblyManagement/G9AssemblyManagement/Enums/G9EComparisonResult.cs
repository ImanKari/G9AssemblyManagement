namespace G9AssemblyManagement.Enums
{
    /// <summary>
    ///     Enum for specifying the comparison result.
    /// </summary>
    public enum G9EComparisonResult : byte
    {
        /// <summary>
        ///     Specifies that comparing process must do by the core process.
        ///     <para />
        ///     Indeed, the comparison process is done by the core default algorithm.
        /// </summary>
        Skip,

        /// <summary>
        ///     Specifies that the members are equal.
        /// </summary>
        Equal,

        /// <summary>
        ///     Specifies that the members are not equal.
        /// </summary>
        Nonequal
    }
}