using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    public class G9CInputOutputTools
    {
        /// <inheritdoc cref="G9CInputOutputHandler.CheckDirectoryPathValidation" />
        public G9EPatchCheckResult CheckDirectoryPathValidation(string directoryPath, bool checkDirectoryDrive,
            bool checkDirectoryExist, bool throwException)
        {
            return G9CInputOutputHandler.CheckDirectoryPathValidation(directoryPath, checkDirectoryDrive,
                checkDirectoryExist, throwException);
        }

        /// <inheritdoc cref="G9CInputOutputHandler.CheckFilePathValidation" />
        public G9EPatchCheckResult CheckFilePathValidation(string filePath, bool checkFileDrive,
            bool checkFileExist, bool throwException)
        {
            return G9CInputOutputHandler.CheckFilePathValidation(filePath, checkFileDrive,
                checkFileExist, throwException);
        }


        /// <inheritdoc cref="G9CInputOutputHandler.WaitForAccessToFile" />
        public void WaitForAccessToFile(string fullPath, Action<FileStream> onAvailableAccess, FileMode fileMode,
            FileAccess fileAccess, FileShare fileShare = FileShare.None, ushort countOfTrying = 9,
            ushort gapBetweenEachTry = 99)
        {
            G9CInputOutputHandler.WaitForAccessToFile(fullPath, onAvailableAccess, fileMode, fileAccess, fileShare,
                countOfTrying, gapBetweenEachTry);
        }

        /// <inheritdoc cref="G9CInputOutputHandler.GetExecutableDirectory" />
        public string GetExecutableDirectory()
        {
            return G9CInputOutputHandler.GetExecutableDirectory();
        }

        /// <inheritdoc cref="G9CInputOutputHandler.GetBaseCurrentDirectory" />
        public string GetBaseCurrentDirectory()
        {
            return G9CInputOutputHandler.GetBaseCurrentDirectory();
        }

        /// <inheritdoc cref="G9CInputOutputHandler.EmbeddedResourceGetStreamFromFile" />
        public Stream EmbeddedResourceGetStreamFromFile(Assembly assemblyHasEmbeddedResource, string embeddedResourceAddress)
        {
            return G9CInputOutputHandler.EmbeddedResourceGetStreamFromFile(assemblyHasEmbeddedResource, embeddedResourceAddress);
        }

        /// <inheritdoc cref="G9CInputOutputHandler.EmbeddedResourceCopyFilesToPath" />
        public void EmbeddedResourceCopyFilesToPath(
            Assembly assemblyHasEmbeddedResource,
            IList<G9DtEmbeddedResourceFile> embeddedResourcesPath,
            Func<string, byte[], byte[]> customProcess,
            bool createPathIfNotExist = false,
            FileMode fileMode = FileMode.CreateNew,
            FileAccess fileAccess = FileAccess.Write,
            FileShare fileShare = FileShare.Write)
        {
            G9CInputOutputHandler.EmbeddedResourceCopyFilesToPath(assemblyHasEmbeddedResource, embeddedResourcesPath, customProcess, createPathIfNotExist, fileMode, fileAccess, fileShare);
        }
    }
}