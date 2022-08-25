using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace G9AssemblyManagement.Core.Class.Hashing
{
    /// <summary>
    ///     Performs 32-bit reversed cyclic redundancy checks.
    /// </summary>
    public class G9CCrc32
    {
        #region Methods

        /// <summary>
        ///     Creates a new instance of the Crc32 class.
        /// </summary>
        public G9CCrc32()
        {
            // Constructs the checksum lookup table. Used to optimize the checksum.
            _checksumTable = Enumerable.Range(0, 256).Select(i =>
            {
                var tableEntry = (uint)i;
                for (var j = 0; j < 8; ++j)
                {
                    tableEntry = ((tableEntry & 1) != 0)
                        ? (Polynomial ^ (tableEntry >> 1))
                        : (tableEntry >> 1);
                }
                return tableEntry;
            }).ToArray();
        }

        /// <summary>
        /// Calculates the checksum of the byte stream.
        /// </summary>
        /// <param name="byteStream">The byte stream to calculate the checksum for.</param>
        /// <returns>A 32-bit reversed checksum.</returns>
        public uint Get<T>(IEnumerable<T> byteStream)
        {
            try
            {
                // Initialize checksumRegister to 0xFFFFFFFF and calculate the checksum.
                return ~byteStream.Aggregate(0xFFFFFFFF, (checksumRegister, currentByte) =>
                    (_checksumTable[(checksumRegister & 0xFF) ^ Convert.ToByte(currentByte)] ^ (checksumRegister >> 8)));
            }
            catch (FormatException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
            catch (InvalidCastException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
            catch (OverflowException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
        }

        /// <summary>
        ///     Method to compute hash
        /// </summary>
        public byte[] ComputeHash(Stream stream)
        {
            var result = 0xFFFFFFFF;

            int current;
            while ((current = stream.ReadByte()) != -1)
                result = _checksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);

            var hash = BitConverter.GetBytes(~result);
            Array.Reverse(hash);
            return hash;
        }

        /// <summary>
        ///     Method to compute hash
        /// </summary>
        public byte[] ComputeHash(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return ComputeHash(stream);
            }
        }

        #endregion

        #region Fields

        /// <summary>
        ///     Contains a cache of calculated checksum chunks.
        /// </summary>
        private readonly uint[] _checksumTable;

        /// <summary>
        ///     Generator polynomial (modulo 2) for the reversed CRC32 algorithm. 
        /// </summary>
        private const uint Polynomial = 0xEDB88320;

        #endregion
    }
}