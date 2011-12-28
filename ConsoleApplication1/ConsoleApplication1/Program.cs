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

                Encrypted_Text = Convert.ToBase64String(Encrypted_Bytes);
                //Decrypted = decrypt_function(Encrypted_Bytes, Encoding.UTF8.GetBytes(Pwd_Text), Encoding.UTF8.GetBytes(IV_Text));

                Console.WriteLine("Start: {0}", Plain_Text);
                Console.WriteLine("Key: {0}", Pwd_Text);
                Console.WriteLine("IV: {0}", IV_Text);
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

        internal static string FormatByteArray(byte[] b)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            int i = 0;
            for (i = 0; i < b.Length; i++)
            {
                if (i != 0 && i % 16 == 0)
                    sb1.Append("\n");
                sb1.Append(System.String.Format("{0:X2} ", b[i]));
            }
            return sb1.ToString();
        }


        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
