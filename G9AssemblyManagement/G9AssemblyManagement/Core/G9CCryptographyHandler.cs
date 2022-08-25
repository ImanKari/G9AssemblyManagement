using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using G9AssemblyManagement.Core.Class.Hashing;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Core
{
    /// <summary>
    ///     Implementation of the Advanced Encryption Standard (AES) and the other cryptographies.
    /// </summary>
    internal static class G9CCryptographyHandler
    {
        private static readonly Dictionary<G9EHashAlgorithm, byte> AlgorithmSizeCollection =
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
            if (hashAlgorithm == G9EHashAlgorithm.CRC32)
            {
                return new G9CCrc32().ComputeHash(bytesForHashing);
            }

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

            return sb.ToString().ToLower();
#endif
        }

        #endregion

        #region AES Cryptography

        /// <summary>
        ///     Method to initialize the standard AES.
        /// </summary>
        /// <param name="aesConfig">Specifies the basis config of AES.</param>
        /// <returns>A created object of AES by basis config</returns>
        private static Aes AesBaseInitializer(G9DtAESConfig aesConfig)
        {
            var customAes = Aes.Create();
            customAes.KeySize = aesConfig.KeySize;
            customAes.BlockSize = aesConfig.BlockSize;
            customAes.Padding = aesConfig.PaddingMode;
            customAes.Mode = aesConfig.CipherMode;
            return customAes;
        }

        /// <summary>
        ///     Method to encrypt a normal string to cipher string.
        /// </summary>
        /// <param name="plainText">Specifies plain text for encrypting.</param>
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
        /// <returns>The encrypted text</returns>
        public static string AesEncryptString(string plainText, string privateKey, string ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            if (customEncoder == null)
                customEncoder = Encoding.UTF8;


            var result = AesEncryptByteArray(customEncoder.GetBytes(plainText), customEncoder.GetBytes(privateKey),
                customEncoder.GetBytes(ivKey), aesConfig);
            return Convert.ToBase64String(result, 0, result.Length);
        }

        /// <summary>
        ///     Method to decrypt a cipher string to normal string.
        /// </summary>
        /// <param name="cipherText">Specifies cipher text for encrypting.</param>
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
        /// <returns>The decrypted text</returns>
        public static string AesDecryptString(string cipherText, string privateKey, string ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            if (customEncoder == null)
                customEncoder = Encoding.UTF8;

            return customEncoder.GetString(AesDecryptByteArray(Convert.FromBase64String(cipherText),
                customEncoder.GetBytes(privateKey),
                customEncoder.GetBytes(ivKey), aesConfig));
        }

        /// <summary>
        ///     Method to encrypt a normal byte array to cipher byte array.
        /// </summary>
        /// <param name="byteArray">Specifies normal byte array for encrypting.</param>
        /// <param name="privateKey">Specifies custom private key for encrypting.</param>
        /// <param name="ivKey">Specifies custom iv key for encrypting.</param>
        /// <param name="aesConfig">
        ///     Specifies the basis config of AES.
        ///     <para />
        ///     By default, the key size and key block are set to 128, the padding mode is set to 'PKCS7', and the cipher mode is
        ///     set to 'CBC.'
        /// </param>
        /// <returns>The encrypted text</returns>
        public static byte[] AesEncryptByteArray(byte[] byteArray, byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default)
        {
            if (Equals(aesConfig, default(G9DtAESConfig)))
                aesConfig = new G9DtAESConfig(PaddingMode.PKCS7);

            // Fixing the keys sizes
            if (aesConfig.EnableAutoFixKeySize)
                FixedKeyAndIvSize(ref privateKey, ref ivKey, aesConfig.KeySize);

            var customAes = AesBaseInitializer(aesConfig);
            using (var encryptor =
                   customAes.CreateEncryptor(privateKey, ivKey))
            {
                return PerformCryptography(byteArray, encryptor);
            }
        }

        /// <summary>
        ///     Method to decrypt a cipher byte array to normal byte array.
        /// </summary>
        /// <param name="cipherByteArray">Specifies cipher byte array for encrypting.</param>
        /// <param name="privateKey">Specifies custom private key for encrypting.</param>
        /// <param name="ivKey">Specifies custom iv key for encrypting.</param>
        /// <param name="aesConfig">
        ///     Specifies the basis config of AES.
        ///     <para />
        ///     By default, the key size and key block are set to 128, the padding mode is set to 'PKCS7', and the cipher mode is
        ///     set to 'CBC.'
        /// </param>
        /// <returns>The decrypted byte array</returns>
        public static byte[] AesDecryptByteArray(byte[] cipherByteArray, byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default)
        {
            if (Equals(aesConfig, default(G9DtAESConfig)))
                aesConfig = new G9DtAESConfig(PaddingMode.PKCS7);

            // Fixing the keys sizes
            if (aesConfig.EnableAutoFixKeySize)
                FixedKeyAndIvSize(ref privateKey, ref ivKey, aesConfig.KeySize);

            var customAes = AesBaseInitializer(aesConfig);
            using (var decryptor =
                   customAes.CreateDecryptor(privateKey, ivKey))
            {
                return PerformCryptography(cipherByteArray,
                    decryptor);
            }
        }

        /// <summary>
        ///     Helper method for encrypting and decrypting.
        /// </summary>
        /// <param name="data">Specifies data for encrypting or decrypting.</param>
        /// <param name="cryptoTransform">Access to encryptor or decryptor</param>
        /// <returns></returns>
        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Method to fix the size of keys with an arbitrary process.
        /// </summary>
        /// <param name="key">Specifies key</param>
        /// <param name="iv">Specifies Iv</param>
        /// <param name="keySize">Specifies the standard key size</param>
        private static void FixedKeyAndIvSize(ref byte[] key, ref byte[] iv, int keySize)
        {
            var keyByteSize = keySize / 8;
            if (key.Length > keyByteSize) key = key.Take(keyByteSize).ToArray();
            if (iv.Length > 16) iv = iv.Take(keyByteSize).ToArray();

            var stackKey = new Stack<byte>(key);
            var stackIv = new Stack<byte>(iv);

            var number = 9;
            while (true)
            {
                var breakTime = true;

                if (stackKey.Count < keyByteSize)
                {
                    stackKey.Push((byte)(number-- * 9));
                    stackKey.Push(stackKey.Pop());
                    breakTime = false;
                }

                if (stackIv.Count < 16)
                {
                    stackIv.Push((byte)(number-- * 9));
                    stackIv.Push(stackIv.Pop());
                    breakTime = false;
                }

                if (breakTime)
                    break;

                if (number < 3)
                    number = 9;
            }

            key = stackKey.ToArray();
            iv = stackIv.ToArray();
        }

        #endregion
    }
}