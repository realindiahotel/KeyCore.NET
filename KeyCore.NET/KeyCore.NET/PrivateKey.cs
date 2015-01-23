using System;
using System.Threading.Tasks;
using System.Linq;
using Bitcoin.BitcoinUtilities;

namespace Bitcoin.KeyCore
{
    /// <summary>
    /// Parses and generates private keys in the Wallet Import Format (WIF).
    /// </summary>
    public class PrivateKey : VersionedChecksummedBytes
    {
        private bool _isCompressedPub;

        /// <summary>
        /// Generates a PrivateKey object from 32 byte array
        /// </summary>
        /// <param name="version">Network version to use for dumped keys get from BitcoinUtilities.Globals</param>
        /// <param name="keyBytes">The 32 byte array to use as the private key</param>
        /// <param name="compressedPublicKey">Optional bool to force compressed final key, true if not specified</param>
        public PrivateKey(byte[] version, byte[] keyBytes, bool compressedPublicKey=true):base(Convert.ToInt32(version[0]),CheckCompression(keyBytes,compressedPublicKey))
        {
            _isCompressedPub = compressedPublicKey;
            if (keyBytes.Length < 32 || keyBytes.Length > 32) // 256 bit keys
                throw new ArgumentException("Keys are 256 bits, so you must provide 32 bytes, got " + keyBytes.Length + " bytes", "keyBytes");
        }

        /// <summary>
        /// Generates a PrivateKey object from an EcKeyPair
        /// </summary>
        /// <param name="version">Network version to use for dumped keys get from BitcoinUtilities.Globals</param>
        /// <param name="keyPair">The EcKeyPair to derive the WIF key from</param>
        public PrivateKey(byte[] version, EcKeyPair keyPair): base(Convert.ToInt32(version[0]), CheckCompression(keyPair.PrivateKey,keyPair.IsCompressedPublicKey))
        {
            _isCompressedPub = keyPair.IsCompressedPublicKey;
            byte[] keyBytes = keyPair.PrivateKey;
                    if (keyBytes.Length < 32 || keyBytes.Length > 32) // 256 bit keys
                throw new ArgumentException("Keys are 256 bits, so you must provide 32 bytes, got " + keyBytes.Length + " bytes", "keyBytes");
        }

        /// <summary>
        /// Parses the given private key supplied in Wallet Import Format (WIF)
        /// </summary>
        /// <param name="version">The bitcoin network version, 0 for prod, 111 for test net, Grab from BitcoinUtilities.Globals</param>
        /// <param name="encoded">The base58 encoded string.</param>
        /// <exception cref="Exception">If the string is invalid or the header byte doesn't match the network params.</exception>
        public PrivateKey(Byte[] version, string wifEncoded)
            : base(wifEncoded)
        {
            _isCompressedPub = false;

            if(wifEncoded.ToUpper().StartsWith("K") || wifEncoded.ToUpper().StartsWith("L"))
            {
                _isCompressedPub = true;
            }

            if (Version != version[0])
                throw new Exception("Mismatched version number, trying to cross networks? " + Version +
                                                 " vs " + version[0]);
        }

        /// <summary>
        /// Gets the private key Base58 encoded as per WIF format
        /// </summary>
        public string WIFEncodedPrivateKeyString
        {
            get
            {
                return ToString();
            }
        }
         
        /// <summary>
        /// Gets the private key bytes
        /// </summary>
        public byte[] PrivateKeyBytes
        {
            get
            {
                if (MakesCompressedPublicKey && Bytes.Length >32)
                {
                    return Bytes.Take(32).ToArray();
                }

                return Bytes;
            }
        }

        /// <summary>
        /// Gets if this private key should make a compressed public key or not
        /// </summary>
        public bool MakesCompressedPublicKey
        {
            get
            {
                return _isCompressedPub;
            }
        }

        public PublicKey PublicKey
        {
            get
            {
                return new PublicKey(this);
            }
        }

        /// <summary>
        /// A static method to create a new PrivateKey object from crypto random 32 bytes
        /// </summary>
        /// <param name="networkVersion">Network version grab from BitcoinUtilities.Globals</param>
        /// <param name="compressedPublicKey">Optional bool to force compressed public key creation, defaults to true</param>
        /// <returns></returns>
        public static PrivateKey CreatePrivateKey(byte[] networkVersion, bool compressedPublicKey=true)
        {
            return new PrivateKey(networkVersion, Utilities.GetRandomBytes(32), compressedPublicKey);
        }

        /// <summary>
        /// A static method to create a new PrivateKey object from crypto random 32 bytes
        /// </summary>
        /// <param name="networkVersion">Network version grab from BitcoinUtilities.Globals</param>
        /// <param name="compressedPublicKey">Optional bool to force compressed public key creation, defaults to true</param>
        /// <returns></returns>
        public async static Task<PrivateKey> CreatePrivateKeyAsync(byte[] networkVersion, bool compressedPublicKey = true)
        {
            return new PrivateKey(networkVersion, await Utilities.GetRandomBytesAsync(32), compressedPublicKey);
        }

        /// <summary>
        /// Checks if the public key is comressed and adds the extra byte for VersionedChecksummedBytes to give us a K or L WIF
        /// used only by the constructor
        /// </summary>
        private static byte[] CheckCompression(byte[] keyBytes, bool isCompressed)
        {

            if (isCompressed && keyBytes.Length < 33)
            {
                byte[] rawbytes = keyBytes;
                byte[] add1 = { 1 };
                return Utilities.MergeByteArrays(rawbytes, add1);
            }

            return keyBytes;
        }
    }
}