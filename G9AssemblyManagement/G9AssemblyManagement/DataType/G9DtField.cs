using System;
using System.Collections.Generic;
using System.Reflection;
using G9AssemblyManagement.Enums;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for fields
    /// </summary>
    public readonly struct G9DtField : G9IMember
    {
        #region ### Fields And Properties ###

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public G9EMemberType MemberBasisType { get; }

        /// <inheritdoc />
        public Type MemberType { get; }

        /// <inheritdoc />
        MemberInfo G9IMemberBase.MemberInfo => FieldInfo;

        /// <summary>
        ///     Access to field info
        /// </summary>
        public readonly FieldInfo FieldInfo;

        /// <summary>
        ///     Access to target object
        /// </summary>
        private readonly object _targetObject;

        #endregion

        #region ### Methods ###

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="fieldName">Specifies field name</param>
        /// <param name="fieldInfo">Specifies field value</param>
        /// <param name="targetObject">Specifies target object</param>
        public G9DtField(string fieldName, FieldInfo fieldInfo, object targetObject)
        {
            Name = fieldName;
            FieldInfo = fieldInfo;
            _targetObject = targetObject;
            MemberBasisType = G9EMemberType.Field;
            MemberType = fieldInfo.FieldType;
        }

        /// <inheritdoc />
        public void SetValue(object value)
        {
            SetValueOnAnotherObject(_targetObject, value);
        }

        /// <inheritdoc />
        public void SetValueOnAnotherObject(object anotherSameObject, object value)
        {
            FieldInfo.SetValue(anotherSameObject, value);
        }

        /// <inheritdoc />
        public TType GetValue<TType>()
        {
            return (TType)GetValueOnAnotherObject(_targetObject);
        }

        /// <inheritdoc />
        public object GetValue()
        {
            return GetValueOnAnotherObject(_targetObject);
        }

        /// <inheritdoc />
        public TType GetValueOnAnotherObject<TType>(object anotherSameObject)
        {
            return (TType)GetValueOnAnotherObject(anotherSameObject);
        }

        /// <inheritdoc />
        public object GetValueOnAnotherObject(object anotherSameObject)
        {
            return FieldInfo.GetValue(anotherSameObject);
        }

        /// <inheritdoc />
        public IList<TType> GetCustomAttributes<TType>(bool inherit) where TType : Attribute
        {
            return (IList<TType>)FieldInfo.GetCustomAttributes(typeof(TType), inherit);
        }

        #endregion
    }
}