using G9AssemblyManagement.Helper;

namespace G9AssemblyManagement
{
    /// <summary>
    ///     Helper class to manage assembly items
    /// </summary>
    public static class G9CAssemblyManagement
    {
        #region ### Methods ###

        /// <summary>
        ///     Constructor
        /// </summary>
        static G9CAssemblyManagement()
        {
            Types = new G9CTypesHelper();
            Instances = new G9CInstanceHelper();
        }

        #endregion

        #region ### Fields And Properties

        /// <summary>
        ///     Access to type utilities
        /// </summary>
        public static readonly G9CTypesHelper Types;

        /// <summary>
        ///     Access to instance utilities
        /// </summary>
        public static readonly G9CInstanceHelper Instances;

        #endregion
    }
}