using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    struct Question
    {
        public string text { get; set; }
        public List<Answer> answers { get; set; }
        public QuestionType questionType { get; set; }

    }
}
