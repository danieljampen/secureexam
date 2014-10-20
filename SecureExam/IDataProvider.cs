using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    interface IDataProvider
    {
        bool readData(QuestionFormularType fromularType, String formularPath, StudentFileType studentType, String studentPath);
        bool export(OutputType type, String path, StudentSecretsFileFormat studentSecretsFileFormat);
    }
}
