using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public class Facade
    {
        // methods
        public void export(OutputType type, String path, StudentSecretsFileFormat studentSecretsFileFormat) {
            DataProvider.getInstance().export(type, path, studentSecretsFileFormat);
        }
        public void readData(QuestionFormularType formularType, String formularPath, StudentFileType studentType, String studentPath, String settingsPath) {
            DataProvider.getInstance().readData(formularType, formularPath, studentType, studentPath, settingsPath);
        }
    }
}
