using System;
using System.Collections.Generic;
using System.Reflection;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for reflections
    /// </summary>
    public class G9CObjectTools
    {
        #region GetProperties Methods

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <returns>A collection of properties</returns>
        public IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject)
        {
            return G9CObjectHandler.GetPropertiesOfObject(targetObject, G9EAccessModifier.Everything);
        }

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of properties</returns>
        public IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers)
        {
            return G9CObjectHandler.GetPropertiesOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of properties</returns>
        public IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers)
        {
            return G9CObjectHandler.GetPropertiesOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of properties</returns>
        public IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers,
            Func<PropertyInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetPropertiesOfObject(targetObject, specifiedModifiers, customFilter);
        }

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of properties</returns>
        public IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers,
            Func<PropertyInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetPropertiesOfObject(targetObject, specifiedModifiers, customFilter);
        }

        #endregion

        #region GetMethods Methods

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <returns>A collection of methods</returns>
        public IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject)
        {
            return G9CObjectHandler.GetMethodsOfObject(targetObject, G9EAccessModifier.Everything);
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of methods</returns>
        public IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers)
        {
            return G9CObjectHandler.GetMethodsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of methods</returns>
        public IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers)
        {
            return G9CObjectHandler.GetMethodsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of methods</returns>
        public IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers,
            Func<MethodInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetMethodsOfObject(targetObject, specifiedModifiers, customFilter);
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of methods</returns>
        public IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers,
            Func<MethodInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetMethodsOfObject(targetObject, specifiedModifiers, customFilter);
        }

        #endregion

        #region GetGenericMethodsOfObject Methods

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <returns>A collection of generic methods</returns>
        public IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject)
        {
            return G9CObjectHandler.GetGenericMethodsOfObject(targetObject, G9EAccessModifier.Everything);
        }

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of generic methods</returns>
        public IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers)
        {
            return G9CObjectHandler.GetGenericMethodsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of generic methods</returns>
        public IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers)
        {
            return G9CObjectHandler.GetGenericMethodsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of generic methods</returns>
        public IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers,
            Func<MethodInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetGenericMethodsOfObject(targetObject, specifiedModifiers, customFilter);
        }

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of generic methods</returns>
        public IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers,
            Func<MethodInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetGenericMethodsOfObject(targetObject, specifiedModifiers, customFilter);
        }

        #endregion


        #region GetAllMembersOfObject Methods

        /// <summary>
        ///     Method to get all members of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic members</param>
        /// <returns>An object with members array</returns>
        public G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject)
        {
            return G9CObjectHandler.GetAllMembersOfObject(targetObject, G9EAccessModifier.Everything);
        }

        /// <summary>
        ///     Method to get all members of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic members</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>An object with members array</returns>
        public G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers)
        {
            return G9CObjectHandler.GetAllMembersOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get all members of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic members</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>An object with members array</returns>
        public G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers)
        {
            return G9CObjectHandler.GetAllMembersOfObject(targetObject, specifiedModifiers);
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
            BindingFlags specifiedModifiers,
            Func<FieldInfo, bool> customFilterForFields,
            Func<PropertyInfo, bool> customFilterForProperties,
            Func<MethodInfo, bool> customFilterMethods,
            Func<MethodInfo, bool> customFilterForGenericMethods)
        {
            return G9CObjectHandler.GetAllMembersOfObject(targetObject, specifiedModifiers, customFilterForFields,
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
        public G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers,
            Func<FieldInfo, bool> customFilterForFields,
            Func<PropertyInfo, bool> customFilterForProperties,
            Func<MethodInfo, bool> customFilterMethods,
            Func<MethodInfo, bool> customFilterForGenericMethods)
        {
            return G9CObjectHandler.GetAllMembersOfObject(targetObject, specifiedModifiers, customFilterForFields,
                customFilterForProperties, customFilterMethods, customFilterForGenericMethods);
        }

        #endregion

        #region GetFields Methods

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <returns>A collection of fields</returns>
        public IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject)
        {
            return G9CObjectHandler.GetFieldsOfObject(targetObject, G9EAccessModifier.Everything);
        }

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of fields</returns>
        public IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers)
        {
            return G9CObjectHandler.GetFieldsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of fields</returns>
        public IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers)
        {
            return G9CObjectHandler.GetFieldsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of fields</returns>
        public IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers,
            Func<FieldInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetFieldsOfObject(targetObject, specifiedModifiers, customFilter);
        }

        /// <summary>
        ///     Method to get fields of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <param name="customFilter">Specifies a custom filter for searching object's members if needed.</param>
        /// <returns>A collection of fields</returns>
        public IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers,
            Func<FieldInfo, bool> customFilter)
        {
            return G9CObjectHandler.GetFieldsOfObject(targetObject, specifiedModifiers, customFilter);
        }

        #endregion

        #region UnifyObjectsValues Methods

        /// <summary>
        ///     Method to unify the values between two objects.
        ///     <para />
        ///     The first object gets its new values from the second object.
        /// </summary>
        /// <param name="mainObject">Specifies the main object for getting new values from the target object.</param>
        /// <param name="targetObject">Specifies the target object for giving its values to the main object.</param>
        public void UnifyObjectsValues(object mainObject, object targetObject)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, specifiedModifiers: G9EAccessModifier.Public);
        }

        /// <summary>
        ///     Method to unify the values between two objects.
        ///     <para />
        ///     The first object gets its new values from the second object.
        /// </summary>
        /// <param name="mainObject">Specifies the main object for getting new values from the target object.</param>
        /// <param name="targetObject">Specifies the target object for giving its values to the main object.</param>
        /// <param name="valueMismatch">Specifies the mismatch checking process for members' values</param>
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                specifiedModifiers: G9EAccessModifier.Public);
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
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch,
            bool enableTryToChangeType)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                enableTryToChangeType, G9EAccessModifier.Public);
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
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch,
            bool enableTryToChangeType, G9EAccessModifier specifiedModifiers)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                enableTryToChangeType, specifiedModifiers);
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
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch,
            bool enableTryToChangeType, BindingFlags specifiedModifiers)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                enableTryToChangeType, specifiedModifiers);
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
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch,
            bool enableTryToChangeType, G9EAccessModifier specifiedModifiers,
            Func<G9IObjectMember, bool> customFilter)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                enableTryToChangeType, specifiedModifiers, customFilter);
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
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch,
            bool enableTryToChangeType, BindingFlags specifiedModifiers,
            Func<G9IObjectMember, bool> customFilter)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                enableTryToChangeType, specifiedModifiers, customFilter);
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
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch,
            bool enableTryToChangeType, G9EAccessModifier specifiedModifiers,
            Func<G9IObjectMember, bool> customFilter,
            Func<G9IObjectMember, G9IObjectMember, object> customProcess)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                enableTryToChangeType, specifiedModifiers, customFilter, customProcess);
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
        public void UnifyObjectsValues(object mainObject, object targetObject,
            G9EValueMismatchChecking valueMismatch,
            bool enableTryToChangeType, BindingFlags specifiedModifiers,
            Func<G9IObjectMember, bool> customFilter,
            Func<G9IObjectMember, G9IObjectMember, object> customProcess)
        {
            G9CObjectHandler.UnifyObjectsValues(mainObject, targetObject, valueMismatch,
                enableTryToChangeType, specifiedModifiers, customFilter, customProcess);
        }

        #endregion
    }
}