using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace SecureExam
{
    class OdtFormularParser:IFormularParser
    {
        public LinkedList<Question> parseFile(String formularPath)
        {
            // XSLT odt -> xml dann mit xml parser einlesen

            string xslFilePath = "../../../../Files/Beispieldateien/Formulare/ODT/odt.xslt";
            
            XslCompiledTransform xt = new XslCompiledTransform();
            
            StringBuilder resultString = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(resultString);

            xt.Load(xslFilePath);
            xt.Transform(formularPath,writer);

            XMLFormularParser xmlFormularParser = new XMLFormularParser();
            return xmlFormularParser.parseXML(resultString.ToString());
        }
    }
}
