using System;
using System.IO;
using System.Linq;
using System.Threading;
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
            var fileFullPath = string.Empty;
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

        /// <summary>
        ///     Method to make a wait system for access to the file with specified access and custom options.
        /// </summary>
        /// <param name="fullPath">Specifies the full path of desired file.</param>
        /// <param name="onAvailableAccess">
        ///     Specifies a callback for invoking.
        ///     <para />
        ///     The specified callback invokes when the desired specified access would be available. In addition, it has a
        ///     parameter "A" that provides a usable opened file stream on your specified file.
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
    }
}