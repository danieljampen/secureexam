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
    /// <summary>
    /// Generates the exam details to a HTML file, this file contains JavaScript and CSS
    /// </summary>
    public class HTMLJSExport : IQuestionsExport
    {
        /// <summary>
        /// Creates the export of the exam using the examdetails.
        /// Saves output to given filename.
        /// </summary>
        /// <param name="filename">Filename of the exam output file</param>
        public void export(String filename)
        {
            StreamWriter outFile = new StreamWriter(filename);

            if (BasicSettings.getInstance().Encryption.AES.questionsAESKey == null)
            {
                BasicSettings.getInstance().Encryption.AES.questionsAESKey = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.AES.KeyLength/8);
                BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.AES.IvLength/8);
            }

            try
            {
                // read skeleton
                String html = BasicSettings.getInstance().exportSkeletons[OutputType.HTMLJS];

                // Replace the placeholders in HTML code with real data
                html = html.Replace("$ENCRYPTEDDATA$", this.exportQuestions());
                html = html.Replace("$USERKEYDB$", this.exportUserKeyDB());
                html = html.Replace("$SUBJECT$", DataProvider.getInstance().examDetails.subject);
                html = html.Replace("$EXAMTITLE$", DataProvider.getInstance().examDetails.examTitle);
                html = html.Replace("$EXAMNOTES$", DataProvider.getInstance().examDetails.examNotes);

                // set variables & constants
                html = html.Replace("$SHA256ITERATIONS$", BasicSettings.getInstance().Encryption.SHA256.Iterations.ToString());
                html = html.Replace("$HISTORYTIMEMAXVARIANCE$", DataProvider.getInstance().examDetails.historyTimeMaxVariance.ToString());
                html = html.Replace("$INTERNALTIMEMAXVARIANCE$", DataProvider.getInstance().examDetails.internalTimeMaxVariance.ToString());
                html = html.Replace("$CONFIRMAUTOSAVERESTORE$", DataProvider.getInstance().examDetails.confirmAutosaveRestore.ToString().ToLower());
                html = html.Replace("$EBOOKREADEREXPORT$", DataProvider.getInstance().examDetails.ebookreaderExport.ToString().ToLower());
                
                //Weiss halt nöd genau wieds wötsch ;)
                html = html.Replace("$VIEWMODE$", DataProvider.getInstance().examDetails.viewMode.ToString().ToLower());

                // Activate choosen security features
                StringBuilder listeners = new StringBuilder();
                if (!DataProvider.getInstance().examDetails.internetAllowed) // internetAccess
                {
                    listeners.Append("exam.addEventListener(SecureExam.Event.InternetAccess.ONLINE, isOnline);\n");
                }
                if (!DataProvider.getInstance().examDetails.tabChangeAllowed) // tabchange
                {
                    listeners.Append("exam.addEventListener(SecureExam.Event.SecureTime.TABCHANGE, tabChange);\n");
                }
                html = html.Replace("$LISTENERS$", listeners.ToString());

                // write data to file
                outFile.Write(html);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                outFile.Close();
            }
        }

        /// <summary>
        /// Generates a string containing the questions and encrypts it.
        /// </summary>
        /// <returns>Returns encrypted string of questons</returns>
        private string exportQuestions()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV));
            sb.Append(",");

            StringBuilder dataToEncrypt = new StringBuilder();
            dataToEncrypt.Append(Helper.dateTimeToMillisecondsSince1970ForJS( DataProvider.getInstance().examDetails.examStartTime ) );
            dataToEncrypt.Append(",");
            dataToEncrypt.Append(Helper.dateTimeToMillisecondsSince1970ForJS( DataProvider.getInstance().examDetails.examEndTime ) );
            dataToEncrypt.Append(",");
            dataToEncrypt.Append(DataProvider.getInstance().examDetails.examDurationMinutes );
            dataToEncrypt.Append(",");
            dataToEncrypt.Append(this.generateQuestionsHTML());

            sb.Append(Helper.ByteArrayToHexString(Helper.encryptAES(dataToEncrypt.ToString(), BasicSettings.getInstance().Encryption.AES.questionsAESKey, BasicSettings.getInstance().Encryption.AES.questionsAESKeyIV)));
            return sb.ToString();
        }

        /// <summary>
        /// Generates a string containing all user details with keys.
        /// </summary>
        /// <returns>Returns a string containing all user details with keys</returns>
        private string exportUserKeyDB()
        {
            if (BasicSettings.getInstance().Encryption.AES.questionsAESKey != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Participant participant in DataProvider.getInstance().Participants)
                {
                    if (participant.GetType() == typeof(Student))
                    {
                        byte[] salt = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.SHA256.SaltLength / 8);
                        byte[] aesIV = Helper.getSecureRandomBytes(BasicSettings.getInstance().Encryption.AES.IvLength/8);
                        byte[] userHAsh = Helper.SHA256(participant.ParticipantSecret, salt, BasicSettings.getInstance().Encryption.SHA256.Iterations);
                        string encryptedMasterKey = Helper.ByteArrayToHexString( Helper.encryptAES(Helper.ByteArrayToHexString(BasicSettings.getInstance().Encryption.AES.questionsAESKey), userHAsh, aesIV) );

                    
                            sb.Append(((Student)participant).preName);
                            sb.Append(((Student)participant).surName);
                            sb.Append(((Student)participant).ID);
                            Debug.WriteLine("STUDENT-> Vorname: " + ((Student)participant).preName + " Nachname: " + ((Student)participant).surName + " ID: " + ((Student)participant).ID + " Passwort: " + ((Student)participant).secret);

                        sb.Append(",");
                        sb.Append(encryptedMasterKey);
                        sb.Append(",");
                        sb.Append(Helper.ByteArrayToHexString(aesIV));
                        sb.Append(",");
                        sb.Append(Convert.ToBase64String(salt));
                        sb.Append(";");
                    }

                }
                return sb.ToString();
            }
            else
                throw new NoAESKeyException("No Masterkey found");
        }

        /// <summary>
        /// Generates a string containing all questions in HTML.
        /// </summary>
        /// <returns>Returns a string containing all questions in HTML</returns>
        private string generateQuestionsHTML()
        {
            StringBuilder sb = new StringBuilder();

            if (DataProvider.getInstance().examDetails.viewMode == ViewMode.PAGE)
            {
                sb.Append("<div id=\"nextQuestion\"><button onclick=\"nextQuestion()\">Weiter</button></div>");
                sb.Append("<div id=\"prevQuestion\"><button onclick=\"prevQuestion()\">Zur&uuml;ck</button></div>");
            }
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
