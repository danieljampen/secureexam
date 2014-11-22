using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public class InvalidExportTypeException : ApplicationException
    {
        public InvalidExportTypeException(string message) : base(message) { }
    }

    public class InvalidFormularTypeException : ApplicationException
    {
        public InvalidFormularTypeException(string message) : base(message) { }
    }

    public class InvalidStudentFileTypeException : ApplicationException
    {
        public InvalidStudentFileTypeException(string message) : base(message) { }
    }

    public class InvalidDataProviderExportTypeException : ApplicationException
    {
        public InvalidDataProviderExportTypeException(string message) : base(message) { }
    }

    public class InvalidStudentSecretsFileFormatException : ApplicationException
    {
        public InvalidStudentSecretsFileFormatException(string message) : base(message) { }
    }

    public class NoAESKeyException : ApplicationException
    {
        public NoAESKeyException(string message) : base(message) { }
    }

    public class DataReadException : ApplicationException
    {
        public DataReadException(string message) : base(message) { }
    }

    public class ExportException : ApplicationException
    {
        public ExportException(string message) : base(message) { }
    }
}
