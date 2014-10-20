﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;

namespace SecureExam
{
    class Helper
    {
        private static Random random = new Random();

        public static string ByteArrayToHexString( Byte[] array )
        {
            StringBuilder sb = new StringBuilder();
            String hex;
            foreach( Byte b in array)
            {
                hex = Convert.ToString(b, 16);
                if (hex.Length == 1) hex = "0" + hex;
                sb.Append( hex );
            }
            return sb.ToString().ToUpper();
        }

        public static byte[] SHA256(string data, byte[] iv, int iterations)
        {
            using(SHA256 mySHA256 = SHA256Managed.Create())
            {
                if(iv.Length != BasicSettings.getInstance().Encryption.SHA256.SALTLENGTH)
                    throw new ArgumentException("SHA256 IV length invalid");
                if (iterations <= 0)
                    throw new ArgumentException("SHA256 Iterations invalid");

                String ivB64 = Convert.ToBase64String(iv);

                byte[] hash = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(data + ivB64));
                for( int i = 0; i < iterations -1; i++ )
                {
                    hash = mySHA256.ComputeHash(hash);
                }

                return hash;
            }
        }

        public static byte[] getSecureRandomBytes(int length)
        {
            Byte[] array = new Byte[length];

            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(array);
            }

            return array;
        }

        public static byte[] encryptAES(string data, byte[] Key, byte[] IV)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }
    }
}