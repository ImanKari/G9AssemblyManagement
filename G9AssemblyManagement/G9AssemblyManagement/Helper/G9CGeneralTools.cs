using System.Reflection;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for general tools
    /// </summary>
    public class G9CGeneralTools
    {
        /// <inheritdoc cref="G9CToolsHandler.ConvertByteSizeToAnotherSize" />
        public decimal ConvertByteSizeToAnotherSize(long byteSize, G9ESizeUnits unit)
        {
            return G9CToolsHandler.ConvertByteSizeToAnotherSize(byteSize, unit);
        }

        /// <inheritdoc cref="G9CToolsHandler.GetAssemblyVersion" />
        public string GetAssemblyVersion(Assembly targetAssembly)
        {
            return G9CToolsHandler.GetAssemblyVersion(targetAssembly);
        }
    }
}