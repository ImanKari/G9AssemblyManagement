using System;
using System.Collections.Generic;
using System.Linq;

namespace G9AssemblyManagement.Core
{
    internal static class G9CAssemblyHandler
    {
        #region ### Fields And Properties ####

        private static readonly object lookObject = new object();

        /// <summary>
        ///     Collection to save all instances of assigned type
        /// </summary>
        private static readonly SortedDictionary<int, List<object>> CollectionOfAllInstance =
            new SortedDictionary<int, List<object>>();

        #endregion

        #region ### Methods ###

        /// <summary>
        ///     Method for assign a instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public static void AssignInstanceOfType(object instance)
        {
            var fullName = instance?.GetType().FullName;
            if (Equals(fullName, null)) return;
            var hashCode = fullName.GetHashCode();
            lock (lookObject)
            {
                if (CollectionOfAllInstance.ContainsKey(hashCode))
                    CollectionOfAllInstance[hashCode].Add(instance);
                else
                    CollectionOfAllInstance.Add(hashCode, new List<object> {instance});
            }
        }

        /// <summary>
        ///     Method for unassign a instance of type
        /// </summary>
        /// <param name="instance">Specifies an instance of type</param>
        public static void UnassignInstanceOfType(object instance)
        {
            var fullName = instance?.GetType().FullName;
            if (Equals(fullName, null)) return;
            var hashCode = fullName.GetHashCode();
            lock (lookObject)
            {
                if (!CollectionOfAllInstance.ContainsKey(hashCode)) return;
                CollectionOfAllInstance[hashCode].Remove(instance);
                if (CollectionOfAllInstance[hashCode].Count == 0)
                    CollectionOfAllInstance.Remove(hashCode);
            }
        }

        /// <summary>
        ///     Method to get total instances of type
        /// </summary>
        /// <typeparam name="TType">
        ///     Specifies type to find instances of type.
        /// </typeparam>
        /// <para />
        /// Notice: The specified type must be one of the inherited types (class or struct).
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
        ///     <para />
        ///     Notice: The specified type must be one of the inherited types (class or struct).
        /// </param>
        /// <returns>Return collection of instances of type.</returns>
        public static IList<object> GetInstancesOfType(Type type)
        {
            if (Equals(type, null))
                throw new ArgumentNullException(nameof(type), $"Parameter '{nameof(type)}' can't be null.");
            var fullName = type.FullName;
            if (Equals(fullName, null))
                throw new ArgumentNullException(nameof(type),
                    $"Parameter '{nameof(type)}', FullName of type is null!");
            if (!type.IsClass && !type.IsValueType)
                throw new ArgumentException("The specified type must be one of the inherited types (class or struct).",
                    nameof(type));
            var hashCode = fullName.GetHashCode();
            lock (lookObject)
            {
                if (!CollectionOfAllInstance.ContainsKey(hashCode)) return new List<object>();
                CollectionOfAllInstance[hashCode].RemoveAll(s => s.Equals(null));
                return CollectionOfAllInstance[hashCode];
            }
        }

        #endregion
    }
}