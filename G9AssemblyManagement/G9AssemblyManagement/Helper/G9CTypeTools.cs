using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.Core;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for types
    /// </summary>
    public class G9CTypeTools
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
        public IList<Type> GetInheritedTypesFromType<TType>(
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            return GetInheritedTypesFromType(typeof(TType), ignoreAbstractType, ignoreInterfaceType, assemblies);
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
        public IList<Type> GetInheritedTypesFromType(Type type,
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            // Set assemblies
            assemblies = Equals(assemblies, null) || !assemblies.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : assemblies;
            return G9CTypeHandler.GetDerivedTypes(type, ignoreAbstractType, ignoreInterfaceType, assemblies);
        }

        /// <summary>
        ///     Method to check a type that is a built-in .NET type or not
        /// </summary>
        /// <param name="type">Specifies type to check.</param>
        /// <returns>The result will be true if the type is a built-in .NET type.</returns>
        public bool IsTypeBuiltInDotNetType(Type type)
        {
            return type.Namespace != null && type.Namespace.StartsWith("System");
        }

        /// <summary>
        ///     Method to check a type in terms of being enumerable
        ///     Note: By default, string type is ignored.
        /// </summary>
        /// <param name="type">Specifies a type for checking</param>
        /// <param name="stringIsIgnored">Specifies that a string type as an enumerable type must be ignored or not.</param>
        /// <returns>The result is true if it's an enumerable type</returns>
        public bool IsEnumerableType(Type type, bool stringIsIgnored = true)
        {
            return stringIsIgnored
                ? type.Name != nameof(String)
                  && type.GetInterface(nameof(IEnumerable)) != null
                : type.GetInterface(nameof(IEnumerable)) != null;
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
        public TToType SmartChangeType<TToType>(object objectItem, IFormatProvider formatProvider = null)
        {
            // Default format provider
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture;

            return (TToType)G9CTypeHandler.ParseTypeToAnotherType(objectItem, typeof(TToType), formatProvider);
        }

        /// <summary>
        ///     Method to change a type of object to another type.
        ///     The method tries many ways to convert one type to another type.
        /// </summary>
        /// <typeparam name="TCurrentType">Specifies current type of object.</typeparam>
        /// <param name="objectItem">Specifies current object.</param>
        /// <param name="toType">Specifies the final type of object after conversion.</param>
        /// <param name="formatProvider">
        ///     Specifies format provider
        ///     Note: By default, format provide is CultureInfo.InvariantCulture
        /// </param>
        /// <returns>Converted object</returns>
        public object SmartChangeType<TCurrentType>(TCurrentType objectItem, Type toType,
            IFormatProvider formatProvider = null)
        {
            // Default format provider
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture;

            return G9CTypeHandler.ParseTypeToAnotherType(objectItem, toType, formatProvider);
        }
    }
}