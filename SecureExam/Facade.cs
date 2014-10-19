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
        public string Professor
        {
            set { BasicSettings.getInstance().Professor = value; }
            get { return BasicSettings.getInstance().Professor; }
        }
        public string Subject
        {
            set { BasicSettings.getInstance().Subject = value; }
            get { return BasicSettings.getInstance().Subject; }
        }
        public string ExamTitle
        {
            set { BasicSettings.getInstance().ExamTitle = value; }
            get { return BasicSettings.getInstance().ExamTitle; }
        }
        public int NumberOfRandomCharsInStudentSecret
        {
            set { BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = value; }
            get { return BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret; }
        }
        public int AESKeyLength
        {
            set { BasicSettings.getInstance().Encryption.AES.KEYLENGTH = value; }
            get { return BasicSettings.getInstance().Encryption.AES.KEYLENGTH; }
        }
        public int PBKDF2Iterations
        {
            set { BasicSettings.getInstance().Encryption.SHA256.ITERATIONS = value; }
            get { return BasicSettings.getInstance().Encryption.SHA256.ITERATIONS; }
        }

        public bool export(OutputType type, String path) { return DataProvider.getInstance().export(type, path); }
        public bool readData(QuestionFormularType formularType, String formularPath, StudentFileType studentType, String studentPath) { return DataProvider.getInstance().readData(formularType, formularPath, studentType, studentPath); }

    }
}
