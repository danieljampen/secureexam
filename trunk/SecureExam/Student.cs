using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class Student
    {
        // members
        private string StudentName;
        private string StudentID;

        // setter n getter
        public string studentName 
        {
            get { return StudentName; }
            set { StudentName = value; }
        }
        public string studentID
        {
            get { return StudentID; }
            set { StudentID = value; }
        }

        // methods
        public string generateStudentSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(studentName);
            sb.Append(studentID);
            sb.Append(Helper.GenerateRandomAlphaNumericChars(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret));
            return sb.ToString();
        }
        
    }
}
