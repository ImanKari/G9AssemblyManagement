using System.Collections.Generic;
using System.Reflection;

namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for total object method members
    /// </summary>
    public interface G9IMethodMember
    {
        /// <summary>
        ///     Specifies method name
        /// </summary>
        string MethodName { get; }

        /// <summary>
        ///     Access to method info
        /// </summary>
        MethodInfo MethodInfo { get; }

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
        ///     Method to get specified attribute on a member
        /// </summary>
        /// <typeparam name="TType">Specifies the type of an attribute</typeparam>
        /// <param name="inherit">
        ///     true to search this member's inheritance chain to find the attributes; otherwise, false. This
        ///     parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <returns>An attribute object if that existed</returns>
        TType GetCustomAttribute<TType>(bool inherit) where TType : System.Attribute;
    }
}