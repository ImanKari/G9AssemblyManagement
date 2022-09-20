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
    ///     Helper class for objects and reflections
    /// </summary>
    public class G9CObjectAndReflectionTools
    {
        /// <summary>
        ///     Method to create custom modifier
        /// </summary>
        /// <param name="customModifier">Specifies custom modifiers are to be included in the search.</param>
        /// <returns>Return a custom BindingFlags object</returns>
        public BindingFlags CreateCustomModifier(
            G9EAccessModifier customModifier = G9EAccessModifier.Everything)
        {
            if (customModifier == G9EAccessModifier.Everything)
                return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            var defaultBindingFlags = (customModifier & G9EAccessModifier.StaticAndInstance) ==
                                      G9EAccessModifier.StaticAndInstance
                ? BindingFlags.Instance | BindingFlags.Static
                : (customModifier & G9EAccessModifier.Static) == G9EAccessModifier.Static
                    ? BindingFlags.Static
                    : BindingFlags.Instance;

            if ((customModifier & G9EAccessModifier.Public) == G9EAccessModifier.Public)
                defaultBindingFlags |= BindingFlags.Public;
            if ((customModifier & G9EAccessModifier.NonPublic) == G9EAccessModifier.NonPublic)
                defaultBindingFlags |= BindingFlags.NonPublic;

            return defaultBindingFlags;
        }

        #region GetFields Methods

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetFieldsOfObject" />
        public IList<G9DtField> GetFieldsOfObject(object targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetFieldsOfObject(targetObject,
                CreateCustomModifier(specifiedModifiers), customFilter,
                considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetFieldsOfObject" />
        public IList<G9DtField> GetFieldsOfObject(object targetObject,
            BindingFlags specifiedModifiers,
            Func<FieldInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetFieldsOfObject(targetObject, specifiedModifiers, customFilter,
                considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetFieldsOfType" />
        public IList<G9DtField> GetFieldsOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetFieldsOfType(targetType,
                CreateCustomModifier(specifiedModifiers), customFilter,
                null, initializeInstance, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetFieldsOfType" />
        public IList<G9DtField> GetFieldsOfType(Type targetType,
            BindingFlags specifiedModifiers,
            Func<FieldInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetFieldsOfType(targetType, specifiedModifiers, customFilter, null,
                initializeInstance, considerInheritedParent);
        }

        #endregion

        #region GetProperties Methods

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetPropertiesOfObject" />
        public IList<G9DtProperty> GetPropertiesOfObject(object targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<PropertyInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetPropertiesOfObject(targetObject,
                CreateCustomModifier(specifiedModifiers), customFilter,
                considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetPropertiesOfObject" />
        public IList<G9DtProperty> GetPropertiesOfObject(object targetObject,
            BindingFlags specifiedModifiers,
            Func<PropertyInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetPropertiesOfObject(targetObject, specifiedModifiers, customFilter,
                considerInheritedParent);
        }


        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetPropertiesOfType" />
        public IList<G9DtProperty> GetPropertiesOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<PropertyInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetPropertiesOfType(targetType,
                CreateCustomModifier(specifiedModifiers), customFilter,
                null, initializeInstance, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetPropertiesOfType" />
        public IList<G9DtProperty> GetPropertiesOfType(Type targetType,
            BindingFlags specifiedModifiers,
            Func<PropertyInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetPropertiesOfType(targetType,
                specifiedModifiers, customFilter,
                null, initializeInstance, considerInheritedParent);
        }

        #endregion

        #region GetMethods Methods

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetMethodsOfObject" />
        public IList<G9DtMethod> GetMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetMethodsOfObject(targetObject,
                CreateCustomModifier(specifiedModifiers),
                customFilter, considerInheritedParent
            );
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetMethodsOfObject" />
        public IList<G9DtMethod> GetMethodsOfObject(object targetObject,
            BindingFlags specifiedModifiers,
            Func<MethodInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetMethodsOfObject(targetObject, specifiedModifiers,
                customFilter, considerInheritedParent
            );
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetMethodsOfType" />
        public IList<G9DtMethod> GetMethodsOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetMethodsOfType(targetType,
                CreateCustomModifier(specifiedModifiers), customFilter, null,
                initializeInstance, considerInheritedParent
            );
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetMethodsOfType" />
        public IList<G9DtMethod> GetMethodsOfType(Type targetType,
            BindingFlags specifiedModifiers,
            Func<MethodInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetMethodsOfType(targetType,
                specifiedModifiers, customFilter, null,
                initializeInstance, considerInheritedParent
            );
        }

        #endregion

        #region GetGenericMethodsOfObject Methods

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetGenericMethodsOfObject" />
        public IList<G9DtGenericMethod> GetGenericMethodsOfObject(object targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetGenericMethodsOfObject(targetObject,
                CreateCustomModifier(specifiedModifiers)
                ,
                customFilter, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetGenericMethodsOfObject" />
        public IList<G9DtGenericMethod> GetGenericMethodsOfObject(object targetObject,
            BindingFlags specifiedModifiers,
            Func<MethodInfo, bool> customFilter = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetGenericMethodsOfObject(targetObject, specifiedModifiers,
                customFilter, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetGenericMethodsOfType" />
        public IList<G9DtGenericMethod> GetGenericMethodsOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<MethodInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetGenericMethodsOfType(targetType,
                CreateCustomModifier(specifiedModifiers)
                , customFilter,
                null, initializeInstance, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetGenericMethodsOfType" />
        public IList<G9DtGenericMethod> GetGenericMethodsOfType(Type targetType,
            BindingFlags specifiedModifiers,
            Func<MethodInfo, bool> customFilter = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetGenericMethodsOfType(targetType, specifiedModifiers, customFilter,
                null, initializeInstance, considerInheritedParent);
        }

        #endregion

        #region GetAllMembersOfObject Methods

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetAllMembersOfObject" />
        public G9DtMember GetAllMembersOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetAllMembersOfObject(targetObject,
                CreateCustomModifier(specifiedModifiers),
                customFilterForFields, customFilterForProperties, customFilterMethods, customFilterForGenericMethods,
                considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetAllMembersOfObject" />
        public G9DtMember GetAllMembersOfObject<TObject>(TObject targetObject,
            BindingFlags specifiedModifiers,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetAllMembersOfObject(targetObject, specifiedModifiers,
                customFilterForFields, customFilterForProperties, customFilterMethods, customFilterForGenericMethods,
                considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetAllMembersOfType" />
        public G9DtMember GetAllMembersOfType(Type targetType,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null, bool initializeInstance = false,
            bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetAllMembersOfType(targetType,
                CreateCustomModifier(specifiedModifiers),
                customFilterForFields, customFilterForProperties, customFilterMethods, customFilterForGenericMethods,
                null, initializeInstance, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.GetAllMembersOfType" />
        public G9DtMember GetAllMembersOfType(Type targetType,
            BindingFlags specifiedModifiers,
            Func<FieldInfo, bool> customFilterForFields = null,
            Func<PropertyInfo, bool> customFilterForProperties = null,
            Func<MethodInfo, bool> customFilterMethods = null,
            Func<MethodInfo, bool> customFilterForGenericMethods = null,
            bool initializeInstance = false, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.GetAllMembersOfType(targetType, specifiedModifiers,
                customFilterForFields, customFilterForProperties, customFilterMethods, customFilterForGenericMethods,
                null, initializeInstance, considerInheritedParent);
        }

        #endregion

        #region MergeObjectsValues Methods

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.MergeObjectsValues" />
        public void MergeObjectsValues(object mainObject, object targetObject,
            G9EAccessModifier specifiedModifiers,
            G9EValueMismatchChecking valueMismatch = G9EValueMismatchChecking.AllowMismatchValues,
            bool enableTryToChangeType = false,
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, bool> customProcess = null, bool considerInheritedParent = false)
        {
            G9CObjectAndReflectionHandler.MergeObjectsValues(mainObject, targetObject,
                CreateCustomModifier(specifiedModifiers), valueMismatch,
                enableTryToChangeType,
                customFilter, customProcess, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.MergeObjectsValues" />
        public void MergeObjectsValues(object mainObject, object targetObject,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public,
            G9EValueMismatchChecking valueMismatch = G9EValueMismatchChecking.AllowMismatchValues,
            bool enableTryToChangeType = false,
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, bool> customProcess = null, bool considerInheritedParent = false)
        {
            G9CObjectAndReflectionHandler.MergeObjectsValues(mainObject, targetObject, specifiedModifiers,
                valueMismatch,
                enableTryToChangeType, customFilter, customProcess, considerInheritedParent);
        }

        #endregion

        #region CompareObjectsValues Methods

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.CompareObjectsValues" />
        public bool CompareObjectsValues(object firstObject, object secondObject,
            out IList<G9DtTuple<G9IMember>> unequalMembers,
            G9EAccessModifier specifiedModifiers,
            bool enableTryToChangeType = false,
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, G9EComparisonResult> customProcess = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.CompareObjectsValues(firstObject, secondObject, out unequalMembers,
                CreateCustomModifier(specifiedModifiers), enableTryToChangeType,
                customFilter, customProcess, considerInheritedParent);
        }

        /// <inheritdoc cref="G9CObjectAndReflectionHandler.CompareObjectsValues" />
        public bool CompareObjectsValues(object firstObject, object secondObject,
            out IList<G9DtTuple<G9IMember>> unequalMembers,
            BindingFlags specifiedModifiers = BindingFlags.Instance | BindingFlags.Public,
            bool enableTryToChangeType = false,
            Func<G9IMember, bool> customFilter = null,
            Func<G9IMember, G9IMember, G9EComparisonResult> customProcess = null, bool considerInheritedParent = false)
        {
            return G9CObjectAndReflectionHandler.CompareObjectsValues(firstObject, secondObject, out unequalMembers,
                specifiedModifiers, enableTryToChangeType, customFilter, customProcess, considerInheritedParent);
        }

        #endregion
    }
}