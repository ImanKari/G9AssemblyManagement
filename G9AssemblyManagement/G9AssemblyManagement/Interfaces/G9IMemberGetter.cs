namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for working with usual members of the object.
    ///     <para />
    ///     Getter part
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface G9IMemberGetter : G9IMemberBase
    {
        /// <summary>
        ///     Method to get member value
        /// </summary>
        /// <typeparam name="TType">Specifies value type</typeparam>
        /// <returns>Return value</returns>
        TType GetValue<TType>();

        /// <inheritdoc cref="GetValue{TType}" />
        object GetValue();

        /// <summary>
        ///     Method to get member value on another object with the same structure.
        ///     <para />
        ///     The member value can be obtained on another object with the same structure if needed.
        /// </summary>
        /// <typeparam name="TType">Specifies value type</typeparam>
        /// <param name="anotherSameObject">
        ///     Specifies another object for getting the value that must have the same structure as the primary object.
        /// </param>
        /// <returns>Return value</returns>
        TType GetValueOnAnotherObject<TType>(object anotherSameObject);

        /// <inheritdoc cref="GetValueOnAnotherObject{TType}" />
        object GetValueOnAnotherObject(object anotherSameObject);
    }
}