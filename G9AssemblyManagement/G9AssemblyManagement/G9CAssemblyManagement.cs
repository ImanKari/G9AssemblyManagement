using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.Core;

namespace G9AssemblyManagement
{
    public static class G9CAssemblyManagement
    {
        #region ### Fields And Properties

        #endregion

        #region ### Methods ###

        public static IList<Type> GetInheritedTypesOfType<TType>(
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            return GetInheritedTypesOfType(typeof(TType), ignoreAbstractType, ignoreInterfaceType,  assemblies);
        }

        public static IList<Type> GetInheritedTypesOfType(this object objectItem,
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            return GetInheritedTypesOfType(objectItem.GetType(), ignoreAbstractType, ignoreInterfaceType, assemblies);
        }

        public static IList<Type> GetInheritedTypesOfType(Type type,
            bool ignoreAbstractType = true, bool ignoreInterfaceType = true, params Assembly[] assemblies)
        {
            // Set assemblies
            assemblies = Equals(assemblies, null) || !assemblies.Any()
                ? AppDomain.CurrentDomain.GetAssemblies()
                : assemblies;
            return G9CTypeManagement.GetDerivedTypes(type, ignoreAbstractType, ignoreInterfaceType, assemblies);
        }

        #endregion
    }
}