using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public class ExamDetails
    {
        public String examNotes { get; set; }
        public String subject { get; set; }
        public String examTitle { get; set; }
        public DateTime examStartTime { get; set; }
        public DateTime examEndTime { get; set; }
        public int examDurationMinutes { get; set; }

        public bool internetAllowed { get; set; }
        public bool tabChangeAllowed { get; set; }
        public int historyTimeMaxVariance { get; set; }
        public int internalTimeMaxVariance { get; set; }
        public bool confirmAutosaveRestore { get; set; }
        public bool ebookreaderExport { get; set; }
        public ViewMode viewMode { get; set; }
    }
}
