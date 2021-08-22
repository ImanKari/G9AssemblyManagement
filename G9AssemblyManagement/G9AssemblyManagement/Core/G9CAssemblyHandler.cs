using System;
using System.Collections.Generic;
using System.Linq;
using G9AssemblyManagement.DataType;

namespace G9AssemblyManagement.Core
{
    internal static class G9CAssemblyHandler
    {
        #region ### Fields And Properties ####

        /// <summary>
        ///     Lock object to handle threads to handle types
        /// </summary>
        private static readonly object TypeLookObject = new object();

        /// <summary>
        ///     Lock object to handle threads to handle new instance
        /// </summary>
        private static readonly object InstanceLookObject = new object();

        /// <summary>
        ///     Collection to save all instances of assigned type
        /// </summary>
        private static readonly SortedDictionary<int, List<object>> CollectionOfAllInstances =
            new SortedDictionary<int, List<object>>();

        /// <summary>
        ///     Collection to save all listener of assigned type
        /// </summary>
        private static readonly SortedDictionary<int, List<G9DtInstanceListener<object>>> CollectionOfAllListeners =
            new SortedDictionary<int, List<G9DtInstanceListener<object>>>();

        #endregion

        #region ### Methods ###

        /// <summary>
        ///     Method for assign a instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public static void AssignInstanceOfType(object instance)
        {
            var fullName = instance?.GetType().FullName;
            if (string.IsNullOrEmpty(fullName)) return;
            var hashCode = fullName.GetHashCode();
            lock (TypeLookObject)
            {
                if (CollectionOfAllInstances.ContainsKey(hashCode))
                    CollectionOfAllInstances[hashCode].Add(instance);
                else
                    CollectionOfAllInstances.Add(hashCode, new List<object> {instance});
                OnAssignNewInstance(hashCode, instance);
            }
        }

        /// <summary>
        ///     Method for unassign a instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public static void UnassignInstanceOfType(object instance)
        {
            var fullName = instance?.GetType().FullName;
            if (string.IsNullOrEmpty(fullName)) return;
            var hashCode = fullName.GetHashCode();
            lock (TypeLookObject)
            {
                if (!CollectionOfAllInstances.ContainsKey(hashCode)) return;
                CollectionOfAllInstances[hashCode].Remove(instance);
                if (CollectionOfAllInstances[hashCode].Count == 0)
                    CollectionOfAllInstances.Remove(hashCode);
                OnUnassignInstance(hashCode, instance);
            }
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <typeparam name="TType">
        ///     Specifies type to find instances of type.
        /// </typeparam>
        /// <returns>Return collection of instances of type.</returns>
        public static IList<TType> GetInstancesOfType<TType>()
        {
            var type = typeof(TType);
            return GetInstancesOfType(type).Select(s => (TType) s).ToArray();
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <param name="type">
        ///     Specifies type to find instances of type.
        /// </param>
        /// <returns>Return collection of instances of type.</returns>
        public static IList<object> GetInstancesOfType(Type type)
        {
            if (Equals(type, null))
                throw new ArgumentNullException(nameof(type), $"Parameter '{nameof(type)}' can't be null.");
            var fullName = type.FullName;
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(nameof(type),
                    $"Parameter '{nameof(type)}', FullName of type is null!");
            var hashCode = fullName.GetHashCode();
            lock (TypeLookObject)
            {
                if (!CollectionOfAllInstances.ContainsKey(hashCode)) return new List<object>();
                CollectionOfAllInstances[hashCode].RemoveAll(s => s.Equals(null));
                return CollectionOfAllInstances[hashCode];
            }
        }

        /// <summary>
        ///     Handle events on assign new instance item
        /// </summary>
        /// <param name="hashCode">Specifies unique hash code off instance type</param>
        /// <param name="instance">Specifies new instance</param>
        private static void OnAssignNewInstance(int hashCode, object instance)
        {
            lock (InstanceLookObject)
            {
                if (!CollectionOfAllListeners.ContainsKey(hashCode)) return;
                var listenerItems = CollectionOfAllListeners[hashCode].Where(s => s.IsActive).ToArray();
                if (!listenerItems.Any()) return;
                foreach (var instanceListener in listenerItems)
                    try
                    {
                        instanceListener.OnAssignInstanceCallback?.Invoke(instance);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            instanceListener.OnExceptionCallback?.Invoke(ex);
                        }
                        catch
                        {
                            // Ignore
                        }
                    }
            }
        }

        /// <summary>
        ///     Handle events on unassign instance item
        /// </summary>
        /// <param name="hashCode">Specifies unique hash code off instance type</param>
        /// <param name="instance">Specifies new instance</param>
        private static void OnUnassignInstance(int hashCode, object instance)
        {
            lock (InstanceLookObject)
            {
                if (!CollectionOfAllListeners.ContainsKey(hashCode)) return;
                var listenerItems = CollectionOfAllListeners[hashCode].Where(s => s.IsActive).ToArray();
                if (!listenerItems.Any()) return;
                foreach (var instanceListener in listenerItems)
                    try
                    {
                        instanceListener.OnUnassignInstanceCallback?.Invoke(instance);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            instanceListener.OnExceptionCallback?.Invoke(ex);
                        }
                        catch
                        {
                            // Ignore
                        }
                    }
            }
        }

