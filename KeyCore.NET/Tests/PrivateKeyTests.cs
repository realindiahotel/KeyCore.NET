using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin.BitcoinUtilities;
using Bitcoin.KeyCore;

namespace Tests
{
    [TestClass]
    public class PrivateKeyTests
    {
        [TestMethod]
        public void Test1()
        {
            byte[] ones = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            PrivateKey pk1Compressed = new PrivateKey(Globals.ProdDumpKeyVersion, new EcKeyPair(ones));
            PrivateKey pk1NotCompressed = new PrivateKey(Globals.ProdDumpKeyVersion, new EcKeyPair(ones,false));
            Assert.AreEqual("KwFfNUhSDaASSAwtG7ssQM1uVX8RgX5GHWnnLfhfiQDigjioWXHH",pk1Compressed.WIFEncodedPrivateKeyString);
            Assert.AreEqual("5HpjE2Hs7vjU4SN3YyPQCdhzCu92WoEeuE6PWNuiPyTu3ESGnzn",pk1NotCompressed.WIFEncodedPrivateKeyString);
            Assert.AreEqual("KwFfNUhSDaASSAwtG7ssQM1uVX8RgX5GHWnnLfhfiQDigjioWXHH", pk1Compressed.ToString());
            Assert.AreEqual("5HpjE2Hs7vjU4SN3YyPQCdhzCu92WoEeuE6PWNuiPyTu3ESGnzn", pk1NotCompressed.ToString());
            Assert.AreEqual(true, pk1Compressed.MakesCompressedPublicKey);
            Assert.AreEqual(false, pk1NotCompressed.MakesCompressedPublicKey);
            Assert.AreEqual(Globals.ProdDumpKeyVersion[0], new byte[] { Convert.ToByte(pk1Compressed.Version) }[0]);
            Assert.AreEqual(Globals.ProdDumpKeyVersion[0], new byte[] { Convert.ToByte(pk1NotCompressed.Version) }[0]);
            Assert.AreEqual(Utilities.BytesToHexString(ones), Utilities.BytesToHexString(pk1Compressed.PrivateKeyBytes));
            Assert.AreEqual(Utilities.BytesToHexString(ones), Utilities.BytesToHexString(pk1NotCompressed.PrivateKeyBytes));
        }     

        [TestMethod]
        public void Test2()
        {
            PrivateKey pk1Compressed = PrivateKey.CreatePrivateKeyAsync(Globals.ProdDumpKeyVersion).Result;
            PrivateKey pk1NotCompressed = PrivateKey.CreatePrivateKeyAsync(Globals.ProdDumpKeyVersion,false).Result;
            string wifPk1Compressed = pk1Compressed.WIFEncodedPrivateKeyString;
            string wifPk2NotCompressed = pk1NotCompressed.WIFEncodedPrivateKeyString;
            Assert.AreEqual(wifPk1Compressed, pk1Compressed.ToString());
            Assert.AreEqual(wifPk2NotCompressed, pk1NotCompressed.ToString());
            Assert.AreEqual(true, pk1Compressed.MakesCompressedPublicKey);
            Assert.AreEqual(false, pk1NotCompressed.MakesCompressedPublicKey);
            Assert.AreEqual(Globals.ProdDumpKeyVersion[0], new byte[] { Convert.ToByte(pk1Compressed.Version) }[0]);
            Assert.AreEqual(Globals.ProdDumpKeyVersion[0], new byte[] { Convert.ToByte(pk1NotCompressed.Version) }[0]);
        }

        [TestMethod]
        public void Test3()
        {
            PrivateKey pk1Compressed = PrivateKey.CreatePrivateKey(Globals.ProdDumpKeyVersion);
            PrivateKey pk1NotCompressed = PrivateKey.CreatePrivateKey(Globals.ProdDumpKeyVersion, false);
            string wifPk1Compressed = pk1Compressed.WIFEncodedPrivateKeyString;
            string wifPk2NotCompressed = pk1NotCompressed.WIFEncodedPrivateKeyString;
            Assert.AreEqual(wifPk1Compressed, pk1Compressed.ToString());
            Assert.AreEqual(wifPk2NotCompressed, pk1NotCompressed.ToString());
            Assert.AreEqual(true, pk1Compressed.MakesCompressedPublicKey);
            Assert.AreEqual(false, pk1NotCompressed.MakesCompressedPublicKey);
            Assert.AreEqual(Globals.ProdDumpKeyVersion[0], new byte[] { Convert.ToByte(pk1Compressed.Version) }[0]);
            Assert.AreEqual(Globals.ProdDumpKeyVersion[0], new byte[] { Convert.ToByte(pk1NotCompressed.Version) }[0]);
        }
    }
}
