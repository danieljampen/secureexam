using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class BasicSettings
    {
        //members
        private static BasicSettings instance;
        private string professor;
        private string subject;
        private string examTitle;
        private Path formularFilePath;
        private Path studentFilePath;
        private int numberOfRandomCharsInStudentSecret;
        private int aesKeyLength;
        private int pbkdf2Iterations;

        // getter n setter
        public string Professor
        {
            get { return professor; }
            set { professor = value; }
        }
        public string Subject 
        { 
            get { return subject; }; 
            set { subject = value; }; 
        }
        public string ExamTitle 
        { 
            get { return examTitle; }; 
            set { examTitle = value; }; 
        }
        public Path FormularFilePath 
        { 
            get { return formularFilePath; }; 
            set { formularFilePath = value; }; 
        }
        public Path StudentFilePath 
        { 
            get { return studentFilePath; }; 
            set { studentFilePath = value; }; 
        }
        public int NumberOfRandomCharsInStudentSecret 
        { 
            get { return numberOfRandomCharsInStudentSecret; }; 
            set { numberOfRandomCharsInStudentSecret = value; }; 
        }
        public int AESKeyLength 
        { 
            get { return aesKeyLength; }; 
            set { aesKeyLength = value; }; 
        }
        public int PBKDF2Iterations 
        { 
            get { return pbkdf2Iterations; }; 
            set { pbkdf2Iterations = value; }; 
        }
        

        // singleton
        private BasicSettings() { }

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
}
