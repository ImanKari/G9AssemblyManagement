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
    public readonly struct G9DtGenericMethod : G9IMethodMember
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
        public G9DtGenericMethod(string methodName, MethodInfo methodInfo, object targetObject)
        {
            MethodName = methodName;
            MethodInfo = methodInfo;
            _targetObject = targetObject;
        }

        /// <summary>
        ///     Method to call the specified generic method.
        /// </summary>
        /// <param name="genericTypes">Specifies the generic types for the method.</param>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        public void CallMethod(Type[] genericTypes, params object[] optionalParametersArray)
        {
            CallMethodOnAnotherObject(_targetObject, genericTypes, optionalParametersArray);
        }

        /// <summary>
        ///     Method to call the specified generic method with the result.
        /// </summary>
        /// <typeparam name="TType">Specifies the type for the method result.</typeparam>
        /// <param name="genericTypes">Specifies the generic types for the method.</param>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        /// <returns>Return value</returns>
        public TType CallMethodWithResult<TType>(Type[] genericTypes, params object[] optionalParametersArray)
        {
            return CallMethodWithResultOnAnotherObject<TType>(_targetObject, genericTypes, optionalParametersArray);
        }

        /// <summary>
        ///     Method to call the specified generic method on another object.
        ///     <para />
        ///     The specified method can be called on another object with the same structure if needed.
        /// </summary>
        /// <param name="anotherSameObject">
        ///     Specifies another object for calling the method that must have the same structure as
        ///     the primary object.
        /// </param>
        /// <param name="genericTypes">Specifies the generic types for the method.</param>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        public void CallMethodOnAnotherObject(object anotherSameObject, Type[] genericTypes,
            params object[] optionalParametersArray)
        {
            var genericMethod = MethodInfo.MakeGenericMethod(genericTypes);
            genericMethod.Invoke(anotherSameObject, optionalParametersArray);
        }

        /// <summary>
        ///     Method to call the specified generic method on another object with the result.
        ///     <para />
        ///     The specified method can be called on another object with the same structure if needed.
        /// </summary>
        /// <typeparam name="TType">Specifies the type for the method result.</typeparam>
        /// <param name="anotherSameObject">
        ///     Specifies another object for calling the method that must have the same structure as
        ///     the primary object.
        /// </param>
        /// <param name="genericTypes">Specifies the generic types for the method.</param>
        /// <param name="optionalParametersArray">Specifies the parameters for the method if needed.</param>
        /// <returns>Return value</returns>
        public TType CallMethodWithResultOnAnotherObject<TType>(object anotherSameObject, Type[] genericTypes,
            params object[] optionalParametersArray)
        {
            var genericMethod = MethodInfo.MakeGenericMethod(genericTypes);
            var result = genericMethod.Invoke(anotherSameObject, optionalParametersArray);
            try
            {
                return (TType)result;
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentException(
                    $"The specified type for the result is incorrect. The specified result type is: '{typeof(TType)}'.",
                    e);
            }
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