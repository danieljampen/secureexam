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
        public string preName { get; set; }
        public string surName { get; set; }
        public string ID { get; set; }
        public string secret { get; set; }

        public Student(string preName, string surName, string id)
        {
            this.preName = preName;
            this.surName = surName;
            this.ID = id;
        }

        // methods
        protected override string generateSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(preName);
            sb.Append(surName);
            sb.Append(ID);
            secret = Helper.ByteArrayToBase64(Helper.getSecureRandomBytes(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret)).Substring(0, BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret);
            sb.Append(secret);
            return sb.ToString();
        }
    }
}
