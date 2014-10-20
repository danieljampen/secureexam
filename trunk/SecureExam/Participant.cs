using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    abstract class Participant
    {
        private string studentSecret;

        public string StudentSecret
        {
            get
            {
                if (studentSecret == null)
                    studentSecret = this.generateStudentSecret();
                return studentSecret;
            }
        }

        protected abstract string generateStudentSecret();
    }
}
