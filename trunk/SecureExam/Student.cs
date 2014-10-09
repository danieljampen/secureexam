using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SecureExam
{
    class Student
    {

        // setter n getter
        public string studentPreName { get; set; }
        public string studentSurName { get; set; }
        public string studentID { get; set; }

        // methods
        public string generateStudentSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(studentPreName);
            sb.Append(studentSurName);
            sb.Append(studentID);

            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret * 8];
                rngCsp.GetBytes(randomBytes);
                sb.Append(Helper.ByteArrayToHexString(randomBytes);
            }
            return sb.ToString();
        }
    }
}
