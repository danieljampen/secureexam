using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public class Professor : Participant
    {
        public string surName { get; set; }
        public string preName { get; set; }
        public string secret { get; set; }

        public Professor(string preName, string surName)
        {
            this.surName = surName;
            this.preName = preName;
        }

        protected override string generateSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(preName);
            sb.Append(surName);
            secret = Helper.ByteArrayToHexString(Helper.getSecureRandomBytes(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret / 2));
            sb.Append(secret);
            return sb.ToString();
        }
    }
}
