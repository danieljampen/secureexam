using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Interface for QuestionsExport<para />
    /// Is used to export questions into a file.
    /// </summary>
    public interface IQuestionsExport
    {
        /// <summary>
        /// Exports questions to a given filename
        /// </summary>
        /// <param name="filename">Filename of the export file</param>
        void export(String filename);
    }
}