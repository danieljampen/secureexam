using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public interface IDataProvider
    {
        void readData(QuestionFormularType fromularType, String formularPath, StudentFileType studentType, String studentPath, String settingsPath);
        void export(OutputType type, String path, StudentSecretsFileFormat studentSecretsFileFormat);
    }
}
