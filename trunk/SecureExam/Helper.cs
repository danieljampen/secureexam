using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

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
    }
}
