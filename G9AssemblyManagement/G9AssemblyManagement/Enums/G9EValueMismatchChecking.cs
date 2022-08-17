namespace G9AssemblyManagement.Enums
{
    /// <summary>
    ///     Enum to specify mismatch checking in terms of the value of members.
    /// </summary>
    public enum G9EValueMismatchChecking
    {
        /// <summary>
        ///     Specifies that if two members' values don't have a shared type, the process ignores them.
        /// </summary>
        AllowMismatchValues,

        /// <summary>
        ///     Specifies that if two members' values of objects don't have the same type, the process throws an exception.
        /// </summary>
        PreventMismatchValues
    }
}