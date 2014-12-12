using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Interface for DataProvder<para/>
    /// DataProvider provides all data which is used to generate an exam
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Reads Data from 3 different files<para/>
        /// 1. exam questions import formular<para/>
        /// 2. students file<para/>
        /// 3. settings file
        /// </summary>
        /// <param name="formularType">Type of the exam questions import formular (XML or ODT)</param>
        /// <param name="formularPath">Path of the exam questions import formular</param>
        /// <param name="studentType">Type of the students file (XML)</param>
        /// <param name="studentPath">Path of the students file</param>
        /// <param name="settingsPath">Path of the settings file</param>        
        void readData(QuestionFormularType formularType, String formularPath, StudentFileType studentType, String studentPath, String settingsPath);

        /// <summary>
        /// Exports 2 different files<para/>
        /// 1. exam output file to a path using an export type<para/>
        /// 2. students output file with secrets to same path using its own file format
        /// </summary>
        /// <param name="type">Type of the exam output file</param>
        /// <param name="path">Path of the exam output file</param>
        /// <param name="studentSecretsFileFormat">Type of the students output file, containing passwords for students</param>
        void export(OutputType type, String path, StudentSecretsFileFormat studentSecretsFileFormat);
    }
}
