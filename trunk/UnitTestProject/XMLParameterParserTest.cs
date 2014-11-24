using System;
using SecureExam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class XMLParameterParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void parseWrongParameterFileNameTest()
        {
            XMLStudentParser parser = new XMLStudentParser();
            parser.parse("invalid.xml");
        }

        [TestMethod]
        public void parseTestFile()
        {
            XMLParameterParser parser = new XMLParameterParser();
            parser.parse("SecureExam.xml");
            
            /*
            ExamDetails examDetails = parser.parse("settingsTest.xml");

            Assert.AreEqual(true , examDetails.confirmAutosaveRestore);
            Assert.AreEqual(false , examDetails.ebookreaderExport);
            Assert.AreEqual(10 , examDetails.examDurationMinutes);
            Assert.AreEqual("Es sind mehrere Antworten möglich" , examDetails.examNotes);
            Assert.AreEqual("Algorithmen und Datenstrukturen" , examDetails.examTitle);
            Assert.AreEqual(5000 , examDetails.historyTimeMaxVariance);
            Assert.AreEqual(5000, examDetails.internalTimeMaxVariance);
            Assert.AreEqual(true , examDetails.internetAllowed);
            Assert.AreEqual("ADS" , examDetails.subject);
            Assert.AreEqual(false , examDetails.tabChangeAllowed);

            DateTime examStartTime = new DateTime(2014, 11, 19, 1, 0, 0);
            DateTime examEndTime = new DateTime(2014, 11, 19, 23, 0, 0);
            Assert.AreEqual(examEndTime, examDetails.examEndTime);
            Assert.AreEqual(examStartTime, examDetails.examStartTime);
             * */
        }
        /*
        [TestMethod]
        [ExpectedException(typeof(InvalidTimeException))]
        public void timeExceptionTest()
        {
            XMLSettingsParser parser = new XMLSettingsParser();
            ExamDetails examDetails = parser.parse("settingsTestTimeException.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidImportException))]
        public void tagExceptionTest()
        {
            XMLSettingsParser parser = new XMLSettingsParser();
            ExamDetails examDetails = parser.parse("settingsTestTagException.xml");
        }
         * */
    }
}
