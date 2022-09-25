using System;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Core
{
    internal static class G9CToolsHandler
    {
        /// <summary>
        ///     Method to convert byte size to another size
        /// </summary>
        /// <param name="byteSize">Specifies the byte size</param>
        /// <param name="unit">Specifies a unit for converting</param>
        /// <returns>Converted size value</returns>
        public static decimal ConvertByteSizeToAnotherSize(long byteSize, G9ESizeUnits unit)
        {
            return byteSize / (decimal)Math.Pow(1024, (long)unit);
        }
    }
}