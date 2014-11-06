using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class Facade
    {
        // methods
        public bool export(OutputType type, String path, StudentSecretsFileFormat studentSecretsFileFormat) { return DataProvider.getInstance().export(type, path, studentSecretsFileFormat); }
        public bool readData(QuestionFormularType formularType, String formularPath, StudentFileType studentType, String studentPath, String settingsPath) { return DataProvider.getInstance().readData(formularType, formularPath, studentType, studentPath, settingsPath); }
    }
}
