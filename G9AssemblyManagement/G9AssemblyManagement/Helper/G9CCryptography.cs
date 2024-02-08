using System.Text;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.Core.Cryptography;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Helper class for the Advanced Encryption Standard (AES) and the other cryptographies.
    /// </summary>
    public class G9CCryptography
    {
        #region Hashing methods

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

        /// <inheritdoc cref="G9CCryptographyHandler.GetHashAlgorithmSize" />
        public byte GetHashAlgorithmSize(G9EHashAlgorithm hashAlgorithm)
        {
            return G9CCryptographyHandler.GetHashAlgorithmSize(hashAlgorithm);
        }

        #endregion

        #region AES Cryptography

        /// <inheritdoc cref="G9CCryptographyHandler.InitAesCryptography" />
        public G9CAesCryptography InitAesCryptography(string privateKey, string ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            customEncoder = customEncoder ?? Encoding.UTF8;
            return InitAesCryptography(customEncoder.GetBytes(privateKey), customEncoder.GetBytes(ivKey), aesConfig,
                customEncoder);
        }

        /// <inheritdoc cref="G9CCryptographyHandler.InitAesCryptography" />
        public G9CAesCryptography InitAesCryptography(byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            return G9CCryptographyHandler.InitAesCryptography(privateKey, ivKey, aesConfig, customEncoder);
        }

        #endregion
    }
}