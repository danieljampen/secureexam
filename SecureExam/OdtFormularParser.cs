using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using System.IO.Compression;

namespace SecureExam
{
    class OdtFormularParser:IFormularParser
    {
        public LinkedList<Question> parse(StreamReader streamReader)
        {
            // XSLT odt -> xml dann mit xml parser einlesen
            string xslFilePath = "../../../../Files/Beispieldateien/Formulare/ODT/odt.xslt";
            string fileContent = streamReader.ReadToEnd();
            StringBuilder xmlOutput = generateXML(xslFilePath, fileContent);

            string xslFilePath2 = "../../../../Files/Beispieldateien/Formulare/ODT/odt2.xslt";
            xmlOutput = generateXML(xslFilePath2, xmlOutput.ToString());

            XMLFormularParser xmlFormularParser = new XMLFormularParser();

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(xmlOutput.ToString());
            streamWriter.Flush();
            memoryStream.Position = 0;

            return xmlFormularParser.parse(new StreamReader(memoryStream));
        }

        private StringBuilder generateXML(string xslFilePath, string xml)
        {
            XslCompiledTransform xt = new XslCompiledTransform();
            StringBuilder resultString = new StringBuilder();
            XmlWriter xmlOutput = XmlWriter.Create(resultString);
            XmlReader xmlReader = XmlReader.Create(new StringReader(xml));

            xt.Load(xslFilePath);
            xt.Transform(xmlReader, xmlOutput);

            return resultString;
        }
    }
}
