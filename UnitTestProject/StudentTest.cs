using System;
using SecureExam;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecureExam
{
    [TestClass]
    public class StudentTest
    {
        [TestMethod]
        public void testName()
        {
            Student stud = new Student("Simon", "Lukes", "S10290182");
            Assert.AreEqual("Lukes" ,stud.studentSurName);
        }
        [TestMethod]
        public void testVorname()
        {
            Student stud = new Student("Simon", "Lukes", "S10290182");
            Assert.AreEqual("Simon", stud.studentPreName);
        }
        [TestMethod]
        public void testID()
        {
            Student stud = new Student("Simon", "Lukes", "S10290182");
            Assert.AreEqual("S10290182", stud.studentID);
        }
        [TestMethod]
        public void testSecretLength()
        {
            SecureExam.BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            String name = "Lukes";
            String vorname = "Simon";
            String id = "S10290182";

            Student stud = new Student(vorname,name,id);

            Assert.AreEqual(name.Length + vorname.Length + id.Length + SecureExam.BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret, stud.ParticipantSecret.Length);
        }
        [TestMethod]
        public void testSecretStatic()
        {
            Student stud = new Student("Simon", "Lukes", "S10290182");

            Assert.AreEqual(stud.ParticipantSecret, stud.ParticipantSecret);
        }
    }
}
