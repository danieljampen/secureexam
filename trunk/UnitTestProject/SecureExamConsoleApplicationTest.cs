using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using System.IO;

namespace SecureExam
{
    [TestClass]
    public class SecureExamConsoleApplicationTest
    {
        [TestMethod]
        public void printErrorTest()
        {
            using(ShimsContext.Create())
            {
                // fake
                System.Fakes.ShimDateTime.NowGet = () => { return new DateTime(2014, 1, 1, 20, 0, 0); };

                // init
                using(StringWriter sw = new StringWriter()) 
                {
                    Console.SetOut(sw);
                    String expected = "[ERROR] 2014-01-01 20:00:00Z: TEST" + Environment.NewLine;

                    // run
                    SecureExamConsoleApplication.printError("TEST");

                    // assert
                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void printErrorTestMessageNull()
        {
            SecureExamConsoleApplication.printError(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void printErrorTestMessageLengthZero()
        {
            SecureExamConsoleApplication.printError("");
        }

        [TestMethod]
        public void printUsageTest()
        {
            using( StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                String expected = "Error: invalid arguments\r\n\r\nusage: secureExam -q questionFile -s studentsFile -o Outputfile\r\n       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o Outputfile [-oType OutputFileType] [-oStudentSecretsFileFormat studentSecretsFileFormat] -p SettingsFile\r\n\r\nQuestionFileTypes: XML, ODT\r\nStudentFileTypes: XML\r\nOutputFileTypes: HTMLJS\r\nStudentSecretsFileFormat: XML\r\n";
                SecureExamConsoleApplication.printUsage();

                Assert.AreEqual(expected, sw.ToString());
            }
        }

        [TestMethod]
        public void mainTest()
        {
            using (ShimsContext.Create())
            {
                SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                String[] args = { "-q", "questions.xml", "-qType", "XML", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                Assert.AreEqual(0, SecureExamConsoleApplication.Main(args));
            }
        }

        [TestMethod]
        public void mainTestFullArguments()
        {
            using (ShimsContext.Create())
            {
                SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                String[] args = { "-q", "questions.xml", "-qType", "odt", "-s", "students.xml", "-o", "outputExam.html", "-oType", "htmljs", "-p", "settings.xml", "-oStudentSecretsFileFormat", "xml" };
                Assert.AreEqual(0, SecureExamConsoleApplication.Main(args));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void mainTestArgsNull()
        {
            using (ShimsContext.Create())
            {
                SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                String[] args = null;
                SecureExamConsoleApplication.Main(args);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void mainTestTooFewArguments()
        {
            using (ShimsContext.Create())
            {
                SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                String[] args = { "-q", "questions.xml" };
                SecureExamConsoleApplication.Main(args);
            }
        }

        [TestMethod]
        public void mainTestQTypeInvalid()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    String expected = "Error: invalid arguments\r\n\r\nusage: secureExam -q questionFile -s studentsFile -o Outputfile\r\n       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o Outputfile [-oType OutputFileType] [-oStudentSecretsFileFormat studentSecretsFileFormat] -p SettingsFile\r\n\r\nQuestionFileTypes: XML, ODT\r\nStudentFileTypes: XML\r\nOutputFileTypes: HTMLJS\r\nStudentSecretsFileFormat: XML\r\n";

                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                    String[] args = { "-q", "questions.xml", "-qType", "ASD", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        public void mainTestArgumentKeyInvalid()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    String expected = "Error: invalid arguments\r\n\r\nusage: secureExam -q questionFile -s studentsFile -o Outputfile\r\n       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o Outputfile [-oType OutputFileType] [-oStudentSecretsFileFormat studentSecretsFileFormat] -p SettingsFile\r\n\r\nQuestionFileTypes: XML, ODT\r\nStudentFileTypes: XML\r\nOutputFileTypes: HTMLJS\r\nStudentSecretsFileFormat: XML\r\n";

                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                    String[] args = { "-q", "questions.xml", "-qTypeeeee", "xml", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        public void mainTestSTypeInvalid()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    String expected = "Error: invalid arguments\r\n\r\nusage: secureExam -q questionFile -s studentsFile -o Outputfile\r\n       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o Outputfile [-oType OutputFileType] [-oStudentSecretsFileFormat studentSecretsFileFormat] -p SettingsFile\r\n\r\nQuestionFileTypes: XML, ODT\r\nStudentFileTypes: XML\r\nOutputFileTypes: HTMLJS\r\nStudentSecretsFileFormat: XML\r\n";

                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                    String[] args = { "-q", "questions.xml", "-SType", "ASD", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        public void mainTestOTypeInvalid()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    String expected = "Error: invalid arguments\r\n\r\nusage: secureExam -q questionFile -s studentsFile -o Outputfile\r\n       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o Outputfile [-oType OutputFileType] [-oStudentSecretsFileFormat studentSecretsFileFormat] -p SettingsFile\r\n\r\nQuestionFileTypes: XML, ODT\r\nStudentFileTypes: XML\r\nOutputFileTypes: HTMLJS\r\nStudentSecretsFileFormat: XML\r\n";

                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                    String[] args = { "-q", "questions.xml", "-OType", "ASD", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        public void mainTestStudentSecretsFileFormatInvalid()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    String expected = "Error: invalid arguments\r\n\r\nusage: secureExam -q questionFile -s studentsFile -o Outputfile\r\n       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o Outputfile [-oType OutputFileType] [-oStudentSecretsFileFormat studentSecretsFileFormat] -p SettingsFile\r\n\r\nQuestionFileTypes: XML, ODT\r\nStudentFileTypes: XML\r\nOutputFileTypes: HTMLJS\r\nStudentSecretsFileFormat: XML\r\n";

                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { };

                    String[] args = { "-q", "questions.xml", "-qType", "xml", "-oStudentSecretsFileFormat", "ASD", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void mainTestFilesNotFound()
        {
            using (ShimsContext.Create())
            {
                SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { throw new FileNotFoundException(); };

                String[] args = { "-q", "questions.xml", "-sType", "xml", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                SecureExamConsoleApplication.Main(args);
            }
        }

        [TestMethod]
        public void mainTestDataReadException()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // fake
                    System.Fakes.ShimDateTime.NowGet = () => { return new DateTime(2014, 1, 1, 20, 0, 0); };
                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { throw new DataReadException(""); };



                    String[] args = { "-q", "questions.xml", "-sType", "xml", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    String expected = "[ERROR] 2014-01-01 20:00:00Z: Data Read failed: \r\n";
                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        public void mainTestExportException()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // fake
                    System.Fakes.ShimDateTime.NowGet = () => { return new DateTime(2014, 1, 1, 20, 0, 0); };
                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { throw new ExportException(""); };



                    String[] args = { "-q", "questions.xml", "-sType", "xml", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    String expected = "[ERROR] 2014-01-01 20:00:00Z: Export failed: \r\n";
                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

        [TestMethod]
        public void mainTestNotImplementedException()
        {
            using (ShimsContext.Create())
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // fake
                    System.Fakes.ShimDateTime.NowGet = () => { return new DateTime(2014, 1, 1, 20, 0, 0); };
                    SecureExam.Fakes.ShimFacade.AllInstances.exportOutputTypeStringStudentSecretsFileFormat = (a, b, c, d) => { };
                    SecureExam.Fakes.ShimFacade.AllInstances.readDataQuestionFormularTypeStringStudentFileTypeStringString = (a, b, c, d, e, f) => { throw new NotImplementedException(""); };



                    String[] args = { "-q", "questions.xml", "-sType", "xml", "-s", "students.xml", "-o", "outputExam.html", "-p", "settings.xml" };
                    SecureExamConsoleApplication.Main(args);

                    String expected = "[ERROR] 2014-01-01 20:00:00Z: Unimplemented method called! \r\n";
                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }

    }
}
