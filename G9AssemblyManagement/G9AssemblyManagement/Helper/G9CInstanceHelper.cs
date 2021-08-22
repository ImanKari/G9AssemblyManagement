using System;
using System.Collections.Generic;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for instances
    /// </summary>
    public class G9CInstanceHelper
    {
        /// <summary>
        ///     Method for assign a instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public void AssignInstanceOfType(object instance)
        {
            G9CAssemblyHandler.AssignInstanceOfType(instance);
        }

        /// <summary>
        ///     Method for unassign a instance of type
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
        ///     Specifies type to find instances of type.
        /// </typeparam>
        /// <returns>Return collection of instances of type.</returns>
        public IList<TType> GetInstancesOfType<TType>()
        {
            return G9CAssemblyHandler.GetInstancesOfType<TType>();
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <param name="type">
        ///     Specifies type to find instances of type.
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
        ///     Specifies callback action, the callback will be executed automatically on assign
        ///     new instance.
        /// </param>
        /// <param name="onUnassignInstanceCallback">
        ///     Specifies callback action, the callback will be executed automatically on
        ///     unassign an instance.
        /// </param>
        /// <param name="onExceptionCallback">
        ///     Specifies callback action, the callback will be executed automatically on receive
        ///     exception. if don't set it, core ignoring the exception.
        /// </param>
        /// <param name="justListenToNewInstance">
        ///     If set to true, the callback is executed just to find a new instance. otherwise,
        ///     callback executes per all old instances (if exists) and then listens to the new instance.
        /// </param>
        /// <returns>Instance listener type, to handle listener</returns>
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
        ///     Specifies callback action, the callback will be executed automatically on assign
        ///     new instance.
        /// </param>
        /// <param name="onUnassignInstanceCallback">
        ///     Specifies callback action, the callback will be executed automatically on
        ///     unassign an instance.
        /// </param>
        /// <param name="onExceptionCallback">
        ///     Specifies callback action, the callback will be executed automatically on receive
        ///     exception. if don't set it, core ignoring the exception.
        /// </param>
        /// <param name="justListenToNewInstance">
        ///     If set to true, the callback is executed just to find a new instance. otherwise,
        ///     callback executes per all old instances (if exists) and then listens to the new instance.
        /// </param>
        /// <returns>Instance listener type, to handle listener</returns>
        public G9DtInstanceListener<object> AssignInstanceListener(Type type, Action<object> onAssignInstanceCallback,
            Action<object> onUnassignInstanceCallback = null,
            Action<Exception> onExceptionCallback = null,
            bool justListenToNewInstance = true)
        {
            return G9CAssemblyHandler.AssignInstanceListener(type, onAssignInstanceCallback, onUnassignInstanceCallback,
                onExceptionCallback, justListenToNewInstance);
        }
    }
}