using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SecureExam
{
    class BasicSettings
    {
        //members
        private static BasicSettings instance;
        public string Professor { get; set; }
        public string Subject { get; set; }
        public string ExamTitle  { get; set; }
        public int NumberOfRandomCharsInStudentSecret { get; set; }
        public int PBKDF2Iterations  { get; set; }
        public Dictionary<OutputType,String> exportSkeletons { get; set; }
        public Encryption Encryption = new Encryption();
        

        // singleton
        private BasicSettings() 
        {
            this.exportSkeletons = new Dictionary<OutputType, String>();
            exportSkeletons.Add(OutputType.HTMLJS, @"skeletons\htmljs.html");

            this.NumberOfRandomCharsInStudentSecret = 10;
        }

        // methods
        public static BasicSettings getInstance()
        {
            if( instance == null )
            {
                instance = new BasicSettings();
            }
            return instance;
        }
    }

    class Encryption
    {
        public AESSettings AES = new AESSettings();
        public PBKDF2Setttings PBKDF2 = new PBKDF2Setttings();
    }

    class AESSettings
    {
        public int KEYLENGTH = 256;
        public Byte[] questionsAESKey { get; set; }
        public Byte[] questionsAESKeyIV { get; set; }
    }

    class PBKDF2Setttings
    {
        public int ITERATIONS = 1000;
        public int SALTLENGTH = 256/8;
        public int LENGTH = 256/8;
    }
}
