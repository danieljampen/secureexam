using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public abstract class Participant
    {
        private string participantSecret;

        public string ParticipantSecret
        {
            get
            {
                if (participantSecret == null)
                    participantSecret = this.generateSecret();
                return participantSecret;
            }
        }

        protected abstract string generateSecret();
    }
}
