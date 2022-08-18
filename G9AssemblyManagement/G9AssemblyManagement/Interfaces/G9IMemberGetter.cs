namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for working with usual members of the object.
    ///     <para />
    ///     Getter part
    /// </summary>
    public interface G9IMemberGetter : G9IMemberBase
    {
        /// <summary>
        ///     Method to get member value
        /// </summary>
        /// <typeparam name="TType">Specifies value type</typeparam>
        /// <returns>Return value</returns>
        TType GetValue<TType>();

        /// <summary>
        ///     Method to get member value
        /// </summary>
        /// <returns>Return value</returns>
        object GetValue();
    }
}