using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Contains Details about the exam
    /// </summary>
    public class ExamDetails
    {
        /// <summary>
        /// Notes displayed in exam
        /// </summary>
        public String examNotes { get; set; }

        /// <summary>
        /// Subject of the exam
        /// </summary>
        public String subject { get; set; }

        /// <summary>
        /// Title of the exam
        /// </summary>
        public String examTitle { get; set; }

        /// <summary>
        /// Time when exam can be started
        /// </summary>
        public DateTime examStartTime { get; set; }

        /// <summary>
        /// Time when exam must be ended
        /// </summary>
        public DateTime examEndTime { get; set; }

        /// <summary>
        /// Duration of exam
        /// </summary>
        public int examDurationMinutes { get; set; }

        /// <summary>
        /// Is internet allowed during exam?
        /// </summary>
        public bool internetAllowed { get; set; }

        /// <summary>
        /// Are tab changes allowed during exam?
        /// </summary>
        public bool tabChangeAllowed { get; set; }

        /// <summary>
        /// Maximal variance of stored time
        /// </summary>
        public int historyTimeMaxVariance { get; set; }

        /// <summary>
        /// Maximal variance of internal time
        /// </summary>
        public int internalTimeMaxVariance { get; set; }

        /// <summary>
        /// Are restores from auto saves allowed?
        /// </summary>
        public bool confirmAutosaveRestore { get; set; }

        /// <summary>
        /// Is the export on a eBookReader?
        /// </summary>
        public bool ebookreaderExport { get; set; }

        /// <summary>
        /// Which viewMode schould be used? (scroll mode or page mode)
        /// </summary>
        public ViewMode viewMode { get; set; }
    }
}
