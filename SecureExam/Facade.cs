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
        // members
        private IDataProvider dataProvider = new DataProvider();

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
            set { BasicSettings.getInstance().AESKeyLength = value; }
            get { return BasicSettings.getInstance().AESKeyLength; }
        }
        public int PBKDF2Iterations
        {
            set { BasicSettings.getInstance().PBKDF2Iterations = value; }
            get { return BasicSettings.getInstance().PBKDF2Iterations; }
        }

        public bool export(exportType type, Path path) { return dataProvider.export(type, path); }
        public bool readData(Path formularPath, Path studentPath) { return dataProvider.readData(formularPath, studentPath); }

    }
}
