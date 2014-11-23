using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public struct Answer
    {
        public string text { get; set; }
        public string placeHolder { get; set; }
        public bool isCorrect { get; set; }
    }
}
