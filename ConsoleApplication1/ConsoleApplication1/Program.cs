using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string Plain_Text;
            string Pwd_Text;
            string IV_Text;
            string Decrypted;
            string Encrypted_Text;
            byte[] Encrypted_Bytes;

            Console.WriteLine("Please put in the text to be encrypted.");
            Plain_Text = Console.ReadLine();
            if (string.IsNullOrEmpty(Plain_Text)) Plain_Text = "test";
            Console.WriteLine("Please put in the text to be used as password.");
            Pwd_Text = Console.ReadLine();
            if (string.IsNullOrEmpty(Pwd_Text)) Pwd_Text = "12345678";
            Console.WriteLine("Please put in the text to be used as IV.");
            IV_Text = Console.ReadLine();
            if (string.IsNullOrEmpty(IV_Text)) IV_Text = "fangjie000000000";

            RijndaelEnhanced rijndaelKey =
             new RijndaelEnhanced(Pwd_Text, IV_Text, 4, 32, 128, "SHA1");

            try
            {

                Encrypted_Bytes = rijndaelKey.EncryptToBytes(Plain_Text);
                //Encrypted_Bytes = encrypt_function(Plain_Text, Pwd_Text, IV_Text);

                Encrypted_Text = Convert.ToBase64String(Encrypted_Bytes);
                //Decrypted = decrypt_function(Encrypted_Bytes, Encoding.UTF8.GetBytes(Pwd_Text), Encoding.UTF8.GetBytes(IV_Text));

                Console.WriteLine("Start: {0}", Plain_Text);
                Console.WriteLine("Encrypted: {0}", Encrypted_Text);
                //Console.WriteLine("Decrypted: {0}", Decrypted);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        private static byte[] encrypt_function(string Plain_Text, string Key, string Vector)
        {
            Byte[] Data = Encoding.UTF8.GetBytes(Plain_Text);

            Byte[] bKey = new Byte[32];
            //Console.WriteLine(Encoding.UTF8.GetBytes(Key).Length);
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            //bKey = Encoding.UTF8.GetBytes(Key);
            Byte[] bVector = new Byte[16];
            Console.WriteLine(Encoding.UTF8.GetBytes(Vector).Length);
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
            //bVector = Encoding.UTF8.GetBytes(Vector);

            Byte[] Cryptograph = null; // 加密后的密文

            Aes aes = new AesManaged();

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.ANSIX923;
            aes.KeySize = 32;
            aes.Key = bKey;
            aes.IV = bVector;

            try
            {
                // 开辟一块内存流
                using (MemoryStream Memory = new MemoryStream())
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream Encryptor = new CryptoStream(Memory, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // 明文数据写入加密流
                        Encryptor.Write(Data, 0, Data.Length);
                        //Encryptor.FlushFinalBlock();

                        Memory.Seek(0, SeekOrigin.Begin);

                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                Cryptograph = null;
            }

            return Cryptograph;
        }

        private static string decrypt_function(byte[] Cipher_Text, byte[] Key, byte[] IV)
        {
            RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;
            ICryptoTransform Decryptor = null;
            CryptoStream Crypto_Stream = null;
            StreamReader Stream_Read = null;
            string Plain_Text;

            try
            {
                Crypto = new RijndaelManaged();
                //Crypto.Key = Key;
                //Crypto.IV = IV;
                Crypto.Mode = CipherMode.CBC;
                Crypto.Padding = PaddingMode.ANSIX923;

                MemStream = new MemoryStream(Cipher_Text);

                //Create Decryptor make sure if you are decrypting that this is here and you did not copy paste encryptor.
                Decryptor = Crypto.CreateDecryptor(Key, IV);

                //This is different from the encryption look at the mode make sure you are reading from the stream.
                Crypto_Stream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);

                //I used the stream reader here because the ReadToEnd method is easy and because it return a string, also easy.
                Stream_Read = new StreamReader(Crypto_Stream);
                Plain_Text = Stream_Read.ReadToEnd();
            }
            finally
            {
                if (Crypto != null)
                    Crypto.Clear();

                MemStream.Flush();
                MemStream.Close();

            }
            return Plain_Text;
        }
    }
}
