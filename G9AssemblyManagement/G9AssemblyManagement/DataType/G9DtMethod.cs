using System;
using System.Collections.Generic;
using System.Linq;
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
        ///     Method to call the specified method.
        /// </summary>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        public void CallMethod(params object[] optionalParametersArray)
        {
            CallMethodOnAnotherObject(_targetObject, optionalParametersArray);
        }

        /// <summary>
        ///     Method to call the specified method with the result.
        /// </summary>
        /// <typeparam name="TType">Specifies the type for the method result.</typeparam>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        /// <returns>Return value</returns>
        public TType CallMethodWithResult<TType>(params object[] optionalParametersArray)
        {
            return CallMethodWithResultOnAnotherObject<TType>(_targetObject, optionalParametersArray);
        }

        /// <summary>
        ///     Method to call the specified method on another object.
        ///     <para />
        ///     The specified method can be called on another object with the same structure if needed.
        /// </summary>
        /// <param name="anotherSameObject">
        ///     Specifies another object for calling the method that must have the same structure as
        ///     the primary object.
        /// </param>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        public void CallMethodOnAnotherObject(object anotherSameObject, params object[] optionalParametersArray)
        {
            MethodInfo.Invoke(anotherSameObject, optionalParametersArray);
        }

        /// <summary>
        ///     Method to call the specified method on another object with the result.
        ///     <para />
        ///     The specified method can be called on another object with the same structure if needed.
        /// </summary>
        /// <typeparam name="TType">Specifies the type for the method result.</typeparam>
        /// <param name="anotherSameObject">
        ///     Specifies another object for calling the method that must have the same structure as
        ///     the primary object.
        /// </param>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        /// <returns>Return value</returns>
        public TType CallMethodWithResultOnAnotherObject<TType>(object anotherSameObject,
            params object[] optionalParametersArray)
        {
            return (TType)MethodInfo.Invoke(anotherSameObject, optionalParametersArray);
        }

        /// <inheritdoc />
        public IList<TType> GetCustomAttributes<TType>(bool inherit) where TType : Attribute
        {
            var result = MethodInfo.GetCustomAttributes(typeof(TType), inherit);
            return result.Length == 0
                ? null
                : (IList<TType>)result;
        }

        /// <inheritdoc />
        public TType GetCustomAttribute<TType>(bool inherit) where TType : Attribute
        {
            var result = MethodInfo.GetCustomAttributes(typeof(TType), inherit);
            return result.Length == 0
                ? null
                : (TType)result.First();
        }

        #endregion
    }
}