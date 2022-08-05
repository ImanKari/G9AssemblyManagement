using System;

namespace G9AssemblyManagement.Enums
{
    /// <summary>
    ///     Enum to specifies modifier type in the search
    /// </summary>
    [Flags]
    public enum G9EAccessModifier : byte
    {
        /// <summary>
        ///     Specifies that all members are to be included in the search.
        /// </summary>
        Everything = 0,

        /// <summary>
        ///     Specifies that public members are to be included in the search.
        /// </summary>
        Public = 1,

        /// <summary>
        ///     Specifies that non-public members are to be included in the search.
        /// </summary>
        NonPublic = 2,

        /// <summary>
        ///     Specifies that static members are to be included in the search.
        /// </summary>
        Static = 4
    }
}