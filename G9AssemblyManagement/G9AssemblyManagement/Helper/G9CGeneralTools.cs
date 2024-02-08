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

        /// <summary>
        ///     Method to get the current version of the assembly.
        /// </summary>
        /// <returns>The current version of the assembly</returns>
        public string GetAssemblyVersion()
        {
            return string.IsNullOrEmpty(Assembly.GetExecutingAssembly().GetName().Version.ToString())
                ? Assembly.GetEntryAssembly()?.GetName().Version.ToString() ?? "0.0.0.0"
                : Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}