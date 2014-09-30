using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    interface IStudentParser
    {
        LinkedList<Student> parse( Path studentPath );
    }
}
