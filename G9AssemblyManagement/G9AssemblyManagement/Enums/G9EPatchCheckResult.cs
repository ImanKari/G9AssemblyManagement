namespace G9AssemblyManagement.Enums
{
    /// <summary>
    ///     Enum for specifying the result of checking the path.
    /// </summary>
    public enum G9EPatchCheckResult
    {
        /// <summary>
        ///     Specifies that everything according to the set condition is correct.
        /// </summary>
        Correct,

        /// <summary>
        ///     Specifies that the specified path of directory/file in terms of used characters for this Operation System is
        ///     incorrect.
        /// </summary>
        PathNameIsIncorrect,

        /// <summary>
        ///     Specifies that the specified hard drive in this path of directory/file is incorrect (indeed, the specified drive in
        ///     this Operation
        ///     System does not exist)
        /// </summary>
        PathDriveIsIncorrect,

        /// <summary>
        ///     Specifies that the specified path of directory/file does not exist.
        /// </summary>
        PathExistenceIsIncorrect
    }
}