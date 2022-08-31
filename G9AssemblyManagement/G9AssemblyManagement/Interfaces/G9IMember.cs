using System;
using System.Collections.Generic;

namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for working with usual members of the object.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface G9IMember : G9IMemberSetter, G9IMemberGetter
    {
        /// <summary>
        ///     Method to get specified attributes on a member
        /// </summary>
        /// <typeparam name="TType">Specifies the type of an attribute</typeparam>
        /// <param name="inherit">
        ///     true to search this member's inheritance chain to find the attributes; otherwise, false. This
        ///     parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <returns>A collection of attributes if that existed</returns>
        IList<TType> GetCustomAttributes<TType>(bool inherit) where TType : Attribute;
    }
}