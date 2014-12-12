using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Provides all data which is used to generate an exam
    /// </summary>
    public class Facade
    {
        /// <summary>
        /// Exports 2 different files
        /// 1. exam output file to a path using an export type
        /// 2. students output file with secrets to same path using its own file format
        /// </summary>
        /// <param name="type">Type of the exam output file</param>
        /// <param name="path">Path of the exam output file</param>
        /// <param name="studentSecretsFileFormat">Type of the students output file, containing passwords for students</param>
        public void export(OutputType type, String path, StudentSecretsFileFormat studentSecretsFileFormat) {
            DataProvider.getInstance().export(type, path, studentSecretsFileFormat);
        }

        /// <summary>
        /// Reads Data from 3 different files
        /// 1. exam questions import formular
        /// 2. students file
        /// 3. settings file
        /// </summary>
        /// <param name="formularType">Type of the exam questions import formular (XML or ODT)</param>
        /// <param name="formularPath">Path of the exam questions import formular</param>
        /// <param name="studentType">Type of the students file (XML)</param>
        /// <param name="studentPath">Path of the students file</param>
        /// <param name="settingsPath">Path of the settings file</param>
        public void readData(QuestionFormularType formularType, String formularPath, StudentFileType studentType, String studentPath, String settingsPath) {
            DataProvider.getInstance().readData(formularType, formularPath, studentType, studentPath, settingsPath);
        }
    }
}
