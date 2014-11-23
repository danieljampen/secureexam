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
            Professor prof = new Professor("Karl", "Rege");
            Assert.AreEqual("Rege", prof.surName);
            Assert.AreEqual("Karl", prof.preName);
        }

        [TestMethod]
        public void testSecretLength()
        {
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            String preName = "Karl";
            String surName = "Rege";
            Professor prof = new Professor(preName, surName);
            Assert.AreEqual(18, prof.ParticipantSecret.Length);
        }

        [TestMethod]
        public void testProfessorParticipantSecretLength()
        {
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;

            String preName = "Karl";
            String surName = "Rege";
            Professor prof = new Professor(preName, surName);
            Assert.AreEqual(surName.Length + preName.Length + BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret, prof.ParticipantSecret.Length);
        }

        [TestMethod]
        public void testProfessorSecretIsStatic()
        {
            BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = 10;
            
            String preName = "Karl";
            String surName = "Rege";
            Professor prof = new Professor(preName, surName);
            Assert.AreEqual(prof.ParticipantSecret, prof.ParticipantSecret);
        }
    }
}
