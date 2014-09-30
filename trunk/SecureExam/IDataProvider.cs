using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    interface IDataProvider
    {
        bool readData(FormularType fromularType, Path formularPath, StudentFileType studentType, Path studentPath);
        bool export(ExportType type, Path path);
    }
}
