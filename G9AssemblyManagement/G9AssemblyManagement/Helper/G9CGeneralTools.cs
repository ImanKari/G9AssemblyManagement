using G9AssemblyManagement.Core;

namespace G9AssemblyManagement.Helper
{
    public class G9CGeneralTools
    {
        /// <summary>
        ///     Method to convert a text to MD5 hash
        /// </summary>
        /// <param name="text">Specifies a text for converting</param>
        /// <returns>A MD5 hash text</returns>
        public string StringToMD5Hash(string text)
        {
            return G9CToolsHandler.CreateMd5(text);
        }
    }
}