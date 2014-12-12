using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Exception when an invalid export type was tried to use
    /// </summary>
    public class InvalidExportTypeException : ApplicationException
    {
        /// <summary>
        /// Exception when an invalid export type was tried to use
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidExportTypeException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when an invalid import question formular type was tried to use
    /// </summary>
    public class InvalidFormularTypeException : ApplicationException
    {
        /// <summary>
        /// Exception when an invalid import question formular type was tried to use
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidFormularTypeException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when an invalid student formular type was tried to use
    /// </summary>
    public class InvalidStudentFileTypeException : ApplicationException
    {
        /// <summary>
        /// Exception when an invalid student formular type was tried to use
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidStudentFileTypeException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when an invalid export type was tried to use for the output exam file
    /// </summary>
    public class InvalidDataProviderExportTypeException : ApplicationException
    {
        /// <summary>
        /// Exception when an invalid export type was tried to use for the output exam file
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidDataProviderExportTypeException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when an invalid student secret file format was tried to use
    /// </summary>
    public class InvalidStudentSecretsFileFormatException : ApplicationException
    {
        /// <summary>
        /// Exception when an invalid student secret file format was tried to use
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidStudentSecretsFileFormatException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when no AES key is used
    /// </summary>
    public class NoAESKeyException : ApplicationException
    {
        /// <summary>
        /// Exception when no AES key is used
        /// </summary>
        /// <param name="message">Error message</param>
        public NoAESKeyException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when an error occurs while reading data 
    /// </summary>
    public class DataReadException : ApplicationException
    {
        /// <summary>
        /// Exception when an error occurs while reading data
        /// </summary>
        /// <param name="message">Error message</param>
        public DataReadException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when an error occurs while exporting data 
    /// </summary>
    public class ExportException : ApplicationException
    {
        /// <summary>
        /// Exception when an error occurs while exporting data 
        /// </summary>
        /// <param name="message">Error message</param>
        public ExportException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when an invalid data was tried to import
    /// </summary>
    public class InvalidImportException : ApplicationException
    {
        /// <summary>
        /// Exception when an invalid data was tried to import
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidImportException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception when time format is invalid
    /// </summary>
    public class InvalidTimeException : ApplicationException
    {
        /// <summary>
        /// Exception when time format is invalid
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidTimeException(string message) : base(message) { }
    }
}
