using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public IList<G9DtFields> G9GetFieldsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilter = null)
            where TObject : new()
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetFields(G9CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray()
                : targetObject.GetType()
                    .GetFields(G9CreateCustomModifier(specifiedModifiers))
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
        public IList<G9DtProperties> G9GetPropertiesOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<PropertyInfo, bool> customFilter = null)
            where TObject : new()
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetProperties(G9CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetProperties(G9CreateCustomModifier(specifiedModifiers))
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
        public IList<G9DtMethods> G9GetMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null)
            where TObject : new()
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(G9CreateCustomModifier(specifiedModifiers))
                    .Where(s => !s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(G9CreateCustomModifier(specifiedModifiers))
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
        public IList<G9DtGenericMethods> G9GetGenericMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null)
            where TObject : new()
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(G9CreateCustomModifier(specifiedModifiers))
                    .Where(s => s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(G9CreateCustomModifier(specifiedModifiers))
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
        public G9DtObjectMembers G9GetAllMembersOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null)
            where TObject : new()
        {
            return new G9DtObjectMembers(
                G9GetFieldsOfObject(targetObject, specifiedModifiers, customFilterForFields),
                G9GetPropertiesOfObject(targetObject, specifiedModifiers, customFilterForProperties),
                G9GetMethodsOfObject(targetObject, specifiedModifiers, customFilterMethods),
                G9GetGenericMethodsOfObject(targetObject, specifiedModifiers, customFilterForGenericMethods)
            );
        }

        /// <summary>
        ///     Method to create custom modifier
        /// </summary>
        /// <param name="customModifier">Specifies custom modifiers are to be included in the search.</param>
        /// <returns>Return a custom BindingFlags object</returns>
        private BindingFlags G9CreateCustomModifier(G9EAccessModifier customModifier = G9EAccessModifier.Everything)
        {
            if (customModifier == G9EAccessModifier.Everything)
                return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            var defaultBindingFlags = (customModifier & G9EAccessModifier.Static) == G9EAccessModifier.Static
                ? BindingFlags.Static
                : BindingFlags.Instance;

            if ((customModifier & G9EAccessModifier.Public) == G9EAccessModifier.Public)
                defaultBindingFlags |= BindingFlags.Public;
            if ((customModifier & G9EAccessModifier.NonPublic) == G9EAccessModifier.NonPublic)
                defaultBindingFlags |= BindingFlags.NonPublic;

            return defaultBindingFlags;
        }
    }
}