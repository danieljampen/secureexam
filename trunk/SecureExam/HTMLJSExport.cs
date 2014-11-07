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
                BasicSettings.getInstance().Encryption.AES.questionsAESKey = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.AES.KEYLENGTH);
                BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.AES.IVLENGTH);
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
                html = html.Replace("$SUBJECT$", DataProvider.getInstance().examDetails.subject);
                html = html.Replace("$EXAMTITLE$", DataProvider.getInstance().examDetails.examTitle);
                html = html.Replace("$PROFESSOR$", DataProvider.getInstance().getProfessor().name);
                html = html.Replace("$EXAMDURATION$", DataProvider.getInstance().examDetails.examDurationMinutes.ToString());
                html = html.Replace("$EXAMNOTES$", DataProvider.getInstance().examDetails.examNotes);
                DateTime baseDate = new DateTime(1970, 1, 1);
                baseDate = baseDate.Add( new TimeSpan(1,0,0) ); // JS FIX 
                TimeSpan diff = DataProvider.getInstance().examDetails.examStartTime - baseDate;
                html = html.Replace("$EXAMSTARTTIME$", diff.TotalMilliseconds.ToString());
                diff = DataProvider.getInstance().examDetails.examEndTime - baseDate;
                html = html.Replace("$EXAMENDTIME$", diff.TotalMilliseconds.ToString());

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
            sb.Append(Helper.ByteArrayToHexString(Helper.encryptAES(this.generateQuestionsHTML(), BasicSettings.getInstance().Encryption.AES.questionsAESKey, BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV)));
            return sb.ToString();
        }

        private string exportUserKeyDB()
        {
            if (BasicSettings.getInstance().Encryption.AES.questionsAESKey != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Participant participant in DataProvider.getInstance().Participants)
                {
                    byte[] salt = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.SHA256.SALTLENGTH);
                    byte[] aesIV = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.AES.IVLENGTH);
                    byte[] userHAsh = Helper.SHA256(participant.StudentSecret, salt, BasicSettings.getInstance().Encryption.SHA256.ITERATIONS);
                    string encryptedMasterKey = Helper.ByteArrayToHexString( Helper.encryptAES(Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKey), userHAsh, aesIV) );

                    if( participant.GetType() == typeof(Student))
                    {
                        sb.Append(((Student)participant).studentPreName);
                        sb.Append(((Student)participant).studentSurName);
                        sb.Append(((Student)participant).studentID);
                    }
                    else if( participant.GetType() == typeof(Professor))
                    {
                        sb.Append(((Professor)participant).name);
                    }
                    sb.Append(",");
                    sb.Append(encryptedMasterKey);
                    sb.Append(",");
                    sb.Append(Helper.ByteArrayToHexString(aesIV));
                    sb.Append(",");
                    sb.Append(Convert.ToBase64String(salt));
                    sb.Append("<br>");

                    Debug.WriteLine("-> UserSecret: " + participant.StudentSecret);
                }
                return sb.ToString();
            }
            else
                throw new NoAESKeyException("No Masterkey found");
        }

        private string generateQuestionsHTML()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<form id=\"exam\">");
            foreach (Question question in DataProvider.getInstance().Questions)
            {
                sb.AppendLine("<div class=\"question\">");
                sb.AppendLine("<p class=\"questionText\">" + question.text + "</p>");
                sb.AppendLine("<p class=\"answer\">");
                switch (question.questionType)
                {
                    case QuestionType.CHECK_BOX:
                        foreach (Answer answer in question.answers)
                        {
                            sb.AppendLine("<input type=\"checkbox\" class=\"checkbox\" />" + answer.text + "<br>");
                        }
                        break;
                    case QuestionType.TEXT_BOX:
                        sb.Append("<textarea rows=\"6\" class=\"textBox\" ");
                        if( question.answers[0].placeHolder != null )
                            sb.Append("placeholder=\"" + question.answers[0].placeHolder + "\"");
                        sb.Append(">");
                        if (question.answers[0].text != null)
                            sb.Append(question.answers[0].text);
                        sb.Append("</textarea>\n");
                        break;
                }
                sb.AppendLine("</p>");
                sb.AppendLine("</div>");
            }
            sb.AppendLine("</form>");
            return sb.ToString();
        }
    }
}
