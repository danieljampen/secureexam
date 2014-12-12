using System;
using System.Collections.Generic;

namespace SecureExam
{
    /// <summary>
    /// Interface for SettingsParser<para />
    /// Is used to parse the settings file, which contains details about the exam.
    /// </summary>
    public interface ISettingsParser
    {
        /// <summary>
        /// Parses the settings file, path is given.
        /// </summary>
        /// <param name="settingsPath">Path of the settings file</param>
        /// <returns>Returns Exam details</returns>
        ExamDetails parse(String settingsPath);
    }
}