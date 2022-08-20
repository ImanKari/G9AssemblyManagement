using System.Collections.Generic;
using System.Reflection;
using G9AssemblyManagement.Interfaces;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for methods
    /// </summary>
    public readonly struct G9DtMethod : G9IMethodMember
    {
        #region ### Fields And Properties ###

        /// <inheritdoc />
        public string MethodName { get; }

        /// <inheritdoc />
        public MethodInfo MethodInfo { get; }

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
        public G9DtMethod(string methodName, MethodInfo methodInfo, object targetObject)
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
            return (TType)MethodInfo.Invoke(_targetObject, optionalParametersArray);
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

        /// <inheritdoc />
        public IList<TType> GetCustomAttributes<TType>(bool inherit) where TType : System.Attribute
        {
            return (IList<TType>)MethodInfo.GetCustomAttributes(typeof(TType), inherit);
        }

        #endregion
    }
}