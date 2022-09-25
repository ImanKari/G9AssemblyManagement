using System;
using System.IO;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    public class G9CInputOutputTools
    {


        /// <inheritdoc cref="G9CInputOutputHandler.CheckDirectoryPathValidation" />
        public G9EPatchCheckResult CheckDirectoryPathValidation(string directoryPath, bool checkDirectoryDrive,
            bool checkDirectoryExist)
        {
            return G9CInputOutputHandler.CheckDirectoryPathValidation(directoryPath, checkDirectoryDrive,
                checkDirectoryExist);
        }

        /// <inheritdoc cref="G9CInputOutputHandler.CheckFilePathValidation" />
        public G9EPatchCheckResult CheckFilePathValidation(string filePath, bool checkFileDrive,
            bool checkFileExist)
        {
            return G9CInputOutputHandler.CheckFilePathValidation(filePath, checkFileDrive,
                checkFileExist);
        }

        /// <inheritdoc cref="G9CInputOutputHandler.WaitForAccessToFile" />
        public void WaitForAccessToFile(string fullPath, Action<FileStream> onAvailableAccess, FileMode fileMode,
            FileAccess fileAccess, FileShare fileShare = FileShare.None, ushort countOfTrying = 9,
            ushort gapBetweenEachTry = 99)
        {
            G9CInputOutputHandler.WaitForAccessToFile(fullPath, onAvailableAccess, fileMode, fileAccess, fileShare,
                countOfTrying, gapBetweenEachTry);
        }
    }
}