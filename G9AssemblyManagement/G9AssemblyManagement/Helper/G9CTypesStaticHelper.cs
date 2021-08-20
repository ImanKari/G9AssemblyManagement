using System;
using System.Collections.Generic;
using System.Reflection;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for types
    /// </summary>
    public static class G9CTypesStaticHelper
    {
        /// <summary>
        ///     Extension method to get inherited types from a type
        /// </summary>
        /// <param name="objectItem">Specifies an object to get type and find inherited types.</param>
        /// <param name="ignoreAbstractType">If set it 'true', method ignores abstract type in find.</param>
        /// <param name="ignoreInterfaceType">If set it 'true', method ignores interface type in find.</param>
        /// <param name="assemblies">
        ///     Specifies custom assembly to search inherited types, if set 'null', method search inherited
        ///     types in all assemblies.
        /// </param>
        /// <returns>Collection of types inherited specifies type.</returns>
        public static IList<Type> GetInheritedTypesFromType(this object objectItem,
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            return G9CAssemblyManagement.Types.GetInheritedTypesFromType(objectItem.GetType(), ignoreAbstractType,
                ignoreInterfaceType, assemblies);
        }
    }
}