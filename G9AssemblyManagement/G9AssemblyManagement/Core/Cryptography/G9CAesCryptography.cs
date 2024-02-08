using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Core.Cryptography
{
    /// <summary>
    ///     Structure for handling cryptography.
    /// </summary>
    public class G9CAesCryptography : IDisposable
    {
        #region Fields And Properties

        /// <summary>
        ///     Specifies encoder
        /// </summary>
        private readonly Encoding _encoder;

        /// <summary>
        ///     Store aes config
        /// </summary>
        private G9DtAESConfig _aesConfig;

        /// <summary>
        ///     Store private key
        /// </summary>
        private byte[] _privateKey;

        /// <summary>
        ///     Store iv key
        /// </summary>
        private byte[] _ivKey;

        /// <summary>
        ///     Store the created Aes object
        /// </summary>
        private Aes _aesObject;

        /// <summary>
        ///     Store the created encryptor
        /// </summary>
        private ICryptoTransform _encryptor;

        /// <summary>
        ///     Store the created decryptor
        /// </summary>
        private ICryptoTransform _decryptor;

        /// <summary>
        ///     Lock object
        /// </summary>
        private static readonly object EncryptLockObject = new object();

        /// <summary>
        ///     Lock object
        /// </summary>
        private static readonly object DecryptLockObject = new object();

        #endregion

        #region Methods

        /// <summary>
        ///     Constructor - Initialize requirements
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
        public G9CAesCryptography(byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default, Encoding customEncoder = null)
        {
            _encoder = customEncoder ?? Encoding.UTF8;
            FirstInit(privateKey, ivKey, aesConfig);
        }


        private void FirstInit(byte[] privateKey, byte[] ivKey,
            G9DtAESConfig aesConfig = default)
        {
            // Set config
            _aesConfig = Equals(aesConfig, default(G9DtAESConfig))
                ? new G9DtAESConfig(PaddingMode.PKCS7)
                : aesConfig;

            // Set private and iv key
            _privateKey = privateKey;
            _ivKey = ivKey;

            // Fix keys if needed
            if (_aesConfig.EnableAutoFixKeySize)
                FixedKeyAndIvSize();

            // Set aes object
            _aesObject = AesBaseInitializer();

            // Set encryptor
            _encryptor = _aesObject.CreateEncryptor(_privateKey, _ivKey);

            // Set decryptor
            _decryptor = _aesObject.CreateDecryptor(_privateKey, _ivKey);
        }

        /// <summary>
        ///     Method to initialize the standard AES.
        /// </summary>
        /// <returns>A created object of AES by basis config</returns>
        private Aes AesBaseInitializer()
        {
            var customAes = Aes.Create();
            customAes.KeySize = _aesConfig.KeySize;
            customAes.BlockSize = _aesConfig.BlockSize;
            customAes.Padding = _aesConfig.PaddingMode;
            customAes.Mode = _aesConfig.CipherMode;
            return customAes;
        }

        /// <summary>
        ///     Method to encrypt a normal string to cipher string.
        /// </summary>
        /// <param name="plainText">Specifies plain text for encrypting.</param>
        /// <returns>The encrypted text</returns>
        public string EncryptString(string plainText)
        {
            var result = EncryptByteArray(_encoder.GetBytes(plainText));
            return Convert.ToBase64String(result, 0, result.Length);
        }

        /// <summary>
        ///     Method to decrypt a cipher string to normal string.
        /// </summary>
        /// <param name="cipherText">Specifies cipher text for encrypting.</param>
        /// <returns>The decrypted text</returns>
        public string DecryptString(string cipherText)
        {
            return _encoder.GetString(DecryptByteArray(Convert.FromBase64String(cipherText)));
        }

        /// <summary>
        ///     Method to encrypt a normal byte array to cipher byte array.
        /// </summary>
        /// <param name="byteArray">Specifies normal byte array for encrypting.</param>
        /// <returns>The encrypted text</returns>
        public byte[] EncryptByteArray(byte[] byteArray)
        {
            byte[] data;
            lock (EncryptLockObject)
            {
                data = PerformCryptography(byteArray, _encryptor);
#if !NETSTANDARD2_1
                _encryptor?.Dispose();
                _encryptor = _aesObject.CreateEncryptor(_privateKey, _ivKey);
#endif
            }

            return data;
        }

        /// <summary>
        ///     Method to decrypt a cipher byte array to normal byte array.
        /// </summary>
        /// <param name="cipherByteArray">Specifies cipher byte array for encrypting.</param>
        /// <returns>The decrypted byte array</returns>
        public byte[] DecryptByteArray(byte[] cipherByteArray)
        {
            byte[] data;
            lock (DecryptLockObject)
            {
                data = PerformCryptography(cipherByteArray, _decryptor);
#if !NETSTANDARD2_1
                _decryptor?.Dispose();
                _decryptor = _aesObject.CreateDecryptor(_privateKey, _ivKey);
#endif
            }

            return data;
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
            {
                using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        ///     Method to fix the size of keys with an arbitrary process.
        /// </summary>
        private void FixedKeyAndIvSize()
        {
            var keyByteSize = _aesConfig.KeySize / 8;
            // ReSharper disable once RedundantAssignment
            var newKey = string.Empty;
#if NETSTANDARD2_1
            Span<byte> hashBytes =
                G9Assembly.CryptographyTools.BytesToCustomHash(G9EHashAlgorithm.SHA512,
                    _ivKey.Concat(_privateKey).ToArray());
            Span<char> stringBuffer =
                stackalloc char[G9CCryptographyHandler.AlgorithmSizeCollection[G9EHashAlgorithm.SHA512] * 2];
            for (var i = 0; i < hashBytes.Length; i++)
                hashBytes[i].TryFormat(stringBuffer.Slice(2 * i), out _, "x2");
            newKey = new string(stringBuffer);
#else
            var hashBytes =
                G9Assembly.CryptographyTools.BytesToCustomHash(G9EHashAlgorithm.SHA512,
                    _ivKey.Concat(_privateKey).ToArray());
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
                sb.Append(t.ToString("x2"));
            newKey = sb.ToString();
#endif

            _privateKey = Encoding.UTF8.GetBytes(newKey.Reverse().Take(keyByteSize).ToArray());
            _ivKey = Encoding.UTF8.GetBytes(newKey.Skip(_privateKey[0] % 9 + 9).Take(16).ToArray());
        }

        /// <inheritdoc />
        public void Dispose()
        {
#if !NET35
            _aesObject?.Dispose();
#endif
            _encryptor?.Dispose();
            _decryptor?.Dispose();
            _aesObject = null;
        }

        #endregion
    }
}