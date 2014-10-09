using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class Helper
    {
        private static Random random = new Random();

        public static string GenerateRandomAlphaNumericChars( int amountOfChars )
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"+
                        "abcdefghijklmnopqerstuvwxyz" +
                        "0123456789";
            return new string(Enumerable.Repeat(chars, amountOfChars).Select(s => s[random.Next(s.Length)]).ToArray());
        }

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
    }
}
