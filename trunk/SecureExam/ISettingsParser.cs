using System;
using System.Collections.Generic;

namespace SecureExam
{
    public interface ISettingsParser
    {
        ExamDetails parse(String settingsPath);
    }
}
