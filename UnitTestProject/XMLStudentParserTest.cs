using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace SecureExam
{
    [TestClass]
    public class XMLStudentParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void parseWrongStudentsFileNameTest()
        {
            XMLStudentParser parser = new XMLStudentParser();
            parser.parse("invalid.xml");
        }

        [TestMethod]
        public void parseTestFile()
        {
            XMLStudentParser parser = new XMLStudentParser();
            LinkedList<Participant> list = parser.parse("Files/studentsTest.xml");
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            foreach (Participant p in list)
            {
                if( p.GetType() == typeof(Professor) )
                {
                    Professor professor = (Professor)p;
                    Assert.AreEqual("Karl", professor.preName);
                    Assert.AreEqual("Rege", professor.surName);
                    Assert.AreEqual(18, professor.ParticipantSecret.Length);
                }
                else if (p.GetType() == typeof(Student))
                {
                    Assert.AreEqual("Daniel", ((Student)p).preName);
                    Assert.AreEqual("Jampen", ((Student)p).surName);
                    Assert.AreEqual("S12198320", ((Student)p).ID);
                    Assert.AreEqual(31, ((Student)p).ParticipantSecret.Length);
                }
            }
        }
    }
}
