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
        public static readonly G9CReflectionTools ReflectionTools = new G9CReflectionTools();

        /// <summary>
        ///     Access to general tools
        /// </summary>
        public static readonly G9CGeneralTools GeneralTools = new G9CGeneralTools();

        /// <summary>
        ///     Access to cryptography tools
        /// </summary>
        public static readonly G9CCryptography CryptographyTools = new G9CCryptography();

        /// <summary>
        ///     Access to performance tools
        /// </summary>
        public static readonly G9CPerformanceTools PerformanceTools = new G9CPerformanceTools();

        /// <summary>
        ///     Access to input and output tools
        /// </summary>
        public static readonly G9CInputOutputTools InputOutputTools = new G9CInputOutputTools();

        #endregion
    }
}