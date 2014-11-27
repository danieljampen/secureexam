using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SecureExam
{
    public class XMLSettingsParser : ISettingsParser
    {
        private XmlDocument xmlDoc;

        public ExamDetails parse(String settingsPath)
        {
            ExamDetails examDetails = DataProvider.getInstance().examDetails;

            this.xmlDoc = new XmlDocument();
            this.xmlDoc.Load(settingsPath);

            DataProvider.getInstance().examDetails.examTitle = getElementByTagName("examTitle");
            DataProvider.getInstance().examDetails.subject = getElementByTagName("subject");
            DataProvider.getInstance().examDetails.examNotes = getElementByTagName("examNotes");
            DataProvider.getInstance().examDetails.internetAllowed = setBoolean(getElementByTagName("internetAllowed"));
            DataProvider.getInstance().examDetails.tabChangeAllowed = setBoolean(getElementByTagName("tabChangeAllowed"));
            DataProvider.getInstance().examDetails.confirmAutosaveRestore = setBoolean(getElementByTagName("confirmAutosaveRestore"));
            DataProvider.getInstance().examDetails.ebookreaderExport = setBoolean(getElementByTagName("ebookreaderExport"));
            
            string viewMode = getElementByTagName("viewMode");
            if (viewMode.ToLower() == "page")
            {
                DataProvider.getInstance().examDetails.viewMode = ViewMode.PAGE;
            }
            else
            {
                DataProvider.getInstance().examDetails.viewMode = ViewMode.SCROLL;
            }

            string internalTimeMaxVariance = getElementByTagName("internalTimeMaxVariance");
            string historyTimeMaxVariance = getElementByTagName("historyTimeMaxVariance");
            string duration = getElementByTagName("duration");

            string examDateString = getElementByTagName("examDate");
            string examStartTimeString = getElementByTagName("startTime");
            string examEndTimeString = getElementByTagName("endTime");

            if (internalTimeMaxVariance == "")
            {
                throw new InvalidImportException("Tag <internalTimeMaxVariance> not set");
            }
            else if (historyTimeMaxVariance == "")
            {
                throw new InvalidImportException("Tag <historyTimeMaxVariance> not set");
            }
            else if (duration == "")
            {
                throw new InvalidImportException("Tag <duration> not set");
            }
            else if (examDateString == "")
            {
                throw new InvalidImportException("Tag <examDateString> not set");
            }
            else if (examStartTimeString == "")
            {
                throw new InvalidImportException("Tag <examStartTimeString> not set");
            }
            else if (examEndTimeString == "")
            {
                throw new InvalidImportException("Tag <examEndTimeString> not set");
            }
            else
            {
                DataProvider.getInstance().examDetails.internalTimeMaxVariance = int.Parse(getElementByTagName("internalTimeMaxVariance"));
                DataProvider.getInstance().examDetails.historyTimeMaxVariance = int.Parse(getElementByTagName("historyTimeMaxVariance"));
                DataProvider.getInstance().examDetails.examDurationMinutes = int.Parse(getElementByTagName("duration"));

                try
                {
                    DateTime examDateTime = Convert.ToDateTime(examDateString);
                    DateTime examStartTime = Convert.ToDateTime(examStartTimeString);
                    DateTime examEndTime = Convert.ToDateTime(examEndTimeString);

                    examStartTime = new DateTime(examDateTime.Year, examDateTime.Month, examDateTime.Day, examStartTime.Hour, examStartTime.Minute, 0);
                    examEndTime = new DateTime(examDateTime.Year, examDateTime.Month, examDateTime.Day, examEndTime.Hour, examEndTime.Minute, 0);

                    DataProvider.getInstance().examDetails.examStartTime = examStartTime;
                    DataProvider.getInstance().examDetails.examEndTime = examEndTime;
                }
                catch (Exception e)
                {
                    throw new InvalidTimeException(e.ToString());
                }
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

        private bool setBoolean(string text)
        {
            if (text == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}