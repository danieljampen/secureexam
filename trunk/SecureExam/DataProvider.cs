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
        private IExport exporter;
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

        public bool export(OutputType type, String path)
        {
            if( path == null || path.Length == 0 )
                throw new ArgumentNullException("path");

            switch (type)
            {
                case OutputType.HTMLJS:
                    this.exporter = new HTMLJSExport();
                    return this.exporter.export(path);
                default:
                    throw new InvalidExportTypeException(type.ToString());
            }
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
                            sb.Append("<input type=\"radio\" class=\"radio\" />" + answer.text + "<br>\n");
                        }
                        break;
                    case QuestionType.TEXT_BOX:
                        sb.Append("<input type=\"checkbox\" class=\"textBox\" ");
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
                            sb.Append("<br>\n");

                            Debug.WriteLine(student.studentSurName + " UserSecret: " + student.StudentSecret);
                            Debug.WriteLine(student.studentSurName + " SHA256: in: " + student.StudentSecret + 
                                " + " + Helper.ByteArrayToHexString(salt) + " iterations: " + BasicSettings.getInstance().Encryption.SHA256.ITERATIONS + 
                                " = " + Helper.ByteArrayToHexString(userHAsh));
                            Debug.WriteLine(student.studentSurName + " AES: IV: " + Helper.ByteArrayToHexString(aes.IV) + "");
                        }
                    }
                }
                Debug.WriteLine("AES MasterKey: " + Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKey) + " iv: " + Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV));
              
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
