using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.Core;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for types
    /// </summary>
    public class G9CTypesHelper
    {
        /// <summary>
        ///     Method to get inherited types from a type
        /// </summary>
        /// <typeparam name="TType">Specifies type to find inherited types.</typeparam>
        /// <param name="ignoreAbstractType">If it's set 'true,' method ignores abstract type in the finding process.</param>
        /// <param name="ignoreInterfaceType">If it's set 'true', method ignores interface type in the finding process.</param>
        /// <param name="assemblies">
        ///     Specifies custom "assembly" to search inherited types; if set 'null,' method searches inherited types in all
        ///     assemblies.
        /// </param>
        /// <returns>A collection of types inherited by a specified type.</returns>
        public IList<Type> G9GetInheritedTypesFromType<TType>(
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            return G9GetInheritedTypesFromType(typeof(TType), ignoreAbstractType, ignoreInterfaceType, assemblies);
        }

        /// <summary>
        ///     Method to get inherited types from a type
        /// </summary>
        /// <param name="type">Specifies type to find inherited types.</param>
        /// <param name="ignoreAbstractType">If it's set 'true,' method ignores abstract type in the finding process.</param>
        /// <param name="ignoreInterfaceType">If it's set 'true', method ignores interface type in the finding process.</param>
        /// <param name="assemblies">
        ///     Specifies custom "assembly" to search inherited types; if set 'null,' method searches inherited types in all
        ///     assemblies.
        /// </param>
        /// <returns>A collection of types inherited by a specified type.</returns>
        public IList<Type> G9GetInheritedTypesFromType(Type type,
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            // Set assemblies
            assemblies = Equals(assemblies, null) || !assemblies.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : assemblies;
            return G9CTypeManagement.GetDerivedTypes(type, ignoreAbstractType, ignoreInterfaceType, assemblies);
        }
    }
}