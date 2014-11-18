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
            return Helper.ByteArrayToHexString(Helper.getSecureRandomBytes(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret / 2));
        }
    }
}
