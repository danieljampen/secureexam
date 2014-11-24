using System;
using SecureExam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class XMLSettingsParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void parseWrongSettingsFileNameTest()
        {
            XMLStudentParser parser = new XMLStudentParser();
            parser.parse("invalid.xml");
        }

        [TestMethod]
        public void parseTestFile()
        {
            XMLSettingsParser parser = new XMLSettingsParser();
            ExamDetails examDetails = parser.parse("Files/settingsTest.xml");

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
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimeException))]
        public void timeExceptionTest()
        {
            XMLSettingsParser parser = new XMLSettingsParser();
            ExamDetails examDetails = parser.parse("Files/settingsTestTimeException.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidImportException))]
        public void tagExceptionTest()
        {
            XMLSettingsParser parser = new XMLSettingsParser();
            ExamDetails examDetails = parser.parse("Files/settingsTestTagException.xml");
        }
    }
}
