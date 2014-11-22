using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SecureExam
{
    public class BasicSettings
    {
        //members
        private static BasicSettings instance;
        public int NumberOfRandomCharsInStudentSecret { get; set; }
        public Dictionary<OutputType, String> exportSkeletons { get; set; }

        public Encryption Encryption = new Encryption();

        // singleton
        private BasicSettings()
        {
            this.exportSkeletons = new Dictionary<OutputType, String>();
            exportSkeletons.Add(OutputType.HTMLJS, @"skeletons\htmljs.html");
        }

        // methods
        public static BasicSettings getInstance()
        {
            if (instance == null)
            {
                instance = new BasicSettings();
            }
            return instance;
        }
    }

    public class Encryption
    {
        public AESSettings AES = new AESSettings();
        public SHA256Setttings SHA256 = new SHA256Setttings();
    }

    public class AESSettings
    {
        public int KeyLength { get; set; }
        public int IvLength { get; set; }
        public Byte[] questionsAESKey { get; set; }
        public Byte[] questionsAESKeyIV { get; set; }
    }

    public class SHA256Setttings
    {
        public int Iterations { get; set; }
        public int SaltLength { get; set; }
        public int Length { get; set; }
    }
}
