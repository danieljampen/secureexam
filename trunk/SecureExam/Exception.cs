using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class InvalidExportTypeException : ApplicationException
    {
        public InvalidExportTypeException(string message) : base(message) { }
    }

    class InvalidFormularTypeException : ApplicationException
    {
        public InvalidFormularTypeException(string message) : base(message) { }
    }

    class InvalidStudentFileTypeException : ApplicationException
    {
        public InvalidStudentFileTypeException(string message) : base(message) { }
    }

    class InvalidDataProviderExportTypeException : ApplicationException
    {
        public InvalidDataProviderExportTypeException(string message) : base(message) { }
    }

    class NoAESKeyException : ApplicationException
    {
        public NoAESKeyException(string message) : base(message) { }
    }
}
