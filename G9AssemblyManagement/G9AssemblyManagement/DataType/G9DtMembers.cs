using System.Collections.Generic;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for members of object
    /// </summary>
    public readonly struct G9DtMembers
    {
        #region ### Fields And Properties ###

        /// <summary>
        ///     Access to total fields of object
        /// </summary>
        public readonly IList<G9DtFields> Fields;

        /// <summary>
        ///     Access to total properties of object
        /// </summary>
        public readonly IList<G9DtProperties> Properties;

        /// <summary>
        ///     Access to total methods of object
        /// </summary>
        public readonly IList<G9DtMethods> Methods;

        /// <summary>
        ///     Access to total generic methods of object
        /// </summary>
        public readonly IList<G9DtGenericMethods> GenericMethods;

        #endregion

        #region ### Methods ###

        /// <summary>
        /// </summary>
        public G9DtMembers(IList<G9DtFields> fields, IList<G9DtProperties> properties, IList<G9DtMethods> methods,
            IList<G9DtGenericMethods> genericMethods)
        {
            Fields = fields;
            Properties = properties;
            Methods = methods;
            GenericMethods = genericMethods;
        }

        #endregion
    }
}