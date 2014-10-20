using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SecureExam
{
    class HTMLJSExport : IQuestionsExport
    {
        public bool export(String filename)
        {
            StreamReader htmlSkeleton = new StreamReader(BasicSettings.getInstance().exportSkeletons[OutputType.HTMLJS]);
            StreamWriter outFile = new StreamWriter(filename);

            if (BasicSettings.getInstance().Encryption.AES.questionsAESKey == null)
            {
                using( Aes aes = Aes.Create() )
                {
                    BasicSettings.getInstance().Encryption.AES.questionsAESKey = aes.Key;
                    BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV = aes.IV;
                }
            }

            try
            {
                // read skeleton
                String html = htmlSkeleton.ReadToEnd();

                // Replace the placeholders in HTML code with real data
                html = html.Replace("$SHA256ITERATIONS$", BasicSettings.getInstance().Encryption.SHA256.ITERATIONS.ToString());
                html = html.Replace("$RANDOMCHARSINUSERSECRET$", BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret.ToString());
                html = html.Replace("$ENCRYPTEDDATA$", this.exportQuestions());
                html = html.Replace("$USERKEYDB$", this.exportUserKeyDB());
                html = html.Replace("$SUBJECT$", BasicSettings.getInstance().Subject);

                // write data to file
                outFile.Write(html);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                htmlSkeleton.Close();
                outFile.Close();
            }
            return true;
        }

        private string exportQuestions()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV));
            sb.Append(",");
            sb.Append(Helper.encryptAES(this.generateQuestionsHTML(), BasicSettings.getInstance().Encryption.AES.questionsAESKey, BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV));
            return sb.ToString();
        }

        private string exportUserKeyDB()
        {
            if (BasicSettings.getInstance().Encryption.AES.questionsAESKey != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Student student in DataProvider.getInstance().Students)
                {
                    byte[] salt = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.SHA256.SALTLENGTH);

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
                        sb.Append(Helper.encryptAES(Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKey), userHAsh, aes.IV));
                        sb.Append(",");
                        sb.Append(Helper.ByteArrayToHexString(aes.IV));
                        sb.Append(",");
                        sb.Append(Convert.ToBase64String(salt));
                        sb.Append("<br>");

                        Debug.WriteLine(student.studentSurName + " " + student.studentPreName + " UserSecret: " + student.StudentSecret);
                    }
                }
                return sb.ToString();
            }
            else
                throw new NoAESKeyException("No Masterkey found");
        }

        private string generateQuestionsHTML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<form>\n");
            foreach (Question question in DataProvider.getInstance().Questions)
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
    }
}
