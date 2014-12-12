using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Defines a Participant, containing secret
    /// </summary>
    public abstract class Participant
    {
        private string participantSecret;

        /// <summary>
        /// Is the secret of the participant, if secret is not set, it will be generated.
        /// </summary>
        public string ParticipantSecret
        {
            get
            {
                if (participantSecret == null)
                    participantSecret = this.generateSecret();
                return participantSecret;
            }
        }

        /// <summary>
        /// Generates secret for exam Participant
        /// </summary>
        /// <returns>Returns generated secret</returns>
        protected abstract string generateSecret();
    }
}
