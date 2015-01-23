using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Bitcoin.KeyCore;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Bitcoin.BitcoinUtilities;

namespace TestUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            //encode 40 random bytes using base64 and put them in the text box
            PrimeThis();
        }

        private async void PrimeThis()
        {
            tbString.Text = Convert.ToBase64String(await Utilities.GetRandomBytes(36)).Replace('\r','r').Replace('\n','n');
            //create a 32 byte private key from the 40 byte random string now in the textbox
            byte[] keyBytes = new SHA256Managed().ComputeHash(UTF8Encoding.UTF8.GetBytes(tbString.Text), 0, UTF8Encoding.UTF8.GetBytes(tbString.Text).Length);
            PrivateKey pkInitialWif = new PrivateKey(Globals.ProdDumpKeyVersion, keyBytes);
            wifp.Text = pkInitialWif.WIFEncodedPrivateKeyString;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //create a new random private key 
            PrivateKey random = await PrivateKey.CreatePrivateKey(Globals.ProdDumpKeyVersion);
            DisplayResults(random.PrivateKeyBytes);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //we convert the textbox text into buytes using UTF8 encoding and then we hash those bytes to get 32 bytes which we use as bytes to construct private keys.
            byte[] seedBytes = new SHA256Managed().ComputeHash(UTF8Encoding.UTF8.GetBytes(tbString.Text), 0, UTF8Encoding.UTF8.GetBytes(tbString.Text).Length);
            DisplayResults(seedBytes);
            
        }

        private void DisplayResults(byte[] seedBytes)
        {
            //create a private key using seedBytes and not supplying compressedPublicKey which by default forces the public key to be compressed
            PrivateKey pkCompressed = new PrivateKey(Globals.ProdDumpKeyVersion, seedBytes);
            //get the wif encoded string that represents the pkCompressed private key
            String wifCompressed = pkCompressed.WIFEncodedPrivateKeyString;
            //here we can create a bitcoin address string using the public key ascociated with our private key
            String bitcoinAddressCompressed = BitcoinAddress.GetBitcoinAdressEncodedStringFromPublicKey(pkCompressed.PublicKey);

            //create a private key using seedBytes which forces the public key to be compressed and supplying compressedPublicKey as false so the public key will not be compressed
            PrivateKey pkNotCompressed = new PrivateKey(Globals.ProdDumpKeyVersion, seedBytes, false);
            String wif = pkNotCompressed.WIFEncodedPrivateKeyString;
            String bitcoinAddress = BitcoinAddress.GetBitcoinAdressEncodedStringFromPublicKey(pkNotCompressed.PublicKey);

            Results res = new Results(wif, bitcoinAddress, wifCompressed, bitcoinAddressCompressed);
            res.Owner = this;
            res.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if(wifp.Text.Equals(""))
            {
                MessageBox.Show("WIF cannot be empty");
                return;
            }

                try
                {
                    Byte[] bytes = new Byte[32];
                    PrivateKey wp = new PrivateKey(Globals.ProdDumpKeyVersion, wifp.Text);
                    DisplayResults(wp.PrivateKeyBytes);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: "+ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Bitcoin:1ETQjMkR1NNh4jwLuN5LxY7bMsHC9PUPSV");
            }
            catch
            {

            }
        }
    }
}
