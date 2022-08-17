using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for instances
    /// </summary>
    public class G9CInstanceTools
    {
        /// <summary>
        ///     Method for assigning an instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public void AssignInstanceOfType(object instance)
        {
            G9CAssemblyHandler.AssignInstanceOfType(instance);
        }

        /// <summary>
        ///     Method for unassigning an instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public void UnassignInstanceOfType(object instance)
        {
            G9CAssemblyHandler.UnassignInstanceOfType(instance);
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <typeparam name="TType">
        ///     Specifies type to find instances of a type.
        /// </typeparam>
        /// <returns>A collection of instances of a type.</returns>
        public IList<TType> GetInstancesOfType<TType>()
        {
            return G9CAssemblyHandler.GetInstancesOfType<TType>();
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <param name="type">
        ///     Specifies type to find instances of a type.
        /// </param>
        /// <returns>Return collection of instances of type.</returns>
        public IList<object> GetInstancesOfType(Type type)
        {
            return G9CAssemblyHandler.GetInstancesOfType(type);
        }


        /// <summary>
        ///     Method to assign new instance listener
        /// </summary>
        /// <typeparam name="TType">Specifies a type to listen</typeparam>
        /// <param name="onAssignInstanceCallback">
        ///     Specifies callback action; the callback will be executed automatically on assigning a new instance.
        /// </param>
        /// <param name="onUnassignInstanceCallback">
        ///     Specifies callback action; the callback will be executed automatically on unassigning an instance.
        /// </param>
        /// <param name="onExceptionCallback">
        ///     Specifies callback action; the callback will be executed automatically on receiving an exception. If it doesn't
        ///     set, the core will ignore the exception.
        /// </param>
        /// <param name="justListenToNewInstance">
        ///     If set to true, the callback will execute to find a new instance. Otherwise, the callback executes per all old
        ///     instances (if anything exists) and then listens to the new instance.
        /// </param>
        /// <returns>Instance listener object to handle</returns>
        public G9DtInstanceListener<TType> AssignInstanceListener<TType>(Action<TType> onAssignInstanceCallback,
            Action<TType> onUnassignInstanceCallback = null, Action<Exception> onExceptionCallback = null,
            bool justListenToNewInstance = true)
        {
            return G9CAssemblyHandler.AssignInstanceListener(onAssignInstanceCallback, onUnassignInstanceCallback,
                onExceptionCallback, justListenToNewInstance);
        }

        /// <summary>
        ///     Method to assign new instance listener
        /// </summary>
        /// <param name="type">Specifies a type to listen</param>
        /// <param name="onAssignInstanceCallback">
        ///     Specifies callback action; the callback will be executed automatically on assigning a new instance.
        /// </param>
        /// <param name="onUnassignInstanceCallback">
        ///     Specifies callback action; the callback will be executed automatically on unassigning an instance.
        /// </param>
        /// <param name="onExceptionCallback">
        ///     Specifies callback action; the callback will be executed automatically on receiving an exception. If it doesn't
        ///     set, the core will ignore the exception.
        /// </param>
        /// <param name="justListenToNewInstance">
        ///     If set to true, the callback will execute to find a new instance. Otherwise, the callback executes per all old
        ///     instances (if anything exists) and then listens to the new instance.
        /// </param>
        /// <returns>Instance listener object to handle</returns>
        public G9DtInstanceListener<object> AssignInstanceListener(Type type, Action<object> onAssignInstanceCallback,
            Action<object> onUnassignInstanceCallback = null,
            Action<Exception> onExceptionCallback = null,
            bool justListenToNewInstance = true)
        {
            return G9CAssemblyHandler.AssignInstanceListener(type, onAssignInstanceCallback, onUnassignInstanceCallback,
                onExceptionCallback, justListenToNewInstance);
        }

        /// <summary>
        ///     Method to create an uninitialized instance from a type
        /// </summary>
        /// <typeparam name="TType">Specifies a type for creating an instance; the type must be creatable.</typeparam>
        /// <returns>A created object from type</returns>
        public TType CreateUninitializedInstanceFromType<TType>()
        {
            return (TType)FormatterServices.GetUninitializedObject(typeof(TType));
        }

        /// <summary>
        ///     Method to create an uninitialized instance from a type
        /// </summary>
        /// <param name="type">Specifies a type for creating an instance; the type must be the same or derived by TType.</param>
        /// <returns>A created object from type</returns>
        public object CreateUninitializedInstanceFromType(Type type)
        {
            return FormatterServices.GetUninitializedObject(type);
        }

        /// <summary>
        ///     Method to create an instance from a type
        /// </summary>
        /// <typeparam name="TType">Specifies a type for creating an instance; the type must be creatable.</typeparam>
        /// <returns>A created object from type</returns>
        public TType CreateInstanceFromType<TType>() where TType : new()
        {
            return Activator.CreateInstance<TType>();
        }

        /// <summary>
        ///     Method to create an instance from a type
        /// </summary>
        /// <param name="type">Specifies a type for creating an instance; the type must be the same or derived by TType.</param>
        /// <returns>A created object from type</returns>
        public object CreateInstanceFromType(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        ///     Method to create an instance from a type with the constructor that has parameters
        /// </summary>
        /// <typeparam name="TType">Specifies a type for creating an instance; the type must be creatable.</typeparam>
        /// <param name="parameters">Specifies constructor parameters</param>
        /// <returns>A created object from type</returns>
        public TType CreateInstanceFromTypeWithParameters<TType>(params object[] parameters)
        {
            return (TType)Activator.CreateInstance(typeof(TType), parameters);
        }

        /// <summary>
        ///     Method to create an instance from a type with the constructor that has parameters
        /// </summary>
        /// <param name="type">Specifies a type for creating an instance; the type must be the same or derived by TType.</param>
        /// <param name="parameters">Specifies constructor parameters</param>
        /// <returns>A created object from type</returns>
        public object CreateInstanceFromTypeWithParameters(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }
    }
}