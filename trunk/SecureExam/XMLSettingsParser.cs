using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SecureExam
{
    class XMLSettingsParser : ISettingsParser
    {
        private XmlDocument xmlDoc;

        public ExamDetails parse(String settingsPath){
            ExamDetails examDetails = DataProvider.getInstance().examDetails;

            try
            {
                this.xmlDoc = new XmlDocument();
                this.xmlDoc.Load(settingsPath);

                DataProvider.getInstance().examDetails.examTitle = getElementByTagName("examTitle");
                DataProvider.getInstance().examDetails.subject = getElementByTagName("subject");
                DataProvider.getInstance().examDetails.examNotes = getElementByTagName("examNotes");

                DataProvider.getInstance().examDetails.examDurationMinutes = int.Parse(getElementByTagName("duration"));

                string examDateString = getElementByTagName("examDate");
                string examStartTimeString = getElementByTagName("startTime");
                string examEndTimeString = getElementByTagName("endTime");

                DateTime examDateTime = Convert.ToDateTime(examDateString);
                DateTime examStartTime = Convert.ToDateTime(examStartTimeString);
                DateTime examEndTime = Convert.ToDateTime(examEndTimeString);

                examStartTime = new DateTime(examDateTime.Year, examDateTime.Month, examDateTime.Day, examStartTime.Hour, examStartTime.Minute, 0);
                examEndTime = new DateTime(examDateTime.Year, examDateTime.Month, examDateTime.Day, examEndTime.Hour, examEndTime.Minute, 0);

                DataProvider.getInstance().examDetails.examStartTime = examStartTime;
                DataProvider.getInstance().examDetails.examEndTime = examEndTime;
            }
            catch (DirectoryNotFoundException e)
            {
                throw new NotImplementedException(e.ToString());
            }
            catch (XmlException e)
            {
                throw new NotImplementedException(e.ToString());
            }

            return examDetails;
        }

        private string getElementByTagName(string tag)
        {
            string result = "";
            if (this.xmlDoc.GetElementsByTagName(tag).Count > 0)
            {
                result = this.xmlDoc.GetElementsByTagName(tag)[0].InnerText;
            }
            return result;
        }
    }
}
