using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace SecureExam
{
    class DataProvider : IDataProvider
    {
        // members
        private static DataProvider instance;
        private LinkedList<Question> questions = new LinkedList<Question>();
        private LinkedList<Participant> participants = new LinkedList<Participant>();
        private IQuestionsExport questionsExporter;
        private IStudentsSecretExport studentsSecretExporter;
        private IFormularParser formularParser;
        private IStudentParser studentParser;

        // methods
        public static DataProvider getInstance()
        {
            if (instance == null)
            {
                instance = new DataProvider();
            }
            return instance;
        }

        public LinkedList<Participant> Participants
        {
            get { return this.participants; }
        }
        public LinkedList<Question> Questions
        {
            get { return this.questions; }
        }

        public bool readData(QuestionFormularType formularType, String formularPath, StudentFileType studentType, String studentPath)
        {
            if (formularPath == null || formularPath.Length == 0)
                throw new ArgumentNullException("formularPath");
            if (studentPath == null || studentPath.Length == 0)
                throw new ArgumentNullException("studentPath");

            switch (formularType)
            {
                case QuestionFormularType.ODT:
                    this.formularParser = new OdtFormularParser();
                    break;
                case QuestionFormularType.XML:
                    this.formularParser = new XMLFormularParser();
                    break;
                default:
                    throw new InvalidFormularTypeException(formularType.ToString());
            }
            this.questions = this.formularParser.parse(formularPath);

            switch (studentType)
            {
                case StudentFileType.XML:
                    this.studentParser = new XMLStudentParser();
                    break;
                default:
                    throw new InvalidStudentFileTypeException(studentType.ToString());
            }
            this.participants = this.studentParser.parse(studentPath);

            return (this.questions.Count != 0 && this.participants.Count != 0);
        }

        public bool export(OutputType type, String path, StudentSecretsFileFormat studentSecretsFileFormat)
        {
            if( path == null || path.Length == 0 )
                throw new ArgumentNullException("path");
            bool success = false;
            String studentsSecretPath;

            switch (type)
            {
                case OutputType.HTMLJS:
                    this.questionsExporter = new HTMLJSExport();
                    break;
                default:
                    throw new InvalidExportTypeException(type.ToString());
            }

            switch (studentSecretsFileFormat)
            {
                case StudentSecretsFileFormat.XML:
                    this.studentsSecretExporter = new XMLStudentsSecretsExporter();
                    studentsSecretPath = path.Substring(0, path.Length-5) + ".xml";
                    break;
                default:
                    throw new InvalidStudentSecretsFileFormatException(type.ToString());
            }

            success = this.questionsExporter.export(path);
            success = success & this.studentsSecretExporter.export(studentsSecretPath);
            return success;
        }        
    }
}
