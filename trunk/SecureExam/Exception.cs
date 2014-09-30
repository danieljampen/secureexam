using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class InvalidExportTypeException : Exception
    {
        public InvalidExportTypeException(string message) : base(message) { }
    }

    class InvalidFormularTypeException : Exception
    {
        public InvalidFormularTypeException(string message) : base(message) { }
    }

    class InvalidStudentFileTypeException : Exception
    {
        public InvalidStudentFileTypeException(string message) : base(message) { }
    }
}
