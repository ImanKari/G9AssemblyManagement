using System.Reflection;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for methods
    /// </summary>
    public readonly struct G9DtMethods
    {
        #region ### Fields And Properties ###

        /// <summary>
        ///     Specifies method name
        /// </summary>
        public readonly string MethodName;

        /// <summary>
        ///     Access to method info
        /// </summary>
        public readonly MethodInfo MethodInfo;

        /// <summary>
        ///     Access to target object
        /// </summary>
        private readonly object _targetObject;

        #endregion

        #region ### Methods ###

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="methodName">Specifies method name</param>
        /// <param name="methodInfo">Specifies method value</param>
        /// <param name="targetObject">Specifies target object</param>
        public G9DtMethods(string methodName, MethodInfo methodInfo, object targetObject)
        {
            MethodName = methodName;
            MethodInfo = methodInfo;
            _targetObject = targetObject;
        }

        /// <summary>
        ///     Execute specifies method with result
        /// </summary>
        /// <typeparam name="TType">Specifies value type</typeparam>
        /// <param name="optionalParametersArray">Specifies optional parameters</param>
        /// <returns>Return value</returns>
        public TType CallMethodWithResult<TType>(params object[] optionalParametersArray)
        {
            return (TType) MethodInfo.Invoke(_targetObject, optionalParametersArray);
        }

        /// <summary>
        ///     Execute specifies method
        /// </summary>
        /// <param name="optionalParametersArray">Specifies optional parameters</param>
        /// <returns>Return value</returns>
        public void CallMethod(params object[] optionalParametersArray)
        {
            MethodInfo.Invoke(_targetObject, optionalParametersArray);
        }

        #endregion
    }
}