using System;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     A data type for defining a custom action for testing performance speed
    /// </summary>
    public class G9DtCustomPerformanceAction
    {
        /// <summary>
        ///     Specifies the custom action for getting a performance test.
        /// </summary>
        public readonly Action CustomAction;

        /// <summary>
        ///     Specifies a custom name for the action (which leads to a more readable result).
        /// </summary>
        public readonly string CustomName;

        /// <summary>
        ///     Constructor for initializing
        /// </summary>
        /// <param name="customName">Specifies a custom name for the action (which leads to a more readable result).</param>
        /// <param name="customAction">Specifies the custom action for getting a performance test.</param>
        public G9DtCustomPerformanceAction(string customName, Action customAction)
        {
            CustomName = customName;
            CustomAction = customAction;
        }
    }
}