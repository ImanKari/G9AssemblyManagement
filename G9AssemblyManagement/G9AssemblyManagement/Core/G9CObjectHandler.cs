using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.Core
{
    internal static class G9CObjectHandler
    {
        /// <summary>
        ///     Method to create custom modifier
        /// </summary>
        /// <param name="customModifier">Specifies custom modifiers are to be included in the search.</param>
        /// <returns>Return a custom BindingFlags object</returns>
        public static BindingFlags CreateCustomModifier(
            G9EAccessModifier customModifier = G9EAccessModifier.Everything)
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

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of fields</returns>
        public static IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetFields(CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray()
                : targetObject.GetType()
                    .GetFields(CreateCustomModifier(specifiedModifiers))
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray();
        }

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of fields</returns>
        public static IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetFields(specifiedModifiers)
                    .Where(customFilter)
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray()
                : targetObject.GetType()
                    .GetFields(specifiedModifiers)
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray();
        }

        /// <summary>
        ///     Method to get the number of fields in an object.
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>The number of fields</returns>
        public static int GetNumberOfFieldsInObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetFields(CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Count()
                : targetObject.GetType()
                    .GetFields(CreateCustomModifier(specifiedModifiers))
                    .Length;
        }

        /// <summary>
        ///     Method to get the number of fields in an object.
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>The number of fields</returns>
        public static int
            GetNumberOfFieldsInObject<TObject>(TObject targetObject,
                BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                                  BindingFlags.Static,
                Func<FieldInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetFields(specifiedModifiers)
                    .Where(customFilter)
                    .Count()
                : targetObject.GetType()
                    .GetFields(specifiedModifiers)
                    .Length;
        }


        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of properties</returns>
        public static IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<PropertyInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetProperties(CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetProperties(CreateCustomModifier(specifiedModifiers))
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray();
        }

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of properties</returns>
        public static IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<PropertyInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetProperties(specifiedModifiers)
                    .Where(customFilter)
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetProperties(specifiedModifiers)
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray();
        }

        /// <summary>
        ///     Method to get the number of properties in an object.
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>The number of properties</returns>
        public static int GetNumberOfPropertiesInObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<PropertyInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetProperties(CreateCustomModifier(specifiedModifiers))
                    .Where(customFilter)
                    .Count()
                : targetObject.GetType()
                    .GetProperties(CreateCustomModifier(specifiedModifiers))
                    .Length;
        }

        /// <summary>
        ///     Method to get the number of properties in an object.
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>The number of properties</returns>
        public static int GetNumberOfPropertiesInObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<PropertyInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetProperties(specifiedModifiers)
                    .Where(customFilter)
                    .Count()
                : targetObject.GetType()
                    .GetProperties(specifiedModifiers)
                    .Length;
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of methods</returns>
        public static IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(CreateCustomModifier(specifiedModifiers))
                    .Where(s => !s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(CreateCustomModifier(specifiedModifiers))
                    .Where(s => !s.IsGenericMethod)
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray();
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of methods</returns>
        public static IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(specifiedModifiers)
                    .Where(s => !s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(specifiedModifiers)
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
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of generic methods</returns>
        public static IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(CreateCustomModifier(specifiedModifiers))
                    .Where(s => s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(CreateCustomModifier(specifiedModifiers))
                    .Where(s => s.IsGenericMethod)
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray();
        }

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of generic methods</returns>
        public static IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null)
        {
            return customFilter != null
                ? targetObject.GetType()
                    .GetMethods(specifiedModifiers)
                    .Where(s => s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetObject.GetType()
                    .GetMethods(specifiedModifiers)
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
        public static G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject,
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
        public static G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
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

        /// <summary>
        ///     Method to unify the values between two objects.
        ///     <para />
        ///     The first object gets its new values from the second object.
        /// </summary>
        /// <param name="mainObject">Specifies the main object for getting new values from the target object.</param>
        /// <param name="targetObject">Specifies the target object for giving its values to the main object.</param>
        /// <param name="valueMismatch">Specifies the mismatch checking process for members' values</param>
        /// <param name="enableTryToChangeType">
        ///     Specifies that if a mismatch occurs between two members' values, an automatic try
        ///     to change type must happen or not.
        /// </param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="customProcess">
        ///     Specifies a custom process for each member if needed.
        ///     <para />
        ///     Notice: In fact, the function's result specifies the member's value in the main object.
        /// </param>
        public static void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch = G9EValueMismatchChecking.AllowMismatchValues,
            bool enableTryToChangeType = false, G9EAccessModifier specifiedModifiers = G9EAccessModifier.Public,
            Func<G9IObjectMember, bool> customFilter = null,
            Func<G9IObjectMember, G9IObjectMember, object> customProcess = null)
        {
            UnifyObjectsValues(mainObject, targetObject, valueMismatch, enableTryToChangeType,
                CreateCustomModifier(specifiedModifiers), customFilter, customProcess);
        }

        /// <summary>
        ///     Method to unify the values between two objects.
        ///     <para />
        ///     The first object gets its new values from the second object.
        /// </summary>
        /// <param name="mainObject">Specifies the main object for getting new values from the target object.</param>
        /// <param name="targetObject">Specifies the target object for giving its values to the main object.</param>
        /// <param name="valueMismatch">Specifies the mismatch checking process for members' values</param>
        /// <param name="enableTryToChangeType">
        ///     Specifies that if a mismatch occurs between two members' values, an automatic try
        ///     to change type must happen or not.
        /// </param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="customProcess">
        ///     Specifies a custom process for each member if needed.
        ///     <para />
        ///     Notice: In fact, the function's result specifies the member's value in the main object.
        /// </param>
        public static void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch = G9EValueMismatchChecking.AllowMismatchValues,
            bool enableTryToChangeType = false,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public,
            Func<G9IObjectMember, bool> customFilter = null,
            Func<G9IObjectMember, G9IObjectMember, object> customProcess = null)
        {
            var totalMembers = (customFilter != null
                    // By custom filter
                    ? GetFieldsOfObject(mainObject, specifiedModifiers).Where(s => customFilter(s))
                        .Select(s => (G9IObjectMember)s)
                        .Concat(GetPropertiesOfObject(mainObject, specifiedModifiers).Where(s => customFilter(s))
                            .Select(s => (G9IObjectMember)s))
                        .Join(
                            GetFieldsOfObject(targetObject, specifiedModifiers).Where(s => customFilter(s))
                                .Select(s => (G9IObjectMember)s)
                                .Concat(GetPropertiesOfObject(targetObject, specifiedModifiers)
                                    .Where(s => customFilter(s))
                                    .Select(s => (G9IObjectMember)s)).ToArray(), x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IObjectMember>(m1, m2)
                        ).ToArray()

                    // Without custom filter
                    : GetFieldsOfObject(mainObject, specifiedModifiers).Select(s => (G9IObjectMember)s)
                        .Concat(GetPropertiesOfObject(mainObject, specifiedModifiers).Select(s => (G9IObjectMember)s))
                        .Join(
                            GetFieldsOfObject(targetObject, specifiedModifiers).Select(s => (G9IObjectMember)s)
                                .Concat(GetPropertiesOfObject(targetObject, specifiedModifiers)
                                    .Select(s => (G9IObjectMember)s))
                            , x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IObjectMember>(m1, m2)
                        )
                )
                .ToArray();


            var ignoreException = valueMismatch == G9EValueMismatchChecking.AllowMismatchValues;

            // Unifying members
            foreach (var member in totalMembers)
                TryToSetValueBetweenTwoMember(member.Item1, member.Item2, enableTryToChangeType, ignoreException,
                    customProcess);
        }

        /// <summary>
        ///     Method to try to unify the value between two members.
        ///     If an exception is thrown in the unifying process, this method makes a readable exception.
        /// </summary>
        /// <param name="memberA">Specifies an object's member for setting the new value (Main object).</param>
        /// <param name="memberB">Specifies an object's member for getting the new value (Target object).</param>
        /// <param name="enableTryToChangeType">
        ///     Specifies that if a mismatch occurs between two members' values, an automatic try
        ///     to change type must happen or not.
        /// </param>
        /// <param name="ignoreException">
        ///     If this parameter is set 'true,' and an exception is thrown in the unifying process, this method ignores the
        ///     current member unifying process and continues.
        /// </param>
        /// <param name="customProcess">
        ///     Specifies a custom process for each member if needed.
        ///     <para />
        ///     Notice: In fact, the function's result specifies the member's value in the main object.
        /// </param>
        private static void TryToSetValueBetweenTwoMember(G9IObjectMember memberA, G9IObjectMember memberB,
            bool enableTryToChangeType, bool ignoreException,
            Func<G9IObjectMember, G9IObjectMember, object> customProcess)
        {
            try
            {
                memberA.SetValue(customProcess == null
                    ? memberB.GetValue()
                    : customProcess(memberA, memberB));
            }
            catch (Exception ex1)
            {
                if (enableTryToChangeType)
                    try
                    {
                        memberA.SetValue(G9Assembly.TypeTools.SmartChangeType(memberB.GetValue(),
                            memberA is G9DtFields fields
                                ? fields.FieldInfo.FieldType
                                : ((G9DtProperties)memberA).PropertyInfo.PropertyType));
                    }
                    catch (Exception ex2)
                    {
                        if (!ignoreException)
                            throw new Exception($@"The members can't unify their values.
In the first object, the member's name is '{memberA.Name}' with the type '{(memberA is G9DtFields result ? result.FieldInfo.FieldType : ((G9DtProperties)memberA).PropertyInfo.PropertyType)}'.
In the second object, the member's name is '{memberB.Name}' with the value '{memberB.GetValue()}' and the type '{(memberB is G9DtFields result2 ? result2.FieldInfo.FieldType : ((G9DtProperties)memberB).PropertyInfo.PropertyType)}'.",
                                ex2);
                    }
                else if (!ignoreException)
                    throw new Exception($@"The members can't unify their values.
In the first object, the member's name is '{memberA.Name}' with the type '{(memberA is G9DtFields result ? result.FieldInfo.FieldType : ((G9DtProperties)memberA).PropertyInfo.PropertyType)}'.
In the second object, the member's name is '{memberB.Name}' with the value '{memberB.GetValue()}' and the type '{(memberB is G9DtFields result2 ? result2.FieldInfo.FieldType : ((G9DtProperties)memberB).PropertyInfo.PropertyType)}'.",
                        ex1);
            }
        }
    }
}