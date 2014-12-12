using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SecureExam
{
    /// <summary>
    /// Contains the basic settings for the exam
    /// </summary>
    public class BasicSettings
    {
        //members
        private static BasicSettings instance;

        /// <summary>
        /// Defines number of random chars for student secret
        /// </summary>
        public int NumberOfRandomCharsInStudentSecret { get; set; }

        /// <summary>
        /// Matches strings to its output type
        /// </summary>
        public Dictionary<OutputType, String> exportSkeletons { get; set; }

        /// <summary>
        /// Creates Encryption
        /// </summary>
        public Encryption Encryption = new Encryption();

        /// <summary>
        /// Construtor of BasicSettings, initiates singleton
        /// </summary>
        private BasicSettings()
        {
            this.exportSkeletons = new Dictionary<OutputType, String>();
            exportSkeletons.Add(OutputType.HTMLJS, @"skeletons\htmljs.html");
        }

        /// <summary>
        /// Returns instance of singleton BasicSettings
        /// </summary>
        /// <returns>Returns instance of BasicSettings</returns>
        public static BasicSettings getInstance()
        {
            if (instance == null)
            {
                instance = new BasicSettings();
            }
            return instance;
        }
    }

    /// <summary>
    /// Creates instance of AES and SHA256
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// Creates AES settings
        /// </summary>
        public AESSettings AES = new AESSettings();

        /// <summary>
        /// Creates SHA256 settings
        /// </summary>
        public SHA256Setttings SHA256 = new SHA256Setttings();
    }

    /// <summary>
    /// Contains settings parameters for AES
    /// </summary>
    public class AESSettings
    {
        /// <summary>
        /// Defines the keyLength
        /// </summary>
        public int KeyLength { get; set; }

        /// <summary>
        /// Defines the initiaization vector
        /// </summary>
        public int IvLength { get; set; }

        /// <summary>
        /// Defines AES key
        /// </summary>
        public Byte[] questionsAESKey { get; set; }

        /// <summary>
        /// Defines AES key initiaization vector
        /// </summary>
        public Byte[] questionsAESKeyIV { get; set; }
    }

    /// <summary>
    /// Contains settings parameters for SHA256
    /// </summary>
    public class SHA256Setttings
    {
        /// <summary>
        /// Sets number of iterations
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Defines salt length
        /// </summary>
        public int SaltLength { get; set; }
    }
}
