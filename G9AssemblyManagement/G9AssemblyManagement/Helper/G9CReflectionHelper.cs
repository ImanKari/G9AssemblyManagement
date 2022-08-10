using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for reflections
    /// </summary>
    public class G9CReflectionHelper
    {
        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter parameter if needed</param>
        /// <returns>A collection of fields</returns>
        public IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetFields(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray()
                : targetObject.GetType()
                    .GetFields(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray();
        }

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter parameter if needed</param>
        /// <returns>A collection of properties</returns>
        public IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<PropertyInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetProperties(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetProperties(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray();
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter parameter if needed</param>
        /// <returns>A collection of methods</returns>
        public IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Where(s => !s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Where(s => !s.IsGenericMethod)
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray();
        }

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter parameter if needed</param>
        /// <returns>A collection of generic methods</returns>
        public IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Where(s => s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(G9CTypeHandler.CreateCustomModifier(specifiedModifiers))
                    .Where(s => s.IsGenericMethod)
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray();
        }


        /// <summary>
        ///     Method to get all members of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic members</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilterForFields">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForProperties">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterMethods">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForGenericMethods">Specifies a custom filter parameter if needed</param>
        /// <returns>An object with members array</returns>
        public G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null)
        {
            return new G9DtObjectMembers(
                GetFieldsOfObject(targetObject, specifiedModifiers, customFilterForFields),
                GetPropertiesOfObject(targetObject, specifiedModifiers, customFilterForProperties),
                GetMethodsOfObject(targetObject, specifiedModifiers, customFilterMethods),
                GetGenericMethodsOfObject(targetObject, specifiedModifiers, customFilterForGenericMethods)
            );
        }
    }
}