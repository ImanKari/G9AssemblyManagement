using System;
using System.Collections.Generic;
using System.Reflection;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for properties
    /// </summary>
    public readonly struct G9DtProperty : G9IMember
    {
        #region ### Fields And Properties ###

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public G9EMemberType MemberBasisType { get; }

        /// <inheritdoc />
        public Type MemberType { get; }

        /// <inheritdoc />
        MemberInfo G9IMemberBase.MemberInfo => PropertyInfo;

        /// <summary>
        ///     Access to property info
        /// </summary>
        public readonly PropertyInfo PropertyInfo;

        /// <summary>
        ///     Access to target object
        /// </summary>
        private readonly object _targetObject;

        #endregion

        #region ### Methods ###

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="propertyName">Specifies property name</param>
        /// <param name="propertyInfo">Specifies property value</param>
        /// <param name="targetObject">Specifies target object</param>
        public G9DtProperty(string propertyName, PropertyInfo propertyInfo, object targetObject)
        {
            Name = propertyName;
            PropertyInfo = propertyInfo;
            _targetObject = targetObject;
            MemberBasisType = G9EMemberType.Property;
            MemberType = propertyInfo.PropertyType;
        }

        /// <inheritdoc />
        public void SetValue<TType>(TType value)
        {
#if (NET35 || NET40)
            PropertyInfo.SetValue(_targetObject, value, null);
#else
            PropertyInfo.SetValue(_targetObject, value);
#endif
        }

        /// <inheritdoc />
        public void SetValue(object value)
        {
            SetValue<object>(value);
        }

        /// <inheritdoc />
        public TType GetValue<TType>()
        {
            return (TType)GetValue();
        }

        /// <inheritdoc />
        public object GetValue()
        {
#if (NET35 || NET40)
            return PropertyInfo.GetValue(_targetObject, null);
#else
            return PropertyInfo.GetValue(_targetObject);
#endif
        }

        /// <inheritdoc />
        public IList<TType> GetCustomAttributes<TType>(bool inherit) where TType : Attribute
        {
            return (IList<TType>)PropertyInfo.GetCustomAttributes(typeof(TType), inherit);
        }

        #endregion
    }
}