using System;
using System.Collections.Generic;
using G9AssemblyManagement.Core;

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
    }
}