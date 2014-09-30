using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    interface IExport
    {
        bool export(String filename, Func<String> getQuestions, Func<String> getUserKeyDB);
    }
}
