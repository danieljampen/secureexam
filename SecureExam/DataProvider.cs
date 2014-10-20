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
        private LinkedList<Student> students = new LinkedList<Student>();
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

        public LinkedList<Student> Students
        {
            get { return this.students; }
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
            this.students = this.studentParser.parse(studentPath);

            return (this.questions.Count != 0 && this.students.Count != 0);
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
                    studentsSecretPath = path.Substring(0, path.Length-4) + ".xml";
                    break;
                default:
                    throw new InvalidStudentSecretsFileFormatException(type.ToString());
            }

            success = this.questionsExporter.export(path);
            success = success & this.studentsSecretExporter.export(studentsSecretPath);
            return success;
        }

        public string exportQuestions(DataProviderExportType type)
        {
            StringBuilder sb = new StringBuilder();
            
            if( BasicSettings.getInstance().Encryption.AES.questionsAESKey == null)
            {
                Aes aes = Aes.Create();
                BasicSettings.getInstance().Encryption.AES.questionsAESKey = aes.Key;
                BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV = aes.IV;
            }

            sb.Append(Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV));
            sb.Append(",");
            
            switch (type)
            {
                case DataProviderExportType.HTML:
                     sb.Append( this.encryptAES( this.exportQuestionsHTML(), BasicSettings.getInstance().Encryption.AES.questionsAESKey, BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV) );
                     break;
                default:
                    throw new InvalidDataProviderExportTypeException(type.ToString());
            }

            return sb.ToString();
        }

        public string exportUserKeyDB(DataProviderExportType type)
        {
            switch (type)
            {
                case DataProviderExportType.HTML:
                    return this.exportUserKeyDBHTML();
                default:
                    throw new InvalidDataProviderExportTypeException(type.ToString());
            }
        }

        private string exportQuestionsHTML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<form>\n");
            foreach (Question question in this.questions)
            {
                sb.Append("<fieldset class=\"questionFieldset\">\n");
                sb.Append("\t<p class=\"questionText\">" + question.text + "</p>\n");
                switch (question.questionType)
                {
                    case QuestionType.CHECK_BOX:
                        foreach (Answer answer in question.answers)
                        {
                            sb.Append("<input type=\"checkbox\" class=\"checkbox\" />" + answer.text + "<br>\n");
                        }
                        break;
                    case QuestionType.TEXT_BOX:
                        sb.Append("<input type=\"text\" class=\"textBox\" ");
                        if (question.answers[0].text != null)
                            sb.Append("value=\"" + question.answers[0].text + "\"");
                        else
                            sb.Append("placeholder=\"" + question.answers[0].placeHolder + "\"");
                        sb.Append(">\n");
                        break;
                }
                sb.Append("</fieldset>\n");
            }
            sb.Append("</form>\n");
            return sb.ToString();
        }

        private string exportUserKeyDBHTML()
        {
            if (BasicSettings.getInstance().Encryption.AES.questionsAESKey != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Student student in this.students)
                {
                    using(RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
                    {
                        byte[] salt = new byte[BasicSettings.getInstance().Encryption.SHA256.SALTLENGTH];
                        rngCsp.GetBytes(salt);

                        using (Aes aes = Aes.Create())
                        {
                            // aes settings
                            aes.Padding = PaddingMode.PKCS7;
                            aes.Mode = CipherMode.CBC;

                            sb.Append(student.studentPreName);
                            sb.Append(student.studentSurName);
                            sb.Append(student.studentID);
                            sb.Append(",");
                            byte[] userHAsh = Helper.SHA256(student.StudentSecret, salt, BasicSettings.getInstance().Encryption.SHA256.ITERATIONS);
                            sb.Append(encryptAES(Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKey), userHAsh, aes.IV));
                            sb.Append(",");
                            sb.Append(Helper.ByteArrayToHexString(aes.IV));
                            sb.Append(",");
                            sb.Append(Convert.ToBase64String(salt));
                            sb.Append("<br>");

                            Debug.WriteLine(student.studentSurName + " "  + student.studentPreName + " UserSecret: " + student.StudentSecret);
                        }
                    }
                }
                return sb.ToString();
            }
            else
                throw new NoAESKeyException("No Masterkey found");
        }

        private string encryptAES(string data, byte[] Key, byte[] IV)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            
            string encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        encrypted = Helper.ByteArrayToHexString(msEncrypt.ToArray());
                    }
                }
            } 

            return encrypted;
        }
    }
}
