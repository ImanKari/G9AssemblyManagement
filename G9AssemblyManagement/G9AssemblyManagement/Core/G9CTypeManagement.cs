using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace G9AssemblyManagement.Core
{
    /// <summary>
    ///     Manage types and derived types
    /// </summary>
    internal static class G9CTypeManagement
    {
        /// <summary>
        ///     Get derived types
        /// </summary>
        /// <param name="baseType">Specify base type</param>
        /// <param name="assemblies">Specify assemblies for search</param>
        /// <param name="ignoreAbstractType">If true => ignore abstract type</param>
        /// <param name="IgnoreInterfaceType">If true => ignore interface typeA</param>
        /// <returns>List of types</returns>

        #region GetDerivedTypes

        public static List<Type> GetDerivedTypes(Type baseType, bool ignoreAbstractType = true,
            bool IgnoreInterfaceType = true, params Assembly[] assemblies)
        {
            var derivedTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                // Get all types from the given assembly
                var types = assembly.GetTypes();

                for (int i = 0, count = types.Length; i < count; i++)
                {
                    var type = types[i];
                    if (IsInheritOf(type, baseType, ignoreAbstractType, IgnoreInterfaceType)) derivedTypes.Add(type);
                }
            }

            return derivedTypes;
        }

        #endregion

        /// <summary>
        ///     Specify type is sub class
        /// </summary>
        /// <param name="type">Specify type</param>
        /// <param name="baseType">Specify base ty[e</param>
        /// <param name="ignoreAbstractType">If true => ignore abstract type</param>
        /// <param name="ignoreInterfaceType">If true => ignore interface typeA</param>
        /// <returns></returns>

        #region IsSubclassOf

        private static bool IsInheritOf(Type type, Type baseType, bool ignoreAbstractType, bool ignoreInterfaceType)
        {
            if (type == null || baseType == null || type == baseType)
                return false;

            if (!baseType.IsGenericType)
            {
                if (!type.IsGenericType)
                {
                    if (!ignoreAbstractType && type.IsAbstract || !ignoreInterfaceType && type.IsInterface)
                        return false;
                    return baseType.IsInterface
                        ? type.GetInterfaces().Contains(baseType)
                        : type.IsSubclassOf(baseType);
                }
            }
            else
            {
                if (ignoreAbstractType && type.IsAbstract || ignoreInterfaceType && type.IsInterface) return false;
                if (baseType.IsInterface)
                    return type.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == baseType);
                baseType = baseType.GetGenericTypeDefinition();
            }

            type = type.BaseType;
            var objectType = typeof(object);
            while (type != objectType && type != null)
            {
                var currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                if (type.Name == "G9CGenericInterfaceTest" || currentType.Name == "G9CGenericInterfaceTest")
                    Debug.WriteLine("asdsad");

                if (currentType == baseType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }

        #endregion
    }
}