using System;
using Bitcoin.BitcoinUtilities;

namespace Bitcoin.KeyCore
{
    /// <summary>
    /// A BitCoin address is fundamentally derived from an elliptic curve public key and a set of network parameters.
    /// </summary>
    /// <remarks>
    /// It has several possible representations:<p/>
    /// <ol>
    ///   <li>The raw public key bytes themselves.</li>
    ///   <li>RIPEMD160 hash of the public key bytes.</li>
    ///   <li>A base58 encoded "human form" that includes a version and check code, to guard against typos.</li>
    /// </ol><p/>
    /// One may question whether the base58 form is really an improvement over the hash160 form, given
    /// they are both very unfriendly for typists. More useful representations might include QR codes
    /// and identicons.<p/>
    /// Note that an address is specific to a network because the first byte is a discriminator value.
    /// </remarks>
    public class BitcoinAddress : VersionedChecksummedBytes
    {
        /// <summary>
        /// Construct a BitcoinAddress from a PublicKey object
        /// </summary>
        /// <param name="publicKey">The public key to build the bitcoin address</param>
        public BitcoinAddress(PublicKey publicKey):base(publicKey.Version,publicKey.PublicKeyHash)
        {

        }

        /// <summary>
        /// Construct a BitcoinAddress from a PublicKey object and supply a version
        /// </summary>
        /// <param name="version">By supplying a version we have the option to force a version different to what is recorded wit hthe public key</param>
        /// <param name="publicKey">The public key used to build the bitcoin address</param>
        public BitcoinAddress(byte[] version, PublicKey publicKey): base(Convert.ToInt32(version[0]), publicKey.PublicKeyHash)
        {

        }

        /// <summary>
        /// Construct an address from version and the hash160 form.
        /// </summary>
        public BitcoinAddress(Byte[] version, byte[] hash160)
            : base(Convert.ToInt32(version[0]), hash160)
        {
            if (hash160.Length != 20) // 160 = 8 * 20
                throw new ArgumentException("Addresses are 160-bit hashes, so you must provide 20 bytes", "hash160");
        }

        /// <summary>
        /// Construct an address from parameters and the standard "human readable" form.
        /// </summary>
        /// <remarks>
        /// Example:<p/>
       public BitcoinAddress(Byte[] version, string address)
            : base(address)
        {
            if (Version != Convert.ToInt32(version[0]))
                throw new Exception("Mismatched version number, trying to cross networks? " + Version +
                                                 " vs " + Convert.ToInt32(version[0]));
        }

        /// <summary>
        /// Gets the Base58 encoded version of the public key hash aka "Bitcoin Adress"
        /// </summary>
        public string BitcoinAddressEncodedString
        {
            get
            {
                return ToString();
            }
        }

        /// <summary>
        /// The (big endian) 20 byte hash that is the core of a BitCoin address.
        /// </summary>
        public byte[] Hash160
        {
            get { return Bytes; }
        }

        /// <summary>
        /// A method to turn a public key into a bitcoin address encoded string
        /// </summary>
        /// <param name="pubKey">The public key to turn into the bitcoin address encoded string</param>
        /// <returns>A bitcoin address encoded string</returns>
        public static string GetBitcoinAdressEncodedStringFromPublicKey(PublicKey pubKey)
        {
            return new BitcoinAddress(pubKey).BitcoinAddressEncodedString;            
        }

        /// <summary>
        /// A method to turn a public key into a bitcoin address encoded string with the option to supply a different version to what is recorded with the public key
        /// </summary>
        /// /// <param name="version">The Network version to use when creating the bitcoin address. Grab from BitcoinUtilities.Globals</param>
        /// <param name="pubKey">The public key to turn into the bitcoin address encoded string</param>
        /// <returns>A bitcoin address encoded string</returns>
        public static string GetBitcoinAdressEncodedStringFromPublicKey(byte[] version, PublicKey pubKey)
        {
            return new BitcoinAddress(version, pubKey).BitcoinAddressEncodedString;
        }
    }
}