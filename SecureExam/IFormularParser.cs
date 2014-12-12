using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Interface for ForlmularParser<para />
    /// Is used to parse the question import file.
    /// </summary>
    public interface IFormularParser
    {
        /// <summary>
        /// Parses a document by a given stream.
        /// </summary>
        /// <param name="streamReader">stream of the document to parse</param>
        /// <returns>Returns LinkedList of questions</returns>
        LinkedList<Question> parse(StreamReader streamReader);
    }
}
