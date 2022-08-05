using System.Reflection;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for properties
    /// </summary>
    public readonly struct G9DtProperties
    {
        #region ### Fields And Properties ###

        /// <summary>
        ///     Specifies property name
        /// </summary>
        public readonly string PropertyName;

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
        public G9DtProperties(string propertyName, PropertyInfo propertyInfo, object targetObject)
        {
            PropertyName = propertyName;
            PropertyInfo = propertyInfo;
            _targetObject = targetObject;
        }

        /// <summary>
        ///     Method to set property value
        /// </summary>
        /// <typeparam name="TType">Type of value</typeparam>
        /// <param name="value">Specifies value</param>
        public void SetPropertyValue<TType>(TType value)
        {
#if (NET35 || NET40)
            PropertyInfo.SetValue(_targetObject, value, null);
#else
            PropertyInfo.SetValue(_targetObject, value);
#endif
        }

        /// <summary>
        ///     Method to set property value
        /// </summary>
        /// <param name="value">Specifies value</param>
        public void SetPropertyValue(object value)
        {
            SetPropertyValue<object>(value);
        }

        /// <summary>
        ///     Method to get property value
        /// </summary>
        /// <typeparam name="TType">Specifies value type</typeparam>
        /// <returns>Return value</returns>
        public TType GetPropertyValue<TType>()
        {
            return (TType)GetPropertyValue();
        }

        /// <summary>
        ///     Method to get property value
        /// </summary>
        /// <returns>Return value</returns>
        public object GetPropertyValue()
        {
#if (NET35 || NET40)
            return PropertyInfo.GetValue(_targetObject, null);
#else
            return PropertyInfo.GetValue(_targetObject);
#endif
        }

#endregion
    }
}