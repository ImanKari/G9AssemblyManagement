using G9AssemblyManagement.Helper;

namespace G9AssemblyManagement
{
    /// <summary>
    ///     Helper class to manage assembly items
    /// </summary>
    public static class G9CAssemblyManagement
    {

        #region ### Fields And Properties

        /// <summary>
        ///     Access to type utilities
        /// </summary>
        public static readonly G9CTypesHelper Types = new G9CTypesHelper();

        /// <summary>
        ///     Access to instance utilities
        /// </summary>
        public static readonly G9CInstanceHelper Instances = new G9CInstanceHelper();

        /// <summary>
        ///     Access to reflection utilities
        /// </summary>
        public static readonly G9CReflectionHelper Reflection = new G9CReflectionHelper();

        #endregion
    }
}