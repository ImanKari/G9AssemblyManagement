using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.Core;

namespace G9AssemblyManagement
{
    public static class G9CAssemblyManagement
    {
        #region ### Fields And Properties

        #endregion

        #region ### Methods ###

        #region GetInheritedTypesFromType Methods

        /// <summary>
        ///     Method to get inherited types from a type
        /// </summary>
        /// <typeparam name="TType">Specifies type to find inherited types.</typeparam>
        /// <param name="ignoreAbstractType">If set it 'true', method ignores abstract type in find.</param>
        /// <param name="ignoreInterfaceType">If set it 'true', method ignores interface type in find.</param>
        /// <param name="assemblies">
        ///     Specifies custom assembly to search inherited types, if set 'null', method search inherited
        ///     types in all assemblies.
        /// </param>
        /// <returns>Collection of types inherited specifies type.</returns>
        public static IList<Type> GetInheritedTypesFromType<TType>(
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            return GetInheritedTypesFromType(typeof(TType), ignoreAbstractType, ignoreInterfaceType, assemblies);
        }

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
            return GetInheritedTypesFromType(objectItem.GetType(), ignoreAbstractType, ignoreInterfaceType, assemblies);
        }

        /// <summary>
        ///     Method to get inherited types from a type
        /// </summary>
        /// <param name="type">Specifies type to find inherited types.</param>
        /// <param name="ignoreAbstractType">If set it 'true', method ignores abstract type in find.</param>
        /// <param name="ignoreInterfaceType">If set it 'true', method ignores interface type in find.</param>
        /// <param name="assemblies">
        ///     Specifies custom assembly to search inherited types, if set 'null', method search inherited
        ///     types in all assemblies.
        /// </param>
        /// <returns>Collection of types inherited specifies type.</returns>
        public static IList<Type> GetInheritedTypesFromType(Type type,
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            // Set assemblies
            assemblies = Equals(assemblies, null) || !assemblies.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : assemblies;
            return G9CTypeManagement.GetDerivedTypes(type, ignoreAbstractType, ignoreInterfaceType, assemblies);
        }

        #endregion

        /// <summary>
        ///     Method for assign a instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public static void AssignInstanceOfType(object instance)
        {
            G9CAssemblyHandler.AssignInstanceOfType(instance);
        }

        /// <summary>
        ///     Method for unassign a instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public static void UnassignInstanceOfType(object instance)
        {
            G9CAssemblyHandler.UnassignInstanceOfType(instance);
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <typeparam name="TType">
        ///     Specifies type to find instances of type.
        ///     <para />
        ///     Notice: The specified type must be one of the inherited types (class or struct).
        /// </typeparam>
        /// <returns>Return collection of instances of type.</returns>
        public static IList<TType> GetInstancesOfType<TType>()
        {
            return G9CAssemblyHandler.GetInstancesOfType<TType>();
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <param name="type">
        ///     Specifies type to find instances of type.
        ///     <para />
        ///     Notice: The specified type must be one of the inherited types (class or struct).
        /// </param>
        /// <returns>Return collection of instances of type.</returns>
        public static IList<object> GetInstancesOfType(Type type)
        {
            return G9CAssemblyHandler.GetInstancesOfType(type);
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <param name="objectItem">
        ///     Specifies an object to get type and find instances of type.
        ///     <para />
        ///     Notice: The specified object item must be one of the inherited types (class or struct).
        /// </param>
        /// <returns>Return collection of instances of type.</returns>
        public static IList<object> GetInstancesOfType(this object objectItem)
        {
            return G9CAssemblyHandler.GetInstancesOfType(objectItem.GetType());
        }

        #endregion
    }
}