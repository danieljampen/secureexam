using System;
using System.Collections.Generic;

namespace SecureExam
{
    interface ISettingsParser
    {
        ExamDetails parse(String settingsPath);
    }
}
