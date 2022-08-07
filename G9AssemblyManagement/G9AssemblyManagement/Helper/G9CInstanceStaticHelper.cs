using System;
using System.Collections.Generic;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for instances
    /// </summary>
    public static class G9CInstanceStaticHelper
    {
        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <param name="objectItem">
        ///     Specifies an object to get type and find instances of that.
        /// </param>
        /// <returns>A collection of instances of a type.</returns>
        public static IList<object> G9GetInstancesOfType(this object objectItem)
        {
            return G9CAssemblyManagement.InstanceHandlers.G9GetInstancesOfType(objectItem.GetType());
        }

        /// <summary>
        ///     Method to assign new instance listener
        /// </summary>
        /// <typeparam name="TType">Specifies a type to listen</typeparam>
        /// <param name="objectInstance">Just for access to type</param>
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
        public static G9DtInstanceListener<TType> G9AssignInstanceListener<TType>(this TType objectInstance, Action<TType> onAssignInstanceCallback,
            Action<TType> onUnassignInstanceCallback = null, Action<Exception> onExceptionCallback = null,
            bool justListenToNewInstance = true)
        {
            return G9CAssemblyManagement.InstanceHandlers.G9AssignInstanceListener(onAssignInstanceCallback, onUnassignInstanceCallback,
                onExceptionCallback, justListenToNewInstance);
        }
    }
}