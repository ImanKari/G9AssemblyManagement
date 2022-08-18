using G9AssemblyManagement.Helper;

namespace G9AssemblyManagement
{
    /// <summary>
    ///     Helper class to manage assembly items
    /// </summary>
    public static class G9Assembly
    {
        #region ### Fields And Properties

        /// <summary>
        ///     Access to type tools
        /// </summary>
        public static readonly G9CTypeTools TypeTools = new G9CTypeTools();

        /// <summary>
        ///     Access to instance tools
        /// </summary>
        public static readonly G9CInstanceTools InstanceTools = new G9CInstanceTools();

        /// <summary>
        ///     Access to object tools (and reflections)
        /// </summary>
        public static readonly G9CObjectAndReflectionTools ObjectAndReflectionTools = new G9CObjectAndReflectionTools();

        /// <summary>
        ///     Access to general tools
        /// </summary>
        public static readonly G9CGeneralTools GeneralTools = new G9CGeneralTools();

        #endregion
    }
}