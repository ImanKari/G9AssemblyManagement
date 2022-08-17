﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Core
{
    /// <summary>
    ///     Manage types and derived types
    /// </summary>
    internal static class G9CTypeHandler
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

        /// <summary>
        ///     The helper method to parse .NET built-in types
        /// </summary>
        /// <param name="value">Specifies a value for parsing.</param>
        /// <param name="specificType">Specifies a type for parsing to that.</param>
        /// <param name="formatProvider">
        ///     Specifies format provider
        ///     Note: By default, format provide is CultureInfo.InvariantCulture
        /// </param>
        /// <returns>parsed object</returns>
        internal static object ParseTypeToAnotherType(object value, Type specificType, IFormatProvider formatProvider)
        {
            var currentType = value.GetType();
            var currentTypeCode = Type.GetTypeCode(currentType);
            var specificTypeCode = Type.GetTypeCode(specificType);
            switch (specificTypeCode)
            {
                case TypeCode.String:
                    switch (currentTypeCode)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? "true" : "false";
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.DateTime:
                            return Convert.ChangeType(value, specificTypeCode, formatProvider);
                        default:
                            return value.ToString();
                    }
                case TypeCode.Char:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    try
                    {
                        return specificType.IsEnum
                            ? Enum.Parse(specificType, value.ToString())
                            : Convert.ChangeType(value, specificTypeCode, formatProvider);
                    }
                    catch
                    {
                        return ParseExceptionableTypes(value, currentType, currentTypeCode, specificType,
                            formatProvider);
                    }
                default:
                    return ParseExceptionableTypes(value, currentType, currentTypeCode, specificType, formatProvider);
            }
        }

        /// <summary>
        ///     Method to parse exceptionable types
        /// </summary>
        /// <param name="value">Specifies a value for parsing.</param>
        /// <param name="valueType">Specifies the type of value.</param>
        /// <param name="valueTypeCode">Specifies the type code of value.</param>
        /// <param name="specificType">Specifies a type for parsing to that.</param>
        /// <param name="formatProvider">
        ///     Specifies format provider
        ///     Note: By default, format provide is CultureInfo.InvariantCulture
        /// </param>
        /// <returns>parsed object</returns>
        private static object ParseExceptionableTypes(object value, Type valueType, TypeCode valueTypeCode,
            Type specificType,
            IFormatProvider formatProvider)
        {
            if (specificType.GetMethods().Any(s => s.Name == nameof(TimeSpan.Parse)))
            {
                var instance =
                    G9Assembly.InstanceTools.CreateUninitializedInstanceFromType(specificType);

                // The first search is related to the "Parse" method that has the same parameter type as the value type.
                var methods =
                    G9Assembly.ObjectTools.GetMethodsOfObject(
                        instance, G9EAccessModifier.Everything,
                        s =>
                            // Specifies name
                            s.Name == nameof(TimeSpan.Parse) &&
                            (
                                // Two parameter with IFormatProvider
                                (s.GetParameters().Length == 2 &&
                                 s.GetParameters()[0].ParameterType == valueType &&
                                 s.GetParameters()[1].ParameterType == typeof(IFormatProvider))
                                ||
                                // One math parameter
                                (s.GetParameters().Length == 1 && s.GetParameters()[0].ParameterType == valueType)
                            ));

                // If in the first search, the process can't find valuable methods. The second search tries to find the "Parse" method with string parameter type.
                if (valueTypeCode != TypeCode.String && methods == null)
                {
                    value = value.ToString();
                    methods = G9Assembly.ObjectTools.GetMethodsOfObject(instance,
                        G9EAccessModifier.Everything,
                        s =>
                            // Specifies name
                            (s.Name == nameof(TimeSpan.Parse) && s.GetParameters().Length == 2 &&
                             s.GetParameters()[0].ParameterType == typeof(string) &&
                             s.GetParameters()[1].ParameterType == typeof(IFormatProvider))
                            ||
                            // One math parameter
                            (s.GetParameters().Length == 1 &&
                             s.GetParameters()[0].ParameterType == typeof(string)));
                }

                // If methods existed, in this process, the "Parse" method would be executed one by one.
                if (methods != null)
                    foreach (var method in methods.OrderByDescending(s => s.MethodInfo.GetParameters().Length))
                        try
                        {
                            return method.MethodInfo.GetParameters().Length == 2
                                ? method.CallMethodWithResult<object>(value, formatProvider)
                                : method.CallMethodWithResult<object>(value);
                        }
                        catch
                        {
                            // Ignore
                        }
            }

            try
            {
                return Convert.ChangeType(value, specificType, formatProvider);
            }
            catch
            {
                return Convert.ChangeType(value, specificType);
            }
        }
    }
}