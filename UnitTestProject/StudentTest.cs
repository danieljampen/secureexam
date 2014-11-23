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
            Student stud = new Student();
            String name = "Lukes";
            stud.studentSurName = name;
            Assert.AreEqual(name ,stud.studentSurName);
        }
        [TestMethod]
        public void testVorname()
        {
            Student stud = new Student();
            String name = "Simon";
            stud.studentPreName = name;
            Assert.AreEqual(name, stud.studentPreName);
        }
        [TestMethod]
        public void testNr()
        {
            Student stud = new Student();
            String id = "S10290182";
            stud.studentID = id;
            Assert.AreEqual(id, stud.studentID);
        }
        [TestMethod]
        public void testSecretLength()
        {
            SecureExam.BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            Student stud = new Student();
            stud = new Student();
            String name = "Lukes";
            stud.studentSurName = name;
            String vorname = "Simon";
            stud.studentPreName = name;
            String id = "S10290182";
            stud.studentID = id;

            string secret = stud.StudentSecret;
            Assert.AreEqual(name.Length + vorname.Length + id.Length + SecureExam.BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret, secret.Length);
        }
        [TestMethod]
        public void testSecretStatic()
        {
            Student stud = new Student();
            stud = new Student();
            String name = "Lukes";
            stud.studentSurName = name;
            String vorname = "Simon";
            stud.studentPreName = vorname;
            String id = "S10290182";
            stud.studentID = id;

            string secret = stud.StudentSecret;
            string secret2 = stud.StudentSecret;
            Assert.AreEqual(secret, secret2);
        }
    }
}
