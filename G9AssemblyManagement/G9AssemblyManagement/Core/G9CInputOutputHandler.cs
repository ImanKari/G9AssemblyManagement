using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Core
{
    internal static class G9CInputOutputHandler
    {
        /// <summary>
        ///     Method to check the validation of directory path.
        /// </summary>
        /// <param name="directoryPath">Specifies a directory path.</param>
        /// <param name="checkDirectoryDrive">Specifies whether the directory drive must be checked or not.</param>
        /// <param name="checkDirectoryExist">Specifies whether the directory path in terms of existence must be checked or not.</param>
        /// <param name="throwException">Specifies if the validation isn't correct, an exception must be thrown or not.</param>
        /// <returns>Returns result as <see cref="G9EPatchCheckResult" /></returns>
        public static G9EPatchCheckResult CheckDirectoryPathValidation(string directoryPath, bool checkDirectoryDrive,
            bool checkDirectoryExist, bool throwException)
        {
            if (directoryPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                if (throwException)
                    PathValidationToArgumentException(G9EPatchCheckResult.PathNameIsIncorrect, directoryPath,
                        nameof(directoryPath));
                else
                    return G9EPatchCheckResult.PathNameIsIncorrect;

            if (checkDirectoryDrive)
            {
                var root = Path.GetPathRoot(directoryPath);
                if (root == "\\")
                    root = Path.GetPathRoot(Path.GetFullPath(directoryPath));
                if (Directory.GetLogicalDrives().All(s => s != root)) return G9EPatchCheckResult.PathDriveIsIncorrect;
            }

            if (!checkDirectoryExist) return G9EPatchCheckResult.Correct;

            if (!Directory.Exists(directoryPath))
                if (throwException)
                    PathValidationToArgumentException(G9EPatchCheckResult.PathExistenceIsIncorrect, directoryPath,
                        nameof(directoryPath));
                else
                    return G9EPatchCheckResult.PathExistenceIsIncorrect;
            else
                return G9EPatchCheckResult.Correct;

            return G9EPatchCheckResult.Correct;
        }

        /// <summary>
        ///     Method to check the validation of file path.
        /// </summary>
        /// <param name="filePath">Specifies a file path.</param>
        /// <param name="checkFileDrive">Specifies whether the file drive must be checked or not.</param>
        /// <param name="checkFileExist">Specifies whether the file path in terms of existence must be checked or not.</param>
        /// <param name="throwException">Specifies if the validation isn't correct, an exception must be thrown or not.</param>
        /// <returns>Returns result as <see cref="G9EPatchCheckResult" /></returns>
        public static G9EPatchCheckResult CheckFilePathValidation(string filePath, bool checkFileDrive,
            bool checkFileExist, bool throwException)
        {
            string fileFullPath;
            try
            {
                // Check the correction of path
                fileFullPath = Path.GetDirectoryName(Path.GetFullPath(filePath));
            }
            catch
            {
                return G9EPatchCheckResult.PathNameIsIncorrect;
            }

            var pathCheckResult =
                CheckDirectoryPathValidation(fileFullPath, checkFileDrive, checkFileExist, throwException);
            if (pathCheckResult != G9EPatchCheckResult.Correct)
                if (throwException)
                    PathValidationToArgumentException(pathCheckResult, filePath, nameof(filePath));
                else
                    return pathCheckResult;

            // Check the correction of file
            var fileName = Path.GetFileName(filePath);
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                if (throwException)
                    PathValidationToArgumentException(G9EPatchCheckResult.PathNameIsIncorrect, filePath,
                        nameof(filePath));
                else
                    return G9EPatchCheckResult.PathNameIsIncorrect;

            // Check file exist
            if (!checkFileExist) return G9EPatchCheckResult.Correct;
            if (!File.Exists(filePath))
                if (throwException)
                    PathValidationToArgumentException(G9EPatchCheckResult.PathExistenceIsIncorrect, filePath,
                        nameof(filePath));
                else
                    return G9EPatchCheckResult.PathExistenceIsIncorrect;
            return G9EPatchCheckResult.Correct;
        }

        /// <summary>
        ///     Method to throw an exception for path validation if the validation result isn't '
        ///     <see cref="G9EPatchCheckResult.Correct" />'.
        /// </summary>
        /// <param name="pathValidation">Specifies validation result</param>
        /// <param name="path">Specified path</param>
        /// <param name="paramName">Specified parameter name</param>
        private static void PathValidationToArgumentException(G9EPatchCheckResult pathValidation, string path,
            string paramName)
        {
            switch (pathValidation)
            {
                case G9EPatchCheckResult.PathNameIsIncorrect:
                    throw new ArgumentException(
                        $"The specified path '{path}' for the specified parameter '{paramName}' is incorrect as a path. The program can't use it as a path with these characters.",
                        nameof(paramName));
                case G9EPatchCheckResult.PathDriveIsIncorrect:
                    throw new ArgumentException(
                        $"The specified path '{path}' for the specified parameter '{paramName}' is incorrect as a path. The program can't use it as a path with this selected drive.(this drive does not exist)",
                        nameof(paramName));
                case G9EPatchCheckResult.PathExistenceIsIncorrect:
                    throw new ArgumentException(
                        $"The specified path '{path}' for the specified parameter '{paramName}' is incorrect because it does not exist. The program can't use it as a path without creating it.",
                        nameof(paramName));
                case G9EPatchCheckResult.Correct:
                default:
                    break;
            }
        }

        /// <summary>
        ///     Method to make a wait system for access to the file with specified access and custom options.
        /// </summary>
        /// <param name="fullPath">Specifies the full path of desired file.</param>
        /// <param name="onAvailableAccess">
        ///     Specifies a callback for invoking.
        ///     <para />
        ///     The specified callback invokes when the desired specified access would be available. In addition, it has a
        ///     parameter that provides a usable opened file stream on your specified file.
        /// </param>
        /// <param name="fileMode">Specifies how the operating system should open a file.</param>
        /// <param name="fileAccess">Defines constants for read, write, or read/write access to a file.</param>
        /// <param name="fileShare">
        ///     Contains constants for controlling the kind of access other
        ///     <see cref="T:System.IO.FileStream" /> objects can have to the same file.
        /// </param>
        /// <param name="countOfTrying">Specifies how many times must be tried.</param>
        /// <param name="gapBetweenEachTry">Specifies how much time must be considered between each try in milliseconds.</param>
        /// <exception cref="Exception">
        ///     Suppose the method can't find the specified file accessible according to options. An
        ///     exception related to file access is thrown.
        /// </exception>
        public static void WaitForAccessToFile(string fullPath, Action<FileStream> onAvailableAccess, FileMode fileMode,
            FileAccess fileAccess, FileShare fileShare = FileShare.None, ushort countOfTrying = 9,
            ushort gapBetweenEachTry = 99)
        {
            ushort numTries = 0;
            var isActionException = false;
            while (true)
                try
                {
                    // Attempt to open the file exclusively.
                    using (var fs = new FileStream(fullPath,
                               fileMode, fileAccess,
                               fileShare))
                    {
                        try
                        {
                            onAvailableAccess(fs);
                        }
                        catch
                        {
                            isActionException = true;
                            throw;
                        }

                        // If we got this far the file is ready
                        break;
                    }
                }
                catch
                {
                    if (isActionException || numTries++ > countOfTrying) throw;

                    // Wait for the lock to be released
                    Thread.Sleep(gapBetweenEachTry);
                }
        }


        /// <summary>
        ///     Method to get executable directory.
        /// </summary>
        /// <returns>Return executable directory</returns>
        public static string GetExecutableDirectory()
        {
            return
#if !NETSTANDARD1_3_OR_GREATER && !NETCOREAPP
                AppDomain.CurrentDomain.BaseDirectory;
#else
                AppContext.BaseDirectory;
#endif
        }

        /// <summary>
        ///     Method to get current directory.
        /// </summary>
        /// <returns>Method to get current directory</returns>
        public static string GetBaseCurrentDirectory()
        {
            return Environment.CurrentDirectory;
        }

        /// <summary>
        ///     Method to get a stream from a file that is an embedded resource.
        /// </summary>
        /// <param name="assemblyHasEmbeddedResource">Specifies the target assembly for catching the file.</param>
        /// <param name="embeddedResourceAddress">Specifies the address if embedded resource file.</param>
        /// <returns>Stream</returns>
        /// <exception cref="InvalidOperationException">
        ///     If the embedded file isn't available, throw an exception: The specified
        ///     embedded resource file is not found in the address...
        /// </exception>
        public static Stream EmbeddedResourceGetStreamFromFile(Assembly assemblyHasEmbeddedResource,
            string embeddedResourceAddress)
        {
            // ReSharper disable once PossibleNullReferenceException
            embeddedResourceAddress = $"{assemblyHasEmbeddedResource.FullName.Split(',')[0]}.{embeddedResourceAddress}";

            var resource = assemblyHasEmbeddedResource.GetManifestResourceStream(embeddedResourceAddress);
            return resource ?? throw new InvalidOperationException(
                $"The specified embedded resource file is not found in the address: '{embeddedResourceAddress}'.");
        }

        /// <summary>
        ///     Method to copy multiple files from embedded resource addresses to a target directory.
        /// </summary>
        /// <param name="assemblyHasEmbeddedResource">Specifies the target assembly for catching the file.</param>
        /// <param name="embeddedResourcesPath">Specifies the embedded resource path array.</param>
        /// <param name="customProcess">
        ///     Specifies a custom process for copying. By that, you can make the needed changes in the
        ///     file before copying.
        /// </param>
        /// <param name="createPathIfNotExist">
        ///     Specifies if the target path (for copying) does not exist, It must be created or
        ///     not.
        /// </param>
        /// <param name="fileMode">Specifies the file mode for creating a copy</param>
        /// <param name="fileAccess">Specifies the file access for creating.</param>
        /// <param name="fileShare">Specifies the file share for creating.</param>
        /// <exception cref="InvalidOperationException">
        ///     If the embedded file isn't available, throw an exception: The specified
        ///     embedded resource file is not found in the address...
        /// </exception>
        public static void EmbeddedResourceCopyFilesToPath(
            Assembly assemblyHasEmbeddedResource,
            IList<G9DtEmbeddedResourceFile> embeddedResourcesPath,
            Func<string, byte[], byte[]> customProcess,
            bool createPathIfNotExist = false,
            FileMode fileMode = FileMode.CreateNew,
            FileAccess fileAccess = FileAccess.Write,
            FileShare fileShare = FileShare.Write)
        {
            // Loop for copying all the specified embedded resource
            foreach (var emStructure in embeddedResourcesPath)
            {
                var directoryPath = Path.GetDirectoryName(emStructure.TargetFilePath);
                // Check path validation
                _ = CheckDirectoryPathValidation(directoryPath, true, !createPathIfNotExist, true);

                // Create directory if needed
                if (createPathIfNotExist && !Directory.Exists(directoryPath))
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Directory.CreateDirectory(directoryPath);

                using (var resource = EmbeddedResourceGetStreamFromFile(assemblyHasEmbeddedResource,
                           emStructure.EmbeddedResourceFilePath))
                {
                    // Check validation of specified embedded resource in the path
                    if (resource == null)
                        throw new InvalidOperationException(
                            $"The specified embedded resource file is not found in the address: '{embeddedResourcesPath}'.");

                    // Read the data of embedded resource
                    var dataBytes = new byte[resource.Length];
                    _ = resource.Read(dataBytes, 0, dataBytes.Length);

                    // If a custom process exists, the core uses it.
                    if (customProcess != null)
                        dataBytes = customProcess(emStructure.EmbeddedResourceFilePath, dataBytes);

                    // Write the embedded resource in desired path.
                    WaitForAccessToFile(emStructure.TargetFilePath, fs => { fs.Write(dataBytes, 0, dataBytes.Length); },
                        fileMode, fileAccess, fileShare);
                }
            }
        }
    }
}