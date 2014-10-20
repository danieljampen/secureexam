using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class Professor:Participant
    {
        public string name { get; set; }

        public Professor(string name)
        {
            this.name = name;
        }

        protected override string generateStudentSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            sb.Append(Helper.ByteArrayToHexString(Helper.getSecureRandomBytes(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret / 2)));
            return sb.ToString();
        }
    }
}
