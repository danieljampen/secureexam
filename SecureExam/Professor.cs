using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public class Professor : Participant
    {
        public string name { get; set; }
        public string secret { get; set; }

        public Professor(string name)
        {
            this.name = name;
        }

        protected override string generateSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            secret = Helper.ByteArrayToHexString(Helper.getSecureRandomBytes(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret / 2));
            sb.Append(secret);
            return sb.ToString();
        }
    }
}
