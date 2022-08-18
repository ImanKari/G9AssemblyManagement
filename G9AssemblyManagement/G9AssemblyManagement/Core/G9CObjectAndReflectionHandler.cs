using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.Core
{
    /// <summary>
    ///     Helper class for objects and reflections
    /// </summary>
    internal static class G9CObjectAndReflectionHandler
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

        #region Unifying Methods

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
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, object> customProcess = null)
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
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, object> customProcess = null)
        {
            var totalMembers = (customFilter != null
                    // By custom filter
                    ? GetFieldsOfObject(mainObject, specifiedModifiers).Where(s => customFilter(s))
                        .Select(s => (G9IMember)s)
                        .Concat(GetPropertiesOfObject(mainObject, specifiedModifiers).Where(s => customFilter(s))
                            .Select(s => (G9IMember)s))
                        .Join(
                            GetFieldsOfObject(targetObject, specifiedModifiers).Where(s => customFilter(s))
                                .Select(s => (G9IMember)s)
                                .Concat(GetPropertiesOfObject(targetObject, specifiedModifiers)
                                    .Where(s => customFilter(s))
                                    .Select(s => (G9IMember)s)).ToArray(), x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IMember>(m1, m2)
                        ).ToArray()

                    // Without custom filter
                    : GetFieldsOfObject(mainObject, specifiedModifiers).Select(s => (G9IMember)s)
                        .Concat(GetPropertiesOfObject(mainObject, specifiedModifiers).Select(s => (G9IMember)s))
                        .Join(
                            GetFieldsOfObject(targetObject, specifiedModifiers).Select(s => (G9IMember)s)
                                .Concat(GetPropertiesOfObject(targetObject, specifiedModifiers)
                                    .Select(s => (G9IMember)s))
                            , x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IMember>(m1, m2)
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
        private static void TryToSetValueBetweenTwoMember(G9IMember memberA, G9IMember memberB,
            bool enableTryToChangeType, bool ignoreException,
            Func<G9IMember, G9IMember, object> customProcess)
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

        #endregion


        #region Get Fields

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
            return GetFieldsOfObject(targetObject, CreateCustomModifier(specifiedModifiers), customFilter);
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
            return GetFieldsOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject);
        }

        /// <summary>
        ///     Method to get fields of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of fields</returns>
        public static IList<G9DtFields> GetFieldsOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilter = null, bool initializeInstance = false)
        {
            return GetFieldsOfType(targetType, CreateCustomModifier(specifiedModifiers), customFilter, null,
                initializeInstance);
        }

        /// <summary>
        ///     Method to get fields of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="targetObject">Specifies an object for set and get value automatically.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of fields</returns>
        public static IList<G9DtFields> GetFieldsOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false)
        {
            if (initializeInstance)
                try
                {
                    targetObject = G9Assembly.InstanceTools.CreateInstanceFromType(targetType);
                }
                catch
                {
                    targetObject = G9Assembly.InstanceTools.CreateUninitializedInstanceFromType(targetType);
                }

            return customFilter != null
                ? targetType
                    .GetFields(specifiedModifiers)
                    .Where(customFilter)
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray()
                : targetType
                    .GetFields(specifiedModifiers)
                    .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray();
        }

        #endregion

        #region Get Properties

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
            return GetPropertiesOfObject(targetObject, CreateCustomModifier(specifiedModifiers), customFilter);
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
            return GetPropertiesOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject);
        }

        /// <summary>
        ///     Method to get properties of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of properties</returns>
        public static IList<G9DtProperties> GetPropertiesOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<PropertyInfo, bool> customFilter = null, bool initializeInstance = false)
        {
            return GetPropertiesOfType(targetType, CreateCustomModifier(specifiedModifiers), customFilter, null,
                initializeInstance);
        }

        /// <summary>
        ///     Method to get properties of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="targetObject">Specifies an object for set and get value automatically.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of properties</returns>
        public static IList<G9DtProperties> GetPropertiesOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<PropertyInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false)
        {
            if (initializeInstance)
                try
                {
                    targetObject = G9Assembly.InstanceTools.CreateInstanceFromType(targetType);
                }
                catch
                {
                    targetObject = G9Assembly.InstanceTools.CreateUninitializedInstanceFromType(targetType);
                }

            return customFilter != null
                ? targetType
                    .GetProperties(specifiedModifiers)
                    .Where(customFilter)
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray()
                : targetType
                    .GetProperties(specifiedModifiers)
                    .Select(s => new G9DtProperties(s.Name, s, targetObject))
                    .ToArray();
        }

        #endregion

        #region Get Methods

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
            return GetMethodsOfObject(targetObject, CreateCustomModifier(specifiedModifiers), customFilter);
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
            return GetMethodsOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject);
        }

        /// <summary>
        ///     Method to get Methods of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of methods</returns>
        public static IList<G9DtMethods> GetMethodsOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null, bool initializeInstance = false)
        {
            return GetMethodsOfType(targetType, CreateCustomModifier(specifiedModifiers), customFilter, null,
                initializeInstance);
        }

        /// <summary>
        ///     Method to get Methods of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="targetObject">Specifies an object for set and get value automatically.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of methods</returns>
        public static IList<G9DtMethods> GetMethodsOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false)
        {
            if (initializeInstance)
                try
                {
                    targetObject = G9Assembly.InstanceTools.CreateInstanceFromType(targetType);
                }
                catch
                {
                    targetObject = G9Assembly.InstanceTools.CreateUninitializedInstanceFromType(targetType);
                }

            return customFilter != null
                ? targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => !s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => !s.IsGenericMethod)
                    .Select(s => new G9DtMethods(s.Name, s, targetObject))
                    .ToArray();
        }

        #endregion

        #region Get Generic Methods

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
            return GetGenericMethodsOfObject(targetObject, CreateCustomModifier(specifiedModifiers), customFilter);
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
            return GetGenericMethodsOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject);
        }

        /// <summary>
        ///     Method to get generic methods of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of generic methods</returns>
        public static IList<G9DtGenericMethods> GetGenericMethodsOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null, bool initializeInstance = false)
        {
            return GetGenericMethodsOfType(targetType, CreateCustomModifier(specifiedModifiers), customFilter, null,
                initializeInstance);
        }

        /// <summary>
        ///     Method to get generic methods of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="targetObject">Specifies an object for set and get value automatically.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>A collection of generic methods</returns>
        public static IList<G9DtGenericMethods> GetGenericMethodsOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false)
        {
            if (initializeInstance)
                try
                {
                    targetObject = G9Assembly.InstanceTools.CreateInstanceFromType(targetType);
                }
                catch
                {
                    targetObject = G9Assembly.InstanceTools.CreateUninitializedInstanceFromType(targetType);
                }


            return customFilter != null
                ? targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray()
                : targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => s.IsGenericMethod)
                    .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                    .ToArray();
        }

        #endregion

        #region Get All Members Methods

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
        public static G9DtMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null)
        {
            return GetAllMembersOfObject(targetObject, CreateCustomModifier(specifiedModifiers), customFilterForFields,
                customFilterForProperties, customFilterMethods, customFilterForGenericMethods);
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
        public static G9DtMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null)
        {
            return GetAllMembersOfType(targetObject.GetType(), specifiedModifiers, customFilterForFields,
                customFilterForProperties, customFilterMethods, customFilterForGenericMethods, targetObject);
        }

        /// <summary>
        ///     Method to get all members of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find generic members</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilterForFields">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForProperties">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterMethods">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForGenericMethods">Specifies a custom filter parameter if needed</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>An object with members array</returns>
        public static G9DtMembers GetAllMembersOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null, bool initializeInstance = false)
        {
            return GetAllMembersOfType(targetType, CreateCustomModifier(specifiedModifiers), customFilterForFields,
                customFilterForProperties, customFilterMethods, customFilterForGenericMethods, null,
                initializeInstance);
        }

        /// <summary>
        ///     Method to get all members of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find generic members</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilterForFields">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForProperties">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterMethods">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForGenericMethods">Specifies a custom filter parameter if needed</param>
        /// <param name="targetObject">Specifies a type for set and get value automatically.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <returns>An object with members array</returns>
        public static G9DtMembers GetAllMembersOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null, object targetObject = null,
            bool initializeInstance = false)
        {
            return new G9DtMembers(
                GetFieldsOfType(targetType, specifiedModifiers, customFilterForFields, targetObject,
                    initializeInstance),
                GetPropertiesOfType(targetType, specifiedModifiers, customFilterForProperties, targetObject,
                    initializeInstance),
                GetMethodsOfType(targetType, specifiedModifiers, customFilterMethods, targetObject, initializeInstance),
                GetGenericMethodsOfType(targetType, specifiedModifiers, customFilterForGenericMethods, targetObject,
                    initializeInstance)
            );
        }

        #endregion
    }
}