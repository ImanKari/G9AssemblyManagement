using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using G9AssemblyManagement.Core.Class.Hashing;
using G9AssemblyManagement.Core.Cryptography;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Core
{
    /// <summary>
    ///     Implementation of the Advanced Encryption Standard (AES) and the other cryptographies.
    /// </summary>
    internal static class G9CCryptographyHandler
    {
        public static readonly Dictionary<G9EHashAlgorithm, byte> AlgorithmSizeCollection =
            new Dictionary<G9EHashAlgorithm, byte>
            {
                { G9EHashAlgorithm.MD5, 16 },
                { G9EHashAlgorithm.SHA1, 20 },
                { G9EHashAlgorithm.SHA256, 32 },
                { G9EHashAlgorithm.SHA384, 48 },
                { G9EHashAlgorithm.SHA512, 64 },
                { G9EHashAlgorithm.CRC32, 4 }
            };

        private static void SetDefaultEncoder(ref Encoding customEncoder)
        {
            if (customEncoder == null)
                customEncoder = Encoding.UTF8;
        }

        #region AES Cryptography

        /// <summary>
        ///     Method to get a prepared object of cryptography by specified custom configs.
        /// </summary>
        /// <param name="privateKey">Specifies custom private key for encrypting.</param>
        /// <param name="ivKey">Specifies custom iv key for encrypting.</param>
        /// <param name="aesConfig">
        ///     Specifies the basis config of AES.
        ///     <para />
        ///     By default, the key size and key block are set to 128, the padding mode is set to 'PKCS7', and the cipher mode is
        ///     set to 'CBC.'
        /// </param>
        /// <param name="customEncoder">
        ///     Specifies the custom encoder if needed.
        ///     <para />
        ///     By default, it's 'UTF8'.
        /// </param>
        /// <returns>Prepared object of cryptography by specified custom configs.</returns>
        public static G9CAesCryptography InitAesCryptography(byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            return new G9CAesCryptography(privateKey, ivKey, aesConfig, customEncoder);
        }

        #endregion

        #region Hashing Methods

        /// <summary>
        ///     Method to get a hash algorithm size
        /// </summary>
        /// <param name="hashAlgorithm">Specifies the algorithm of hashing.</param>
        public static byte GetHashAlgorithmSize(G9EHashAlgorithm hashAlgorithm)
        {
            return AlgorithmSizeCollection[hashAlgorithm];
        }

        public static HashAlgorithm CreateHashAlgorithm(G9EHashAlgorithm hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case G9EHashAlgorithm.MD5:
                    return MD5.Create();
                case G9EHashAlgorithm.SHA1:
                    return SHA1.Create();
                case G9EHashAlgorithm.SHA256:
                    return SHA256.Create();
                case G9EHashAlgorithm.SHA384:
                    return SHA384.Create();
                case G9EHashAlgorithm.SHA512:
                    return SHA512.Create();
                case G9EHashAlgorithm.CRC32:
                    return SHA512.Create();
                default:
                    throw new ArgumentOutOfRangeException(nameof(hashAlgorithm), hashAlgorithm, null);
            }
        }

        /// <summary>
        ///     Method to generate a hash byte array from a byte array.
        /// </summary>
        /// <param name="hashAlgorithm">Specifies the algorithm of hashing.</param>
        /// <param name="bytesForHashing">Specifies a byte array for hashing</param>
        /// <returns>Return hash byte array from normal byte array</returns>
        public static byte[] CreateHash(G9EHashAlgorithm hashAlgorithm, byte[] bytesForHashing)
        {
            if (hashAlgorithm == G9EHashAlgorithm.CRC32) return new G9CCrc32().ComputeHash(bytesForHashing);

            using (var hashAlgorithmParser = HashAlgorithm.Create(hashAlgorithm.ToString()))
            {
                if (hashAlgorithmParser == null)
                    throw new ArgumentException(
                        $"The specified algorithm for hashing ({hashAlgorithm}) isn't supported.",
                        nameof(hashAlgorithm));
#if NETSTANDARD2_1
                Span<byte> hashBytes = stackalloc byte[AlgorithmSizeCollection[hashAlgorithm]];
                hashAlgorithmParser.TryComputeHash(bytesForHashing, hashBytes, out var written);
                if (written != hashBytes.Length)
                    throw new OverflowException();
                return hashBytes.ToArray();
#else
                return hashAlgorithmParser.ComputeHash(bytesForHashing);
#endif
            }
        }

        /// <summary>
        ///     Method to generate a hash text from a normal text.
        /// </summary>
        /// <param name="hashAlgorithm">Specifies the algorithm of hashing.</param>
        /// <param name="text">Species a text for hashing</param>
        /// <param name="upperCase">Specifies that the hashed code must be in upper-case or lower-case format.</param>
        /// <param name="customEncoder">
        ///     Specifies the custom encoder if needed.
        ///     <para />
        ///     By default, it's 'UTF8'.
        /// </param>
        /// <returns>Return hash text from normal text</returns>
        public static string CreateHash(G9EHashAlgorithm hashAlgorithm, string text, bool upperCase = false,
            Encoding customEncoder = null)
        {
            SetDefaultEncoder(ref customEncoder);
            var convertType = upperCase ? "X2" : "x2";

#if NETSTANDARD2_1
            Span<byte> hashBytes = CreateHash(hashAlgorithm, customEncoder.GetBytes(text));
            Span<char> stringBuffer = stackalloc char[AlgorithmSizeCollection[hashAlgorithm] * 2];
            for (var i = 0; i < hashBytes.Length; i++)
                hashBytes[i].TryFormat(stringBuffer.Slice(2 * i), out _, convertType);
            return new string(stringBuffer);
#else
            var hashBytes = CreateHash(hashAlgorithm, customEncoder.GetBytes(text));
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
                sb.Append(t.ToString(convertType));

            return sb.ToString();
#endif
        }

        #endregion
    }
}