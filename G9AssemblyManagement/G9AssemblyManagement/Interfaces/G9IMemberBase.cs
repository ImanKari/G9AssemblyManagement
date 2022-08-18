using System.Reflection;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Interfaces
{
    /// <summary>
    ///     An interface for working with usual members of the object.
    ///     <para />
    ///     Base part
    /// </summary>
    public interface G9IMemberBase
    {
        /// <summary>
        ///     Specifies the name of member
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Specifies the type of member
        /// </summary>
        G9EMemberType MemberType { get; }

        /// <summary>
        ///     Property to get the MemberInfo of member
        /// </summary>
        MemberInfo GetMemberInfo { get; }
    }
}