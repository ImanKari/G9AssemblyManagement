using System.Collections.Generic;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    public static class G9CReflectionStaticHelper
    {
        private static readonly G9CReflectionHelper _accessToReflectionHelper = new G9CReflectionHelper();

        /// <summary>
        ///     Method to get fields of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of fields</returns>
        public static IList<G9DtFields> GetFieldsOfObject<TObject>(this TObject targetObject, G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return _accessToReflectionHelper.GetFieldsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get properties of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of properties</returns>
        public static IList<G9DtProperties> GetPropertiesOfObject<TObject>(this TObject targetObject, G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return _accessToReflectionHelper.GetPropertiesOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get Methods of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find Methods</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of Methods</returns>
        public static IList<G9DtMethods> GetMethodsOfObject<TObject>(this TObject targetObject, G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return _accessToReflectionHelper.GetMethodsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get generic methods of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find generic Methods</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of generic Methods</returns>
        public static IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(this TObject targetObject, G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return _accessToReflectionHelper.GetGenericMethodsOfObject(targetObject, specifiedModifiers);
        }


        /// <summary>
        ///     Method to get all members of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find all members</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to all members of object</returns>
        public static G9DtObjectMembers GetAllMembersOfObject<TObject>(this TObject targetObject, G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return _accessToReflectionHelper.GetAllMembersOfObject(targetObject, specifiedModifiers);
        }
    }
}