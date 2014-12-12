using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Contains the details to an answer
    /// </summary>
    public struct Answer
    {
        /// <summary>
        /// Defines the answer text.
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// Defines a placeHolder text, it is used for textboxes.
        /// </summary>
        public string placeHolder { get; set; }

        /// <summary>
        /// Defines whether an answer is correct or not.
        /// </summary>
        public bool isCorrect { get; set; }
    }
}
