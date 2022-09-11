namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for working with usual members of the object.
    ///     <para />
    ///     Setter part
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface G9IMemberSetter : G9IMemberBase
    {
        /// <summary>
        ///     Method to set member value
        /// </summary>
        /// <param name="value">Specifies value</param>
        void SetValue(object value);

        /// <summary>
        ///     Method to set member value on another object with the same structure.
        ///     <para />
        ///     The member value can be set on another object with the same structure if needed.
        /// </summary>
        /// <param name="anotherSameObject">
        ///     Specifies another object for setting the value that must have the same structure as the primary object.
        /// </param>
        /// <param name="value">Specifies value</param>
        void SetValueOnAnotherObject(object anotherSameObject, object value);
    }
}