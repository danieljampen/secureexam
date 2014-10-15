using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class OdtFormularParser:IFormularParser
    {
        public LinkedList<Question> parse(String formularPath)
        {
            // XSLT odt -> xml dann mit xml parser einlesen
            throw new NotImplementedException();
        }
    }
}
