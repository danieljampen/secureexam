using System;
using System.Collections.Generic;

namespace SecureExam
{
    /// <summary>
    /// Interface for ParameterParser<para />
    /// Is used to parse the SecureExam configuration file.
    /// </summary>
    public interface IParameterParser
    {
        /// <summary>
        /// Parses a document by a given path.
        /// </summary>
        /// <param name="parameterPath">Path of the document to parse</param>
        void parse(String parameterPath);
    }
}
