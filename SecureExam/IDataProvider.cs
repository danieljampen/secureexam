using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    interface IDataProvider
    {
        bool readData();
        bool export(exportType type, string filename);
    }
}
