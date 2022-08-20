using System.Collections.Generic;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for members of object
    /// </summary>
    public readonly struct G9DtMember
    {
        #region ### Fields And Properties ###

        /// <summary>
        ///     Access to total fields of object
        /// </summary>
        public readonly IList<G9DtField> Fields;

        /// <summary>
        ///     Access to total properties of object
        /// </summary>
        public readonly IList<G9DtProperty> Properties;

        /// <summary>
        ///     Access to total methods of object
        /// </summary>
        public readonly IList<G9DtMethod> Methods;

        /// <summary>
        ///     Access to total generic methods of object
        /// </summary>
        public readonly IList<G9DtGenericMethod> GenericMethods;

        #endregion

        #region ### Methods ###

        /// <summary>
        /// </summary>
        public G9DtMember(IList<G9DtField> fields, IList<G9DtProperty> properties, IList<G9DtMethod> methods,
            IList<G9DtGenericMethod> genericMethods)
        {
            Fields = fields;
            Properties = properties;
            Methods = methods;
            GenericMethods = genericMethods;
        }

        #endregion
    }
}