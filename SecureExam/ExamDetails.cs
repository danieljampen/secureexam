using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class ExamDetails
    {
        public String examNotes { get; set; }
        public String subject { get; set; }
        public String examTitle { get; set; }
        public DateTime examStartTime { get; set; }
        public DateTime examEndTime { get; set; }
        public int examDurationMinutes { get; set; }
    }
}
