using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Professor is a Paricipant<para />
    /// Contains details about Professor
    /// </summary>
    public class Professor : Participant
    {
        /// <summary>
        /// Professors surname
        /// </summary>
        public string surName { get; set; }
        /// <summary>
        /// Professors prename
        /// </summary>
        public string preName { get; set; }
        /// <summary>
        /// Professors secret
        /// </summary>
        public string secret { get; set; }

        /// <summary>
        /// Creates new Professor
        /// </summary>
        /// <param name="preName">Professors prename</param>
        /// <param name="surName">Professors surname</param>
        public Professor(string preName, string surName)
        {
            this.surName = surName;
            this.preName = preName;
        }

        /// <summary>
        /// Generates secret for Professor
        /// </summary>
        /// <returns>Returns generated secret</returns>
        protected override string generateSecret()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(preName);
            sb.Append(surName);
            secret = Helper.ByteArrayToBase64(Helper.getSecureRandomBytes(BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret)).Substring(0, BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret);
            sb.Append(secret);
            return sb.ToString();
        }
    }
}
