using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecureExam
{
    [TestClass]
    public class ProfessorTest
    {
        [TestMethod]
        public void testName()
        {
            Professor prof = new Professor("rege");
            Assert.AreEqual("rege", prof.name);
        }

        [TestMethod]
        public void testSecretLength()
        {
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            String name = "rege";
            Professor prof = new Professor(name);
            Assert.AreEqual(name.Length, prof.secret.Length);
        }

        [TestMethod]
        public void testProfessorParticipantSecretLength()
        {
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            String name = "rege";
            Professor prof = new Professor(name);
            Assert.AreEqual(name.Length + BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret, prof.ParticipantSecret.Length);
        }

        [TestMethod]
        public void testProfessorSecretIsStatic()
        {
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            String name = "rege";
            Professor prof = new Professor(name);
            Assert.AreEqual(prof.ParticipantSecret, prof.ParticipantSecret);
        }
    }
}