        /// <summary>
        ///     Method to push total instances of type to a callback
        /// </summary>
        /// <param name="type">Specifies type to find instances inherited this type.</param>
        /// <param name="onAssignInstanceCallback">
        ///     Specifies callback action, the callback will be executed automatically on assign
        ///     new instance.
        /// </param>
        /// <param name="onExceptionCallback">
        ///     Specifies callback action, the callback will be executed automatically on receive
        ///     exception. if don't set it, core ignoring the exception.
        /// </param>
        private static void PushTotalInstancesOfTypeToCallback(Type type, Action<object> onAssignInstanceCallback,
            Action<Exception> onExceptionCallback = null)
        {
            var totalExistInstance = GetInstancesOfType(type);
            if (!totalExistInstance.Any()) return;
            foreach (var instance in totalExistInstance)
                try
                {
                    onAssignInstanceCallback(instance);
                }
                catch (Exception ex)
                {
                    try
                    {
                        onExceptionCallback?.Invoke(ex);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
        }

        /// <summary>
        ///     Method to push total instances of type to a callback
        /// </summary>
        /// <typeparam name="TType">Specifies type to find instances inherited this type.</typeparam>
        /// <param name="onAssignInstanceCallback">
        ///     Specifies callback action, the callback will be executed automatically on assign
        ///     new instance.
        /// </param>
        /// <param name="onExceptionCallback">
        ///     Specifies callback action, the callback will be executed automatically on receive
        ///     exception. if don't set it, core ignoring the exception.
        /// </param>
        private static void PushTotalInstancesOfTypeToCallback<TType>(Action<TType> onAssignInstanceCallback,
            Action<Exception> onExceptionCallback = null)
        {
            var totalExistInstance = GetInstancesOfType<TType>();
            if (!totalExistInstance.Any()) return;
            foreach (var instance in totalExistInstance)
                try
                {
                    onAssignInstanceCallback(instance);
                }
                catch (Exception ex)
                {
                    try
                    {
                        onExceptionCallback?.Invoke(ex);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
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
        public static G9DtInstanceListener<TType> AssignInstanceListener<TType>(Action<TType> onAssignInstanceCallback,
            Action<TType> onUnassignInstanceCallback = null, Action<Exception> onExceptionCallback = null,
            bool justListenToNewInstance = true)
        {
            var type = typeof(TType);
            var fullName = type.FullName;
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(nameof(TType),
                    $"Parameter '{nameof(TType)}', FullName of type is null!");
            var hashCode = fullName.GetHashCode();
            var instanceListenerItem =
                new G9DtInstanceListener<TType>(Guid.NewGuid(), hashCode, onAssignInstanceCallback,
                    UnassignInstanceListener, onUnassignInstanceCallback, onExceptionCallback);

            AddInstanceListener(hashCode, (G9DtInstanceListener<object>) instanceListenerItem, () =>
            {
                if (!justListenToNewInstance)
                    PushTotalInstancesOfTypeToCallback(onAssignInstanceCallback);
            });

            return instanceListenerItem;
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
        public static G9DtInstanceListener<object> AssignInstanceListener(Type type,
            Action<object> onAssignInstanceCallback, Action<object> onUnassignInstanceCallback = null,
            Action<Exception> onExceptionCallback = null, bool justListenToNewInstance = true)
        {
            var fullName = type.FullName;
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(nameof(type),
                    $"Parameter '{nameof(type)}', FullName of type is null!");
            var hashCode = fullName.GetHashCode();
            var instanceListenerItem =
                new G9DtInstanceListener<object>(Guid.NewGuid(), hashCode, onAssignInstanceCallback,
                    UnassignInstanceListener, onUnassignInstanceCallback, onExceptionCallback);

            AddInstanceListener(hashCode, instanceListenerItem, () =>
            {
                if (!justListenToNewInstance)
                    PushTotalInstancesOfTypeToCallback(type, onAssignInstanceCallback);
            });

            return instanceListenerItem;
        }

        /// <summary>
        ///     Helper to add instance listener
        /// </summary>
        /// <param name="hashCode">Specifies type full name hash code</param>
        /// <param name="instanceListenerItem">Specifies listener item</param>
        /// <param name="callBackExecuteBeforeExitLock">Specifies an action, executed before exit lock scope</param>
        private static void AddInstanceListener(int hashCode, G9DtInstanceListener<object> instanceListenerItem,
            Action callBackExecuteBeforeExitLock)
        {
            lock (InstanceLookObject)
            {
                if (CollectionOfAllListeners.ContainsKey(hashCode))
                    CollectionOfAllListeners[hashCode].Add(instanceListenerItem);
                else
                    CollectionOfAllListeners.Add(hashCode,
                        new List<G9DtInstanceListener<object>> {instanceListenerItem});
                callBackExecuteBeforeExitLock?.Invoke();
            }
        }

        /// <summary>
        ///     Method to unassign an instance listener of type
        /// </summary>
        /// <param name="typeHashCode">Specifies type full name hash code</param>
        /// <param name="listenerIdentity">Specifies listener identity</param>
        private static void UnassignInstanceListener(int typeHashCode, Guid listenerIdentity)
        {
            lock (InstanceLookObject)
            {
                if (CollectionOfAllListeners.ContainsKey(typeHashCode))
                    CollectionOfAllListeners[typeHashCode].RemoveAll(s => s.Identity == listenerIdentity);
            }
        }

        #endregion
    }
}