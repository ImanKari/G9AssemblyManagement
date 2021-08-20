using System.Collections.Generic;
using G9AssemblyManagement.Core;

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
        ///     Specifies an object to get type and find instances of type.
        /// </param>
        /// <returns>Return collection of instances of type.</returns>
        public static IList<object> GetInstancesOfType(this object objectItem)
        {
            return G9CAssemblyHandler.GetInstancesOfType(objectItem.GetType());
        }
    }
}