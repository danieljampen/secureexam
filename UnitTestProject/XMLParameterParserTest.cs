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
            parser.parse("Files/SecureExam.xml");
            //parser.parse("Files/SecureExamException.xml");

            Assert.AreEqual(10, BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret);
            Assert.AreEqual(128, BasicSettings.getInstance().Encryption.AES.IvLength);
            Assert.AreEqual(256, BasicSettings.getInstance().Encryption.AES.KeyLength);
            
            Assert.AreEqual(100000, BasicSettings.getInstance().Encryption.SHA256.Iterations);
            Assert.AreEqual(256, BasicSettings.getInstance().Encryption.SHA256.Length);
            Assert.AreEqual(256, BasicSettings.getInstance().Encryption.SHA256.SaltLength);
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
