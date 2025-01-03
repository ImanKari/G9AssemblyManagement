using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.Core
{
    /// <summary>
    ///     Helper class for objects and reflections
    /// </summary>
    internal static class G9CReflectionHandler
    {
        #region merging Methods

        /// <summary>
        ///     Method to merge the values between two objects.
        ///     <para />
        ///     The first object gets its new values from the second object.
        /// </summary>
        /// <param name="mainObject">Specifies the main object for getting new values from the target object.</param>
        /// <param name="targetObject">Specifies the target object for giving its values to the main object.</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="valueMismatch">Specifies the mismatch checking process for members' values</param>
        /// <param name="enableTryToChangeType">
        ///     Specifies that if a mismatch occurs between two members' values, an automatic try
        ///     to change type must happen or not.
        /// </param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="customProcess">
        ///     Specifies a custom process for desired members if needed.
        ///     <para />
        ///     Notice: The function's result specifies whether the custom process handled merging or not.
        ///     <para />
        ///     If it's returned 'true.' Specifies that the custom process has done the merging process, and the core mustn't do
        ///     anything.
        ///     <para />
        ///     If it's returned 'false.' Specifies that the custom process skipped the merging process, So the core must do it.
        /// </param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        public static void MergeObjectsValues<TTypeMain, TTypeTarget>(ref TTypeMain mainObject,
            TTypeTarget targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public,
            G9EValueMismatchChecking valueMismatch = G9EValueMismatchChecking.AllowMismatchValues,
            bool enableTryToChangeType = false,
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, bool> customProcess = null, bool considerInheritedParent = false)
        {
            var totalMembers = (customFilter != null
                    // By custom filter
                    ? GetFieldsOfObject(mainObject, specifiedModifiers,
                            considerInheritedParent: considerInheritedParent).Where(s => customFilter(s))
                        .Select(s => (G9IMember)s)
                        .Concat(GetPropertiesOfObject(mainObject, specifiedModifiers).Where(s => customFilter(s))
                            .Select(s => (G9IMember)s))
                        .Join(
                            GetFieldsOfObject(targetObject, specifiedModifiers,
                                    considerInheritedParent: considerInheritedParent).Where(s => customFilter(s))
                                .Select(s => (G9IMember)s)
                                .Concat(GetPropertiesOfObject(targetObject, specifiedModifiers)
                                    .Where(s => customFilter(s))
                                    .Select(s => (G9IMember)s)).ToArray(), x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IMember>(m1, m2)
                        ).ToArray()

                    // Without custom filter
                    : GetFieldsOfObject(mainObject, specifiedModifiers,
                            considerInheritedParent: considerInheritedParent).Select(s => (G9IMember)s)
                        .Concat(GetPropertiesOfObject(mainObject, specifiedModifiers).Select(s => (G9IMember)s))
                        .Join(
                            GetFieldsOfObject(targetObject, specifiedModifiers,
                                    considerInheritedParent: considerInheritedParent).Select(s => (G9IMember)s)
                                .Concat(GetPropertiesOfObject(targetObject, specifiedModifiers)
                                    .Select(s => (G9IMember)s))
                            , x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IMember>(m1, m2)
                        )
                )
                .ToArray();


            var ignoreException = valueMismatch == G9EValueMismatchChecking.AllowMismatchValues;

            // Merging members
            foreach (var member in totalMembers)
                TryToSetValueBetweenTwoMember(member.Item1, member.Item2, enableTryToChangeType, ignoreException,
                    customProcess);
        }

        /// <summary>
        ///     Method to try to merge the value between two members.
        ///     If an exception is thrown in the merging process, this method makes a readable exception.
        /// </summary>
        /// <param name="memberA">Specifies an object's member for setting the new value (Main object).</param>
        /// <param name="memberB">Specifies an object's member for getting the new value (Target object).</param>
        /// <param name="enableTryToChangeType">
        ///     Specifies that if a mismatch occurs between two members' values, an automatic try
        ///     to change type must happen or not.
        /// </param>
        /// <param name="ignoreException">
        ///     If this parameter is set 'true,' and an exception is thrown in the merging process, this method ignores the
        ///     current member merging process and continues.
        /// </param>
        /// <param name="customProcess">
        ///     Specifies a custom process for desired members if needed.
        ///     <para />
        ///     Notice: The function's result specifies whether the custom process handled merging or not.
        ///     <para />
        ///     If it's returned 'true.' Specifies that the custom process has done the merging process, and the core mustn't do
        ///     anything.
        ///     <para />
        ///     If it's returned 'false.' Specifies that the custom process skipped the merging process, So the core must do it.
        /// </param>
        private static void TryToSetValueBetweenTwoMember(G9IMember memberA, G9IMember memberB,
            bool enableTryToChangeType, bool ignoreException,
            Func<G9IMember, G9IMember, bool> customProcess)
        {
            var useCustomProcess = false;
            try
            {
                if (customProcess != null)
                {
                    useCustomProcess = true;
                    if (customProcess(memberA, memberB)) return;
                    useCustomProcess = false;
                    memberA.SetValue(memberB.GetValue());
                }
                else
                {
                    memberA.SetValue(memberB.GetValue());
                }
            }
            catch (Exception ex1)
            {
                if (useCustomProcess && ignoreException)
                    return;
                if (useCustomProcess)
                    throw new Exception(
                        $@"The specified custom process for merging members' values threw an unhandled exception.
In the first object, the member's name is '{memberA.Name}' with the type '{memberA.MemberType}'.
In the second object, the member's name is '{memberB.Name}' with the value '{memberB.GetValue()}' and the type '{memberB.MemberType}'.",
                        ex1);

                if (enableTryToChangeType)
                    try
                    {
                        memberA.SetValue(G9Assembly.TypeTools.SmartChangeType(memberB.GetValue(),
                            memberA is G9DtField fields
                                ? fields.FieldInfo.FieldType
                                : ((G9DtProperty)memberA).PropertyInfo.PropertyType));
                    }
                    catch (Exception ex2)
                    {
                        if (!ignoreException)
                            throw new Exception($@"The members can't merge their values.
In the first object, the member's name is '{memberA.Name}' with the type '{memberA.MemberType}'.
In the second object, the member's name is '{memberB.Name}' with the value '{memberB.GetValue()}' and the type '{memberB.MemberType}'.",
                                ex2);
                    }
                else if (!ignoreException)
                    throw new Exception($@"The members can't merge their values.
In the first object, the member's name is '{memberA.Name}' with the type '{memberA.MemberType}'.
In the second object, the member's name is '{memberB.Name}' with the value '{memberB.GetValue()}' and the type '{memberB.MemberType}'.",
                        ex1);
            }
        }

        #endregion

        #region Comparasion Methods

        /// <summary>
        ///     Method to compare the values between two objects.
        /// </summary>
        /// <param name="firstObject">Specifies the first object for comparison.</param>
        /// <param name="secondObject">Specifies the second object for comparison.</param>
        /// <param name="unequalMembers">
        ///     Specifies a collection of unequal members if the compare method result would be 'false.'
        ///     <para />
        ///     Otherwise, It's an empty collection.
        /// </param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process.</param>
        /// <param name="enableTryToChangeType">
        ///     Specifies that if a mismatch occurs between two members' values, an automatic try
        ///     to change type must happen or not.
        /// </param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="customProcess">
        ///     Specifies a custom process for desired members if needed.
        ///     <para />
        ///     Notice: The function's result specifies whether the custom process handled merging or not.
        ///     <para />
        ///     If it's returned 'Equal' or 'Nonequal,' it Specifies that the custom process has done the comparing process, and
        ///     the core mustn't do anything.
        ///     <para />
        ///     If 'Skip' is returned, it specifies that the custom process skipped the comparing process, So the core must do it.
        /// </param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>
        ///     Specifies that specified objects are equal (true) or not (false).
        ///     <para />
        ///     If the returned result is 'false,' the unequal members are specified in parameter 'unequalMembers.'
        /// </returns>
        public static bool CompareObjectsValues(object firstObject, object secondObject,
            out IList<G9DtTuple<G9IMember>> unequalMembers,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public,
            bool enableTryToChangeType = false,
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, G9EComparisonResult> customProcess = null, bool considerInheritedParent = false)
        {
            var totalMembers = (customFilter != null
                    // By custom filter
                    ? GetFieldsOfObject(firstObject, specifiedModifiers,
                            considerInheritedParent: considerInheritedParent).Where(s => customFilter(s))
                        .Select(s => (G9IMember)s)
                        .Concat(GetPropertiesOfObject(firstObject, specifiedModifiers).Where(s => customFilter(s))
                            .Select(s => (G9IMember)s))
                        .Join(
                            GetFieldsOfObject(secondObject, specifiedModifiers,
                                    considerInheritedParent: considerInheritedParent).Where(s => customFilter(s))
                                .Select(s => (G9IMember)s)
                                .Concat(GetPropertiesOfObject(secondObject, specifiedModifiers)
                                    .Where(s => customFilter(s))
                                    .Select(s => (G9IMember)s)).ToArray(), x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IMember>(m1, m2)
                        ).ToArray()

                    // Without custom filter
                    : GetFieldsOfObject(firstObject, specifiedModifiers,
                            considerInheritedParent: considerInheritedParent).Select(s => (G9IMember)s)
                        .Concat(GetPropertiesOfObject(firstObject, specifiedModifiers).Select(s => (G9IMember)s))
                        .Join(
                            GetFieldsOfObject(secondObject, specifiedModifiers,
                                    considerInheritedParent: considerInheritedParent).Select(s => (G9IMember)s)
                                .Concat(GetPropertiesOfObject(secondObject, specifiedModifiers)
                                    .Select(s => (G9IMember)s))
                            , x => x.Name, s => s.Name, (m1, m2) =>
                                new G9DtTuple<G9IMember>(m1, m2)
                        )
                )
                .ToArray();


            // Compare members
            unequalMembers = new List<G9DtTuple<G9IMember>>();
            foreach (var member in totalMembers)
                if (CompareValueBetweenTwoMembers(member.Item1, member.Item2, enableTryToChangeType, customProcess) ==
                    G9EComparisonResult.Nonequal)
                    unequalMembers.Add(member);

            return !unequalMembers.Any();
        }

        /// <summary>
        ///     Helper method for comparison
        /// </summary>
        private static G9EComparisonResult CompareValueBetweenTwoMembers(G9IMember memberA, G9IMember memberB,
            bool enableTryToChangeType, Func<G9IMember, G9IMember, G9EComparisonResult> customProcess)
        {
            if (customProcess != null) return customProcess(memberA, memberB);

            if (Equals(memberA.GetValue(), memberB.GetValue()))
                return G9EComparisonResult.Equal;
            if (!enableTryToChangeType) return G9EComparisonResult.Nonequal;
            try
            {
                if (Equals(memberA.GetValue(),
                        G9Assembly.TypeTools.SmartChangeType(memberB.GetValue(), memberA.MemberType)))
                    return G9EComparisonResult.Equal;
            }
            catch
            {
                // Ignore
            }

            return G9EComparisonResult.Nonequal;
        }

        #endregion

        #region Get Fields

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of fields</returns>
        public static IList<G9DtField> GetFieldsOfObject(object targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return GetFieldsOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject, false,
                considerInheritedParent);
        }

        /// <summary>
        ///     Method to get fields of a type
        /// </summary>
        /// <param name="targetType">Specifies a type to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching type's members if needed.</param>
        /// <param name="targetObject">
        ///     Specifies a created object for assigning to members for getting and setting its values.
        ///     <para />
        ///     Notice: Without assigning an instance, you can't use members (for getting or setting value) unless they are static
        ///     type members.
        /// </param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of fields</returns>
        public static IList<G9DtField> GetFieldsOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
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

            if (considerInheritedParent)
            {
                var collection = new List<G9DtField>();
                var tempType = targetType;
                while (tempType != null && tempType != typeof(object))
                {
                    collection.AddRange(
                        customFilter != null
                            ? tempType
                                .GetFields(specifiedModifiers)
                                .Where(customFilter)
                                .Select(s => new G9DtField(s.Name, s, targetObject)).ToArray()
                            : tempType
                                .GetFields(specifiedModifiers)
                                .Select(s => new G9DtField(s.Name, s, targetObject)).ToArray()
                    );
                    tempType = tempType.BaseType;
                }

                return collection;
            }

            return customFilter != null
                ? targetType
                    .GetFields(specifiedModifiers)
                    .Where(customFilter)
                    .Select(s => new G9DtField(s.Name, s, targetObject)).ToArray()
                : targetType
                    .GetFields(specifiedModifiers)
                    .Select(s => new G9DtField(s.Name, s, targetObject)).ToArray();
        }

        #endregion

        #region Get Properties

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of properties</returns>
        public static IList<G9DtProperty> GetPropertiesOfObject(object targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<PropertyInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return GetPropertiesOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject, false,
                considerInheritedParent);
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
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of properties</returns>
        public static IList<G9DtProperty> GetPropertiesOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<PropertyInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
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

            if (considerInheritedParent)
            {
                var collection = new List<G9DtProperty>();
                var tempType = targetType;
                while (tempType != null && tempType != typeof(object))
                {
                    collection.AddRange(
                        customFilter != null
                            ? tempType
                                .GetProperties(specifiedModifiers)
                                .Where(customFilter)
                                .Select(s => new G9DtProperty(s.Name, s, targetObject))
                                .ToArray()
                            : tempType
                                .GetProperties(specifiedModifiers)
                                .Select(s => new G9DtProperty(s.Name, s, targetObject))
                                .ToArray()
                    );
                    tempType = tempType.BaseType;
                }

                return collection;
            }

            return customFilter != null
                ? targetType
                    .GetProperties(specifiedModifiers)
                    .Where(customFilter)
                    .Select(s => new G9DtProperty(s.Name, s, targetObject))
                    .ToArray()
                : targetType
                    .GetProperties(specifiedModifiers)
                    .Select(s => new G9DtProperty(s.Name, s, targetObject))
                    .ToArray();
        }

        #endregion

        #region Get Methods

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of methods</returns>
        public static IList<G9DtMethod> GetMethodsOfObject(object targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return GetMethodsOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject, false,
                considerInheritedParent);
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
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of methods</returns>
        public static IList<G9DtMethod> GetMethodsOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
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

            if (considerInheritedParent)
            {
                var collection = new List<G9DtMethod>();
                var tempType = targetType;
                while (tempType != null && tempType != typeof(object))
                {
                    collection.AddRange(
                        customFilter != null
                            ? tempType
                                .GetMethods(specifiedModifiers)
                                .Where(s => !s.IsGenericMethod && customFilter(s))
                                .Select(s => new G9DtMethod(s.Name, s, targetObject))
                                .ToArray()
                            : tempType
                                .GetMethods(specifiedModifiers)
                                .Where(s => !s.IsGenericMethod)
                                .Select(s => new G9DtMethod(s.Name, s, targetObject))
                                .ToArray()
                    );
                    tempType = tempType.BaseType;
                }

                return collection;
            }

            return customFilter != null
                ? targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => !s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtMethod(s.Name, s, targetObject))
                    .ToArray()
                : targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => !s.IsGenericMethod)
                    .Select(s => new G9DtMethod(s.Name, s, targetObject))
                    .ToArray();
        }

        #endregion

        #region Get Generic Methods

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of generic methods</returns>
        public static IList<G9DtGenericMethod> GetGenericMethodsOfObject(object targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return GetGenericMethodsOfType(targetObject.GetType(), specifiedModifiers, customFilter, targetObject,
                false,
                considerInheritedParent);
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
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>A collection of generic methods</returns>
        public static IList<G9DtGenericMethod> GetGenericMethodsOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<MethodInfo, bool> customFilter = null, object targetObject = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
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

            if (considerInheritedParent)
            {
                var collection = new List<G9DtGenericMethod>();
                var tempType = targetType;
                while (tempType != null && tempType != typeof(object))
                {
                    collection.AddRange(
                        customFilter != null
                            ? tempType
                                .GetMethods(specifiedModifiers)
                                .Where(s => s.IsGenericMethod && customFilter(s))
                                .Select(s => new G9DtGenericMethod(s.Name, s, targetObject))
                                .ToArray()
                            : tempType
                                .GetMethods(specifiedModifiers)
                                .Where(s => s.IsGenericMethod)
                                .Select(s => new G9DtGenericMethod(s.Name, s, targetObject))
                                .ToArray()
                    );
                    tempType = tempType.BaseType;
                }

                return collection;
            }

            return customFilter != null
                ? targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => s.IsGenericMethod && customFilter(s))
                    .Select(s => new G9DtGenericMethod(s.Name, s, targetObject))
                    .ToArray()
                : targetType
                    .GetMethods(specifiedModifiers)
                    .Where(s => s.IsGenericMethod)
                    .Select(s => new G9DtGenericMethod(s.Name, s, targetObject))
                    .ToArray();
        }

        #endregion

        #region Get All Members Methods

        /// <summary>
        ///     Method to get all members of an object
        /// </summary>
        /// <param name="targetObject">Specifies an object to find generic members</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilterForFields">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForProperties">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterMethods">Specifies a custom filter parameter if needed</param>
        /// <param name="customFilterForGenericMethods">Specifies a custom filter parameter if needed</param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>An object with members array</returns>
        public static G9DtMember GetAllMembersOfObject(object targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null, bool considerInheritedParent = false)
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
        /// <param name="targetObject">Specifies a type for set and get value automatically.</param>
        /// <param name="initializeInstance">
        ///     If it's set 'true,' the method initializes an instance from the type that leads to access to some members who need
        ///     an instance to use (like non-abstract and non-sealed members).
        ///     <para />
        ///     If the specified type is kind of some type that can't be initialized (like abstract and sealed), an exception is
        ///     thrown.
        /// </param>
        /// <param name="considerInheritedParent">
        ///     Specifies whether the searching process must consider the inherited parent (if it exists) or not.
        ///     <para />
        ///     If it's set 'true.' All inherited parents will check, and their members will be got.
        /// </param>
        /// <returns>An object with members array</returns>
        public static G9DtMember GetAllMembersOfType(Type targetType,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null, object targetObject = null,
            bool initializeInstance = false, bool considerInheritedParent = false)
        {
            return new G9DtMember(
                GetFieldsOfType(targetType, specifiedModifiers, customFilterForFields, targetObject,
                    initializeInstance, considerInheritedParent),
                GetPropertiesOfType(targetType, specifiedModifiers, customFilterForProperties, targetObject,
                    initializeInstance, considerInheritedParent),
                GetMethodsOfType(targetType, specifiedModifiers, customFilterMethods, targetObject, initializeInstance,
                    considerInheritedParent),
                GetGenericMethodsOfType(targetType, specifiedModifiers, customFilterForGenericMethods, targetObject,
                    initializeInstance, considerInheritedParent)
            );
        }

        #endregion

        #region GetAttributes Methods

        /// <summary>
        ///     Method to get specified attribute from an object
        /// </summary>
        /// <typeparam name="TAttr">Specifies the type of an attribute</typeparam>
        /// <param name="target">Specifies a target object for getting specified attributes</param>
        /// <param name="memberName">Specifies the member in object that you want </param>
        /// <param name="inherit">
        ///     true to search this member's inheritance chain to find the attributes; otherwise, false. This
        ///     parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <returns>An attribute object if that existed, otherwise return null</returns>
        public static TAttr[] GetCustomAttributes<TAttr>(object target, string memberName, bool inherit)
            where TAttr : Attribute
        {
            var res = GetCustomAttributes(typeof(TAttr), target, memberName, inherit);
            return res.Length > 0
                    ? res.Select(s => (TAttr)s).ToArray()
                    :
#if NET8_0_OR_GREATER
                    []
#else
                new TAttr[0]
#endif
                ;
        }

        /// <summary>
        ///     Method to get specified attribute from an object
        /// </summary>
        /// <typeparam name="TAttr">Specifies the type of an attribute</typeparam>
        /// <typeparam name="TObject">Specifies the type of target object</typeparam>
        /// <param name="target">Specifies a target object for getting specified attributes</param>
        /// <param name="selectMemberExpression">An expression for selecting a member</param>
        /// <param name="inherit">
        ///     true to search this member's inheritance chain to find the attributes; otherwise, false. This
        ///     parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <returns>An attribute object if that existed, otherwise return null</returns>
        public static TAttr[] GetCustomAttributes<TAttr, TObject>(TObject target,
            Expression<Func<TObject, object>> selectMemberExpression, bool inherit)
            where TAttr : Attribute
        {
            // Extract the member info from the expression
            var memberExpression = GetMemberExpression(selectMemberExpression);
            if (memberExpression == null)
                throw new ArgumentException("The expression is not a member access expression.",
                    nameof(selectMemberExpression));

            // Get the member name
            var memberName = memberExpression.Member.Name;

            var res = GetCustomAttributes(typeof(TAttr), target, memberName, inherit);
            return res.Length > 0
                    ? res.Select(s => (TAttr)s).ToArray()
                    :
#if NET8_0_OR_GREATER
                    []
#else
                new TAttr[0]
#endif
                ;
        }

        /// <summary>
        ///     Method to get specified attribute from an object
        /// </summary>
        /// <param name="typeOfAttribute">Specifies the type of an attribute for searching and getting</param>
        /// <param name="target">Specifies a target object for getting specified attributes</param>
        /// <param name="selectMemberExpression">An expression for selecting a member</param>
        /// <param name="inherit">
        ///     true to search this member's inheritance chain to find the attributes; otherwise, false. This
        ///     parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <returns>An attribute object if that existed, otherwise return null</returns>
        public static object[] GetCustomAttributes<TObject>(Type typeOfAttribute, TObject target,
            Expression<Func<TObject, object>> selectMemberExpression, bool inherit)
        {
            // Extract the member info from the expression
            var memberExpression = GetMemberExpression(selectMemberExpression);
            if (memberExpression == null)
                throw new ArgumentException("The expression is not a member access expression.",
                    nameof(selectMemberExpression));

            // Get the member name
            var memberName = memberExpression.Member.Name;

            // Get custom attributes of the member
            var res = GetCustomAttributes(typeOfAttribute, target, memberName, inherit);
            return res.Length > 0
                    ? res
                    :
#if NET8_0_OR_GREATER
                    []
#else
                    new object[0]
#endif
                ;
        }

        /// <summary>
        ///     Method to get specified attribute from an object
        /// </summary>
        /// <param name="typeOfAttribute">Specifies the type of an attribute for searching and getting</param>
        /// ///
        /// <param name="target">Specifies a target object for getting specified attributes</param>
        /// <param name="memberName">Specifies the member in object that you want </param>
        /// <param name="inherit">
        ///     true to search this member's inheritance chain to find the attributes; otherwise, false. This
        ///     parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <returns>An attribute object if that existed, otherwise return null</returns>
        public static object[] GetCustomAttributes(Type typeOfAttribute, object target, string memberName, bool inherit)
        {
            var member = target.GetType().GetMember(memberName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            // ReSharper disable once UseCollectionExpression
            return member.Length == 0
                ? new object[0]
                : member.SelectMany(s => s.GetCustomAttributes(typeOfAttribute, inherit)).ToArray();
        }

        /// <summary>
        ///     Get member Expression
        /// </summary>
        private static MemberExpression GetMemberExpression<TObject>(Expression<Func<TObject, object>> expression)
        {
            if (expression.Body is MemberExpression memberExpression) return memberExpression;

            if (expression.Body is UnaryExpression unaryExpression &&
                unaryExpression.Operand is MemberExpression operand) return operand;

            return null;
        }

        #endregion
    }
}