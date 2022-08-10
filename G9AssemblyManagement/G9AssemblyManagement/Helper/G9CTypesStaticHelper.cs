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
            return G9CAssemblyManagement.TypeHandlers.GetInheritedTypesFromType(objectItem.GetType(),
                ignoreAbstractType,
                ignoreInterfaceType, assemblies);
        }

        /// <summary>
        ///     Method to check a type that is a built-in .NET type or not
        /// </summary>
        /// <param name="type">Specifies type to check.</param>
        /// <returns>The result will be true if the type is a built-in .NET type.</returns>
        public static bool G9IsTypeBuiltInDotNetType(this Type type)
        {
            return G9CAssemblyManagement.TypeHandlers.IsTypeBuiltInDotNetType(type);
        }

        /// <summary>
        ///     Method to check a type in terms of being enumerable
        ///     Note: By default, string type is ignored.
        /// </summary>
        /// <param name="type">Specifies a type for checking</param>
        /// <param name="stringIsIgnored">Specifies that a string type as an enumerable type must be ignored or not.</param>
        /// <returns>The result is true if it's an enumerable type</returns>
        public static bool G9IsEnumerableType(this Type type, bool stringIsIgnored = true)
        {
            return G9CAssemblyManagement.TypeHandlers.IsEnumerableType(type, stringIsIgnored);
        }

        /// <summary>
        ///     Method to change a type of object to another type.
        ///     The method tries many ways to convert one type to another type.
        /// </summary>
        /// <typeparam name="TToType">Specifies the final type of object after conversion.</typeparam>
        /// <param name="objectItem">Specifies current object.</param>
        /// <param name="formatProvider">
        ///     Specifies format provider
        ///     Note: By default, format provide is CultureInfo.InvariantCulture
        /// </param>
        /// <returns>Converted object</returns>
        public static TToType G9SmartChangeType<TToType>(this object objectItem,
            IFormatProvider formatProvider = null)
        {
            return G9CAssemblyManagement.TypeHandlers.SmartChangeType<TToType>(objectItem, formatProvider);
        }

        /// <summary>
        ///     Method to change a type of object to another type.
        ///     The method tries many ways to convert one type to another type.
        /// </summary>
        /// <param name="objectItem">Specifies current object.</param>
        /// <param name="toType">Specifies the final type of object after conversion.</param>
        /// <param name="formatProvider">
        ///     Specifies format provider
        ///     Note: By default, format provide is CultureInfo.InvariantCulture
        /// </param>
        /// <returns>Converted object</returns>
        public static object G9SmartChangeType(this object objectItem, Type toType,
            IFormatProvider formatProvider = null)
        {
            return G9CAssemblyManagement.TypeHandlers.SmartChangeType(objectItem, toType, formatProvider);
        }
    }
}