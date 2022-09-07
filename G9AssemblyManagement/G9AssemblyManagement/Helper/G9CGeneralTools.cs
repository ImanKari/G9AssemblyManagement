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

        /// <inheritdoc cref="G9CToolsHandler.CheckDirectoryPathValidation" />
        public G9EPatchCheckResult CheckDirectoryPathValidation(string directoryPath, bool checkDirectoryDrive,
            bool checkDirectoryExist)
        {
            return G9CToolsHandler.CheckDirectoryPathValidation(directoryPath, checkDirectoryDrive,
                checkDirectoryExist);
        }

        /// <inheritdoc cref="G9CToolsHandler.CheckFilePathValidation" />
        public G9EPatchCheckResult CheckFilePathValidation(string filePath, bool checkFileDrive,
            bool checkFileExist)
        {
            return G9CToolsHandler.CheckFilePathValidation(filePath, checkFileDrive,
                checkFileExist);
        }
    }
}