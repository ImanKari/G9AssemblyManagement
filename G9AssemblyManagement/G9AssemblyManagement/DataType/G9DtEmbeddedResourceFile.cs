namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type for embedded resource files
    /// </summary>
    public struct G9DtEmbeddedResourceFile
    {
        /// <summary>
        ///     Specifies the file path in embedded resource
        /// </summary>
        public string EmbeddedResourceFilePath;

        /// <summary>
        ///     Specifies the target file path
        /// </summary>
        public string TargetFilePath;

        public G9DtEmbeddedResourceFile(string embeddedResourceFilePath, string targetFilePath)
        {
            EmbeddedResourceFilePath = embeddedResourceFilePath;
            TargetFilePath = targetFilePath;
        }
    }
}