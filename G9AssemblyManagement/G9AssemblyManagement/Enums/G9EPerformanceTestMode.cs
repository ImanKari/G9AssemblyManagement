namespace G9AssemblyManagement.Enums
{
    /// <summary>
    ///     Enum for specifying the mode of testing.
    /// </summary>
    public enum G9EPerformanceTestMode
    {
        /// <summary>
        ///     Specifies that test must occur on the single-core.
        /// </summary>
        SingleCore,

        /// <summary>
        ///     Specifies that test must occur on the multi-core.
        /// </summary>
        MultiCore,

        /// <summary>
        ///     Specifies that test must occur on the single-core and multi-core (both of them).
        /// </summary>
        Both
    }
}