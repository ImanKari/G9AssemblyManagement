using G9AssemblyManagement.Core;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for general tools
    /// </summary>
    public class G9CGeneralTools
    {
        /// <summary>
        ///     Method to convert byte size to another size
        /// </summary>
        /// <param name="byteSize">Specifies the byte size</param>
        /// <param name="unit">Specifies a unit for converting</param>
        /// <returns>Converted size value</returns>
        public decimal ConvertByteSizeToAnotherSize(long byteSize, G9ESizeUnits unit)
        {
            return G9CToolsHandler.ConvertByteSizeToAnotherSize(byteSize, unit);
        }
    }
}