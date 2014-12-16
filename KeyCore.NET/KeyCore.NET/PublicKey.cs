using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitcoin.BitcoinUtilities;

namespace Bitcoin.KeyCore
{
    public class PublicKey:VersionedChecksummedBytes
    {
        private bool _isCompressed;

        /// <summary>
        /// Creates the public key for the given private key
        /// </summary>
        /// <param name="version">Network version grab frpm Bitcoinutilities.Globals</param>
        /// <param name="privKey">The private key to derive the public key from</param>
        /// /// <param name="addressNetworkVersion">The version used to create the Bitcoin Address. Please use BitcoinUtilities.Globals.ProdAddressVersion and not the Dumpkey Version</param>
        public PublicKey(PrivateKey privKey, byte[] addressNetworkVersion):base(Convert.ToInt32(addressNetworkVersion[0]),EcKeyPair.GetPublicKeyBytesFromPrivateKeyBytes(privKey.PrivateKeyBytes,privKey.MakesCompressedPublicKey))
        {
            _isCompressed = privKey.MakesCompressedPublicKey;
        }

        /// <summary>
        /// Creates the public key for the given private key, uses the default ProdAddressVersion in Globals
        /// </summary>
        /// <param name="privKey">The private key to derive the public key from</param>
        public PublicKey(PrivateKey privKey): base(Convert.ToInt32(Globals.ProdAddressVersion[0]), EcKeyPair.GetPublicKeyBytesFromPrivateKeyBytes(privKey.PrivateKeyBytes, privKey.MakesCompressedPublicKey))
        {
            _isCompressed = privKey.MakesCompressedPublicKey;
        }

        /// <summary>
        /// Gets the hash160 form of the public key (as seen in addresses).
        /// </summary>
        public byte[] PublicKeyHash
        {
            get 
            {
                return Utilities.Sha256Hash160(Bytes); 
            }
        }

        /// <summary>
        /// Gets a bool which tells us if the public key is in compressed form
        /// </summary>
        public bool IsCompressed
        {
            get
            {
                return _isCompressed;
            }
        }

        /// <summary>
        /// The public key in byte array form
        /// </summary>
        public byte[] PublicKeyBytes
        {
            get
            {
                return Bytes;
            }
        }

    }
}
