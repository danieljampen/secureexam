using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    struct Answer
    {
        public string text { get; set; }

        public Answer(string text)
        {
            this.text = text;
        }
    }
}
