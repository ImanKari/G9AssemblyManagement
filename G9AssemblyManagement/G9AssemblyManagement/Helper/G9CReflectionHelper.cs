using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class G9CReflectionHelper
    {
        /// <summary>
        ///     Method to get fields of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find fields</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of fields</returns>
        public IList<G9DtFields> GetFieldsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return targetObject.GetType()
                .GetFields(CreateCustomModifier(specifiedModifiers))
                .Select(s => new G9DtFields(s.Name, s, targetObject)).ToArray();
        }

        /// <summary>
        ///     Method to get properties of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find properties</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of properties</returns>
        public IList<G9DtProperties> GetPropertiesOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return targetObject.GetType()
                .GetProperties(CreateCustomModifier(specifiedModifiers))
                .Select(s => new G9DtProperties(s.Name, s, targetObject))
                .ToArray();
        }

        /// <summary>
        ///     Method to get Methods of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find Methods</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of Methods</returns>
        public IList<G9DtMethods> GetMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return targetObject.GetType()
                .GetMethods(CreateCustomModifier(specifiedModifiers))
                .Where(s => !s.IsGenericMethod)
                .Select(s => new G9DtMethods(s.Name, s, targetObject))
                .ToArray();
        }

        /// <summary>
        ///     Method to get generic methods of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find generic Methods</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to collection of generic Methods</returns>
        public IList<G9DtGenericMethods> GetGenericMethodsOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return targetObject.GetType()
                .GetMethods(CreateCustomModifier(specifiedModifiers))
                .Where(s => s.IsGenericMethod)
                .Select(s => new G9DtGenericMethods(s.Name, s, targetObject))
                .ToArray();
        }


        /// <summary>
        ///     Method to get all members of object
        /// </summary>
        /// <typeparam name="TObject">Specifies type of object</typeparam>
        /// <param name="targetObject">Specifies object to find all members</param>
        /// <param name="specifiedModifiers">Specifies which modifier types are to be included in the search</param>
        /// <returns>Access to all members of object</returns>
        public G9DtObjectMembers GetAllMembersOfObject<TObject>(TObject targetObject,
            G9EAccessModifier specifiedModifiers = G9EAccessModifier.Everything)
            where TObject : new()
        {
            return new G9DtObjectMembers(
                GetFieldsOfObject(targetObject, specifiedModifiers),
                GetPropertiesOfObject(targetObject, specifiedModifiers),
                GetMethodsOfObject(targetObject, specifiedModifiers),
                GetGenericMethodsOfObject(targetObject, specifiedModifiers)
            );
        }

        /// <summary>
        ///     Method to create custom modifier
        /// </summary>
        /// <param name="customModifier">Specifies custom modifiers are to be included in the search.</param>
        /// <returns>Return a custom BindingFlags object</returns>
        private BindingFlags CreateCustomModifier(G9EAccessModifier customModifier = G9EAccessModifier.Everything)
        {
            if (customModifier == G9EAccessModifier.Everything)
                return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            var defaultBindingFlags = (customModifier & G9EAccessModifier.Static) == G9EAccessModifier.Static ? BindingFlags.Static : BindingFlags.Instance;

            if ((customModifier & G9EAccessModifier.Public) == G9EAccessModifier.Public)
                defaultBindingFlags |= BindingFlags.Public;
            if ((customModifier & G9EAccessModifier.NonPublic) == G9EAccessModifier.NonPublic)
                defaultBindingFlags |= BindingFlags.NonPublic;
            
            return defaultBindingFlags;
        }
    }
}