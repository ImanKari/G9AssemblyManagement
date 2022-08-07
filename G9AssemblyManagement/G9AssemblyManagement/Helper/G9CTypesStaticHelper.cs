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
        /// <param name="ignoreAbstractType">If it's set 'true,' method ignores abstract type in the finding process.</param>
        /// <param name="ignoreInterfaceType">If it's set 'true', method ignores interface type in the finding process.</param>
        /// <param name="assemblies">
        ///     Specifies custom "assembly" to search inherited types; if set 'null,' method searches inherited types in all
        ///     assemblies.
        /// </param>
        /// <returns>A collection of types inherited by a specified type.</returns>
        public static IList<Type> G9GetInheritedTypesFromType(this object objectItem,
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            return G9CAssemblyManagement.TypeHandlers.G9GetInheritedTypesFromType(objectItem.GetType(), ignoreAbstractType,
                ignoreInterfaceType, assemblies);
        }
    }
}