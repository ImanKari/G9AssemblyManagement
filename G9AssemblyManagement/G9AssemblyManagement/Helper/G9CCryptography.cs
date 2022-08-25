using System;
using System.Text;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for the Advanced Encryption Standard (AES) and the other cryptographies.
    /// </summary>
    public class G9CCryptography
    {
        /// <summary>
        ///     Generate hash text from text
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
        // ReSharper disable once InconsistentNaming
        public string StringToCustomHash(G9EHashAlgorithm hashAlgorithm, string text, bool upperCase = false,
            Encoding customEncoder = null)
        {
            return G9CCryptographyHandler.CreateHash(hashAlgorithm, text, upperCase, customEncoder);
        }

        /// <summary>
        ///     Method to generate a hash byte array from a byte array.
        /// </summary>
        /// <param name="hashAlgorithm">Specifies the algorithm of hashing.</param>
        /// <param name="bytesForHashing">Specifies a byte array for hashing</param>
        /// <returns>Return hash byte array from normal byte array</returns>
        // ReSharper disable once InconsistentNaming
        public byte[] BytesToCustomHash(G9EHashAlgorithm hashAlgorithm, byte[] bytesForHashing)
        {
            return G9CCryptographyHandler.CreateHash(hashAlgorithm, bytesForHashing);
        }

        /// <summary>
        ///     Method to get a hash algorithm size
        /// </summary>
        /// <param name="hashAlgorithm">Specifies the algorithm of hashing.</param>
        public byte GetHashAlgorithmSize(G9EHashAlgorithm hashAlgorithm)
        {
            return G9CCryptographyHandler.GetHashAlgorithmSize(hashAlgorithm);
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
        public string AesEncryptString(string plainText, string privateKey, string ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            return G9CCryptographyHandler.AesEncryptString(plainText, privateKey, ivKey, aesConfig, customEncoder);
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
        public string AesDecryptString(string cipherText, string privateKey, string ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            return G9CCryptographyHandler.AesDecryptString(cipherText, privateKey, ivKey, aesConfig, customEncoder);
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
        public byte[] AesEncryptByteArray(byte[] byteArray, byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default)
        {
            return G9CCryptographyHandler.AesEncryptByteArray(byteArray, privateKey, ivKey, aesConfig);
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
        public byte[] AesDecryptByteArray(byte[] cipherByteArray, byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default)
        {
            return G9CCryptographyHandler.AesDecryptByteArray(cipherByteArray, privateKey, ivKey, aesConfig);
        }
    }
}