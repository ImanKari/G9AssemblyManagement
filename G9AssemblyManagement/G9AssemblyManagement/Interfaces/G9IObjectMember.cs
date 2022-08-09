using System.Collections.Generic;
using System.Reflection;

namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for common member things of object
    /// </summary>
    public interface G9IObjectMember
    {
        /// <summary>
        ///     Specifies the name of member
        /// </summary>
        string Name { get; }

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

        /// <summary>
        ///     Method to get specified attributes on a member
        /// </summary>
        /// <typeparam name="TType">Specifies the type of an attribute</typeparam>
        /// <param name="inherit">
        ///     true to search this member's inheritance chain to find the attributes; otherwise, false. This
        ///     parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <returns>A collection of attributes if that existed</returns>
        IList<TType> GetCustomAttributes<TType>(bool inherit) where TType : System.Attribute;

        /// <summary>
        ///     Method to get the MemberInfo of member
        /// </summary>
        /// <returns>MemberInfo</returns>
        MemberInfo GetMemberInfo();
    }
}