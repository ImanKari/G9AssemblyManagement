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
        ///     Access to type handlers
        /// </summary>
        public static readonly G9CTypesHelper TypeHandlers = new G9CTypesHelper();

        /// <summary>
        ///     Access to instance handlers
        /// </summary>
        public static readonly G9CInstanceHelper InstanceHandlers = new G9CInstanceHelper();

        /// <summary> 
        ///     Access to reflection handlers
        /// </summary>
        public static readonly G9CReflectionHelper ReflectionHandlers = new G9CReflectionHelper();

        #endregion
    }
}