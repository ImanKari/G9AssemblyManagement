using System;
using System.IO;
using System.Linq;
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

        /// <summary>
        ///     Method to check the validation of directory path.
        /// </summary>
        /// <param name="directoryPath">Specifies a directory path.</param>
        /// <param name="checkDirectoryDrive">Specifies whether the directory drive must be checked or not.</param>
        /// <param name="checkDirectoryExist">Specifies whether the directory path in terms of existence must be checked or not.</param>
        /// <returns>Returns result as <see cref="G9EPatchCheckResult" /></returns>
        public static G9EPatchCheckResult CheckDirectoryPathValidation(string directoryPath, bool checkDirectoryDrive,
            bool checkDirectoryExist)
        {
            if (directoryPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                return G9EPatchCheckResult.PathNameIsIncorrect;

            if (checkDirectoryDrive)
            {
                var root = Path.GetPathRoot(directoryPath);
                if (root == "\\")
                    root = Path.GetPathRoot(Path.GetFullPath(directoryPath));
                if (Directory.GetLogicalDrives().All(s => s != root)) return G9EPatchCheckResult.PathDriveIsIncorrect;
            }

            if (!checkDirectoryExist) return G9EPatchCheckResult.Correct;
            return !Directory.Exists(directoryPath)
                ? G9EPatchCheckResult.PathExistenceIsIncorrect
                : G9EPatchCheckResult.Correct;
        }

        /// <summary>
        ///     Method to check the validation of file path.
        /// </summary>
        /// <param name="filePath">Specifies a file path.</param>
        /// <param name="checkFileDrive">Specifies whether the file drive must be checked or not.</param>
        /// <param name="checkFileExist">Specifies whether the file path in terms of existence must be checked or not.</param>
        /// <returns>Returns result as <see cref="G9EPatchCheckResult" /></returns>
        public static G9EPatchCheckResult CheckFilePathValidation(string filePath, bool checkFileDrive,
            bool checkFileExist)
        {
            string fileFullPath = string.Empty;
            try
            {
                // Check the correction of path
                fileFullPath = Path.GetDirectoryName(Path.GetFullPath(filePath));
            }
            catch
            {
                return G9EPatchCheckResult.PathNameIsIncorrect;
            }

            var pathCheckResult = CheckDirectoryPathValidation(fileFullPath, checkFileDrive, checkFileExist);
            if (pathCheckResult != G9EPatchCheckResult.Correct)
                return pathCheckResult;

            // Check the correction of file
            var fileName = Path.GetFileName(filePath);
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return G9EPatchCheckResult.PathNameIsIncorrect;
            
            // Check file exist
            if (!checkFileExist) return G9EPatchCheckResult.Correct;
            return !File.Exists(filePath)
                ? G9EPatchCheckResult.PathExistenceIsIncorrect
                : G9EPatchCheckResult.Correct;
        }
    }
}