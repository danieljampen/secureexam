using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Interface for StudentParser<para />
    /// Is used to parse the students file, which contains details about all students.
    /// </summary>
    public interface IStudentParser
    {
        /// <summary>
        /// Parses the students file, path is given.
        /// </summary>
        /// <param name="studentPath">Path of the students file</param>
        /// <returns>Returns LinkedList of participants</returns>
        LinkedList<Participant> parse(String studentPath);
    }
}