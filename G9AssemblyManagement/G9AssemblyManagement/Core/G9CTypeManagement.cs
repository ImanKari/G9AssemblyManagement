using System;
using System.Collections.Generic;
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
        ///     Method to find and get derived types
        /// </summary>
        /// <param name="baseType">Specifies type to find inherited types.</param>
        /// <param name="ignoreAbstractType">If it's set 'true,' method ignores abstract type in the finding process.</param>
        /// <param name="ignoreInterfaceType">If it's set 'true', method ignores interface type in the finding process.</param>
        /// <param name="assemblies">
        ///     Specifies custom "assembly" to search inherited types; if set 'null,' method searches inherited types in all
        ///     assemblies.
        /// </param>
        /// <returns>A collection of types inherited by a specified type.</returns>
        public static List<Type> GetDerivedTypes(Type baseType, bool ignoreAbstractType = true,
            bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            var derivedTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                // Get all types from the given assembly
                var types = assembly.GetTypes();
                derivedTypes.AddRange(types.Where(type =>
                    IsInheritOf(type, baseType, ignoreAbstractType, ignoreInterfaceType)));
            }

            return derivedTypes;
        }

        /// <summary>
        ///     Method to check a type in terms of whether it's a subtype.
        /// </summary>
        /// <param name="type">Specifies type for checking.</param>
        /// <param name="baseType">Specifies type to find inherited types.</param>
        /// <param name="ignoreAbstractType">If it's set 'true,' method ignores abstract type in the finding process.</param>
        /// <param name="ignoreInterfaceType">If it's set 'true', method ignores interface type in the finding process.</param>
        /// <returns></returns>
        private static bool IsInheritOf(Type type, Type baseType, bool ignoreAbstractType, bool ignoreInterfaceType)
        {
            // If one of types is null or type equal with base type
            if (type == null || baseType == null || type == baseType)
                return false;

            // Check validation for ignoring
            if ((ignoreAbstractType && type.IsAbstract) || (ignoreInterfaceType && type.IsInterface))
                return false;

            // Generic type
            if (!baseType.IsGenericType)
            {
                if (!type.IsGenericType)
                    return baseType.IsInterface
                        ? type.GetInterfaces().Contains(baseType)
                        : type.IsSubclassOf(baseType);
            }
            else
            {
                // Interface
                if (baseType.IsInterface)
                    return type.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == baseType);

                // Set base type generic definition
                baseType = baseType.GetGenericTypeDefinition();
            }

            // Check generic base type in loop
            type = type.BaseType;
            var objectType = typeof(object);
            while (type != objectType && type != null)
            {
                var currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (currentType == baseType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }
    }
}