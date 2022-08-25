using System.Security.Cryptography;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     A data type structure for basis config of AES
    /// </summary>
    public struct G9DtAESConfig
    {
        /// <summary>
        ///     Specifies the padding mode of cryptography.
        /// </summary>
        public readonly PaddingMode PaddingMode;

        /// <summary>
        ///     Specifies the cipher mode of cryptography.
        /// </summary>
        public readonly CipherMode CipherMode;

        /// <summary>
        ///     Specifies the key size for cryptography.
        /// </summary>
        public readonly int KeySize;

        /// <summary>
        ///     Specifies the block size for cryptography.
        /// </summary>
        public readonly int BlockSize;

        /// <summary>
        ///     Specifies that if the key size isn't standard. The process must fix it or not. (With an arbitrary process)
        /// </summary>
        public readonly bool EnableAutoFixKeySize;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="paddingMode">Specifies the padding mode of cryptography.</param>
        /// <param name="cipherMode">Specifies the cipher mode of cryptography.</param>
        /// <param name="keySize">Specifies the key size for cryptography.</param>
        /// <param name="blockSize">Specifies the block size for cryptography.</param>
        /// <param name="enableAutoFixKeySize">
        ///     Specifies that if the key size isn't standard. The process must fix it or not. (With
        ///     an arbitrary process)
        /// </param>
        public G9DtAESConfig(PaddingMode paddingMode = PaddingMode.PKCS7,
            CipherMode cipherMode = CipherMode.CBC, int keySize = 128, int blockSize = 128,
            bool enableAutoFixKeySize = false)
        {
            EnableAutoFixKeySize = enableAutoFixKeySize;
            PaddingMode = paddingMode;
            CipherMode = cipherMode;
            KeySize = keySize;
            BlockSize = blockSize;
        }
    }
}