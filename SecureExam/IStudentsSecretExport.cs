using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Interface for StudentsSecretExport<para />
    /// Is used to export the student details with secrets into a file.
    /// </summary>
    public interface IStudentsSecretExport
    {
        /// <summary>
        /// Exports student secrets file to a given filename
        /// </summary>
        /// <param name="filename">Filename, where to export the students secrets file</param>
        void export(String filename);
    }
}
