using System.Collections.Generic;
using System.Reflection;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for fields
    /// </summary>
    public readonly struct G9DtFields : G9IObjectMember
    {
        #region ### Fields And Properties ###

        /// <inheritdoc />
        public string Name { get; }

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
        public G9DtFields(string fieldName, FieldInfo fieldInfo, object targetObject)
        {
            Name = fieldName;
            FieldInfo = fieldInfo;
            _targetObject = targetObject;
        }

        /// <inheritdoc />
        public void SetValue<TType>(TType value)
        {
            FieldInfo.SetValue(_targetObject, value);
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
            return FieldInfo.GetValue(_targetObject);
        }

        /// <inheritdoc />
        public IList<TType> GetCustomAttributes<TType>(bool inherit) where TType : System.Attribute
        {
            return (IList<TType>)FieldInfo.GetCustomAttributes(typeof(TType), inherit);
        }

        /// <inheritdoc />
        public MemberInfo GetMemberInfo()
        {
            return FieldInfo;
        }

        #endregion
    }
}