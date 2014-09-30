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
        public string Professor { get; set; }
        public string Subject { get; set; }
        public string ExamTitle  { get; set; }
        public int NumberOfRandomCharsInStudentSecret  { get; set; }
        public int AESKeyLength  { get; set; }
        public int PBKDF2Iterations  { get; set; }
        public Dictionary<OutputType,String> exportSkeletons { get; set; }
        

        // singleton
        private BasicSettings() 
        {
            this.exportSkeletons = new Dictionary<OutputType, String>();
            exportSkeletons.Add(OutputType.HTMLJS, System.Environment.CurrentDirectory + @"skeletons\htmljs.html");
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
}
