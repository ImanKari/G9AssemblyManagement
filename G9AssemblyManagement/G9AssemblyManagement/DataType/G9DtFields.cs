using System.Reflection;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for fields
    /// </summary>
    public readonly struct G9DtFields
    {
        #region ### Fields And Properties ###

        /// <summary>
        ///     Specifies field name
        /// </summary>
        public readonly string FieldName;

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
            FieldName = fieldName;
            FieldInfo = fieldInfo;
            _targetObject = targetObject;
        }

        /// <summary>
        ///     Method to set field value
        /// </summary>
        /// <typeparam name="TType">Type of value</typeparam>
        /// <param name="value">Specifies value</param>
        public void SetFieldValue<TType>(TType value)
        {
            FieldInfo.SetValue(_targetObject, value);
        }

        /// <summary>
        ///     Method to set field value
        /// </summary>
        /// <param name="value">Specifies value</param>
        public void SetFieldValue(object value)
        {
            SetFieldValue<object>(value);
        }

        /// <summary>
        ///     Method to get field value
        /// </summary>
        /// <typeparam name="TType">Specifies value type</typeparam>
        /// <returns>Return value</returns>
        public TType GetFieldValue<TType>()
        {
            return (TType)GetFieldValue();
        }

        /// <summary>
        ///     Method to get field value
        /// </summary>
        /// <returns>Return value</returns>
        public object GetFieldValue()
        {
            return FieldInfo.GetValue(_targetObject);
        }

        #endregion
    }
}