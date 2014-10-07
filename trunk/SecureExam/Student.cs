using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            sb.Append(Helper.GenerateRandomAlphaNumericChars(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret));
            return sb.ToString();
        }
    }
}
