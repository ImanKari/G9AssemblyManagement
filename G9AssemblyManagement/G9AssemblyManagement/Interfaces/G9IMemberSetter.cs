namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for working with usual members of the object.
    ///     <para />
    ///     Setter part
    /// </summary>
    public interface G9IMemberSetter : G9IMemberBase
    {
        /// <summary>
        ///     Method to set member value
        /// </summary>
        /// <typeparam name="TType">Type of value</typeparam>
        /// <param name="value">Specifies value</param>
        void SetValue<TType>(TType value);

        /// <summary>
        ///     Method to set member value
        /// </summary>
        /// <param name="value">Specifies value</param>
        void SetValue(object value);
    }
}