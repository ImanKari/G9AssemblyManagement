using System.Collections.Generic;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for reflections
    /// </summary>
    public static class G9CReflectionStaticHelper
    {
        /// <summary>
        ///     A static field for access to reflection helper
        /// </summary>
        private static readonly G9CReflectionHelper AccessToReflectionHelper = new G9CReflectionHelper();

        /// <summary>
        ///     Method to get fields of object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of fields</returns>
        public static IList<G9DtFields> G9GetFieldsOfObject<TObject>(this TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return AccessToReflectionHelper.G9GetFieldsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get properties of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of properties</returns>
        public static IList<G9DtProperties> G9GetPropertiesOfObject<TObject>(this TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return AccessToReflectionHelper.G9GetPropertiesOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get Methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of methods</returns>
        public static IList<G9DtMethods> G9GetMethodsOfObject<TObject>(this TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return AccessToReflectionHelper.G9GetMethodsOfObject(targetObject, specifiedModifiers);
        }

        /// <summary>
        ///     Method to get generic methods of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic methods</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>A collection of generic methods</returns>
        public static IList<G9DtGenericMethods> G9GetGenericMethodsOfObject<TObject>(this TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return AccessToReflectionHelper.G9GetGenericMethodsOfObject(targetObject, specifiedModifiers);
        }


        /// <summary>
        ///     Method to get all members of an object
        /// </summary>
        /// <typeparam name="TObject">Specifies the type of an object</typeparam>
        /// <param name="targetObject">Specifies an object to find generic members</param>
        /// <param name="specifiedModifiers">Specifies which modifiers will include in the searching process</param>
        /// <returns>An object with members array</returns>
        public static G9DtObjectMembers G9GetAllMembersOfObject<TObject>(this TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return AccessToReflectionHelper.G9GetAllMembersOfObject(targetObject, specifiedModifiers);
        }
    }
}