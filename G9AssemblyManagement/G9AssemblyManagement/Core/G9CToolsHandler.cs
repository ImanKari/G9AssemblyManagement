using System;
using System.Reflection;
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
        ///     Method to get the current version of the assembly.
        /// </summary>
        /// <param name="targetAssembly">cc</param>
        /// <returns>The current version of the assembly</returns>
        public static string GetAssemblyVersion(Assembly targetAssembly)
        {
            if (targetAssembly == null)
            {
                targetAssembly = Assembly.GetExecutingAssembly();
            }
            return string.IsNullOrEmpty(targetAssembly.GetName().Version?.ToString())
                ? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0.0"
                : targetAssembly.GetName().Version?.ToString();
        }
    }
}