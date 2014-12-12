using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SecureExam
{
    /// <summary>
    /// Student is a Paricipant<para />
    /// Contains details about Student
    /// </summary>
    public class Student : Participant
    {
        /// <summary>
        /// Students surname
        /// </summary>
        public string surName { get; set; }
        /// <summary>
        /// Students prename
        /// </summary>
        public string preName { get; set; }
        /// <summary>
        /// Students secret
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// Students ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Creates new Student
        /// </summary>
        /// <param name="preName">Students prename</param>
        /// <param name="surName">Students surname</param>
        /// <param name="id">Students ID</param>
        public Student(string preName, string surName, string id)
        {
            this.preName = preName;
            this.surName = surName;
            this.ID = id;
        }

        /// <summary>
        /// Generates secret for Student
        /// </summary>
        /// <returns>Returns secret</returns>
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
