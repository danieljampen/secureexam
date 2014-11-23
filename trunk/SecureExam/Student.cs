using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SecureExam
{
    public class Student : Participant
    {

        // setter n getter
        public string studentPreName { get; set; }
        public string studentSurName { get; set; }
        public string studentID { get; set; }
        public string secret { get; set; }

        public Student(String preName, string surName, string id)
        {
            studentPreName = preName;
            studentSurName = surName;
            studentID = id;
        }

        // methods
        protected override string generateSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(studentPreName);
            sb.Append(studentSurName);
            sb.Append(studentID);
            secret = Helper.ByteArrayToHexString(Helper.getSecureRandomBytes(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret / 2));
            sb.Append(secret);
            return sb.ToString();
        }
    }
}
