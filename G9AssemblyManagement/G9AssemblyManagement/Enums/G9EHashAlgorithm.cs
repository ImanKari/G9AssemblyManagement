namespace G9AssemblyManagement.Enums
{
    /// <summary>
    ///     Enum to specified the hash algorithm
    /// </summary>
    public enum G9EHashAlgorithm
    {
        /// <summary>
        ///     message-digest algorithm
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MD5,

        /// <summary>
        ///     Secure Hash Algorithm 1
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA1,

        /// <summary>
        ///     Secure Hash Algorithm 256
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA256,

        /// <summary>
        ///     Secure Hash Algorithm 384
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA384,

        /// <summary>
        ///     Secure Hash Algorithm 512
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA512,

        /// <summary>
        ///     cyclic redundancy checksum
        /// </summary>
        // ReSharper disable once InconsistentNaming
        CRC32
    }
}