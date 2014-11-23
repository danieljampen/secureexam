using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecureExam
{
    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ByteArrayToHexArrayTestNull()
        {
            Helper.ByteArrayToHexString(null);
        }
        
        [TestMethod]
        public void ByteArrayToHexArrayTest()
        {
            Byte[] array = {0x20,0x20,0x25};
            Assert.AreEqual("202025",Helper.ByteArrayToHexString(array));
        }
    }
}
