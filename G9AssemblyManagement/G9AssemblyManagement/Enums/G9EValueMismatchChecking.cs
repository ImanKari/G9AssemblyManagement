namespace G9AssemblyManagement.Enums
{
    /// <summary>
    ///     Enum to specify mismatch checking in terms of the value of members.
    /// </summary>
    public enum G9EValueMismatchChecking : byte
    {
        /// <summary>
        ///     Specifies that if two members' values don't have a shared type and can't transfer their values, the process must ignore them.
        /// </summary>
        AllowMismatchValues,

        /// <summary>
        ///     Specifies that if two members' values don't have a shared type and can't transfer their values, the process must throw an exception.
        /// </summary>
        PreventMismatchValues
    }
}