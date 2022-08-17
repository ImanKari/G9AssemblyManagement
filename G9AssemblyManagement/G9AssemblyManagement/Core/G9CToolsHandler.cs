using System.Security.Cryptography;
using System.Text;
#if NETSTANDARD2_1
using System;
#endif

namespace G9AssemblyManagement.Core
{
    internal static class G9CToolsHandler
    {
        /// <summary>
        ///     Generate MD5 from text
        /// </summary>
        /// <param name="text">Specify text</param>
        /// <returns>Return MD5 from text</returns>
        public static string CreateMd5(string text)
        {
#if NETSTANDARD2_1
            using var md5 = MD5.Create();
            var encoding = Encoding.ASCII;
            var data = encoding.GetBytes(text);

            Span<byte> hashBytes = stackalloc byte[16];
            md5.TryComputeHash(data, hashBytes, out var written);
            if (written != hashBytes.Length)
                throw new OverflowException();


            Span<char> stringBuffer = stackalloc char[32];
            for (var i = 0; i < hashBytes.Length; i++)
                hashBytes[i].TryFormat(stringBuffer.Slice(2 * i), out _, "x2");
            return new string(stringBuffer).ToLower();
#else
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(text);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                    sb.Append(t.ToString("X2"));

                return sb.ToString().ToLower();
            }
#endif
        }
    }
}