using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Question containing text, list of answers and defines the type of the queston (Checkbox or Textbox)
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Is the text of the question
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// Is a list of answers
        /// </summary>
        public List<Answer> answers { get; set; }

        /// <summary>
        /// Defines the type of the question (Checkbox or Textbox)
        /// </summary>
        public QuestionType questionType { get; set; }


        /// <summary>
        /// Creates new Question and an empty list of answers
        /// </summary>
        public Question()
        {
            answers = new List<Answer>();
        }
    }
}
