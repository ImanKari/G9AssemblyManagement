using System;
using System.Collections.Generic;
using System.Reflection;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for working with usual members of the object.
    ///     <para />
    ///     Base part
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface G9IMemberBase
    {
        /// <summary>
        ///     Specifies the name of member
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Specifies the basis type of member in terms of being field or property
        /// </summary>
        G9EMemberType MemberBasisType { get; }

        /// <summary>
        ///     Specifies the type of member
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        ///     Property to get the MemberInfo of member
        /// </summary>
        MemberInfo MemberInfo { get; }
    }
}