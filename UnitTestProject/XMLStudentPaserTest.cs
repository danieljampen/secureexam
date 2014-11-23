using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace SecureExam
{
    [TestClass]
    public class XMLStudentPaserTest
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void parseWrongFileNameTest()
        {
            XMLStudentParser parser = new XMLStudentParser();
            parser.parse("invalid.xml");
        }

        [TestMethod]
        public void parseTestFile()
        {
            XMLStudentParser parser = new XMLStudentParser();
            LinkedList<Participant> list = parser.parse("studentsTest.xml");
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;


            foreach (Participant p in list)
            {
                if( p.GetType() == typeof(Professor) ){
                    Assert.AreEqual("rege",((Professor)p).name);
                    Assert.AreEqual(10, ((Professor)p).secret.Length);
                }
                else if (p.GetType() == typeof(Student))
                {
                    Assert.AreEqual("Daniel", ((Student)p).studentPreName);
                    Assert.AreEqual("Jampen", ((Student)p).studentSurName);
                    Assert.AreEqual("S12198320", ((Student)p).studentID);
                    Assert.AreEqual(10, ((Student)p).secret.Length);
                }
            }
        }
        
    }
}
