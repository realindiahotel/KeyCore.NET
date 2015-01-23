KeyCore.NET
===========

C# (PCL) implementation of Bitcoin Private Key and Public Key management and also the generation of Bitcoin Addresses.

This PCL is targeted for Universal Apps (Windows 8.1/Windows Phone 8.1) and .NET 4.5.1 ONLY. No support for Windows 8 or Windows Phone 8/8.1 Silverlight apps.

### PLEASE ENSURE YOU ALSO DOWNLOAD, BUILD AND REFERENCE THE BitcoinUtilities.NET PROJECT LOCATED HERE: https://github.com/Thashiznets/BitcoinUtilities.NET ###

Supports compressed Public Keys as well as legacy full y co-ordinate Public Keys. Supports Bitcoin Addresses for both Public Key types, as well as dumping and importing of both WIF and WIF compressed Private Keys.

The source of random bytes used to generate random Bitcoin Private Keys is BouncyCastle SecureRandom, and I am creating a hardened seed (More secure than the default DateTime.Now.Ticks + ThreadedSeedGenerator minimal entropy) to build the SecureRandom which consists of milliseconds (Ticks) the machine has been running, processor affinity (number of CPU cores) and DateTime.UtcNow.Ticks with a few other things thrown in there such as key stretching by XORing multiple SHA512 outputs etc. Then I use this hardened seed to build the SecureRandom object and call NextBytes() to derive our random bytes. Good reading to be had here: http://comments.gmane.org/gmane.comp.encryption.bouncy-castle.devel.csharp/257 , which prompted me to harden my seed.

Demo GUI has been supplied to demonstrate usage, you can use this GUI to create "brain wallets" if you wish, however brain wallets are not something I'd recommend anyone to use.

Also has unit tests to make sure I don't break things going forward.

This code is put out for all to use for free, I don't have a great deal of Bitcoin but I'd really like some so if you find yourself using this code in a commercial implementation and you feel you are going to make some money from it, I’d appreciate it if you fling me some bitcoin to 1ETQjMkR1NNh4jwLuN5LxY7bbip39HC9PUPSV thanks :)
