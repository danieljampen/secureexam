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
    public class OdtFormularParser : IFormularParser
    {
        public LinkedList<Question> parse(StreamReader streamReader)
        {
            string xslFileContent = Properties.Resources.odt;
            XmlReader xmlReaderXslFile = XmlReader.Create(new StringReader(xslFileContent));

            string fileContent = streamReader.ReadToEnd();
            StringBuilder xmlOutput = generateXML(xmlReaderXslFile, fileContent);

            string xsl2FileContent = Properties.Resources.odt2;
            XmlReader xmlReaderXslFile2 = XmlReader.Create(new StringReader(xsl2FileContent));

            xmlOutput = generateXML(xmlReaderXslFile2, xmlOutput.ToString());

            XMLFormularParser xmlFormularParser = new XMLFormularParser();
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(xmlOutput.ToString());
            streamWriter.Flush();
            memoryStream.Position = 0;

            return xmlFormularParser.parse(new StreamReader(memoryStream));
        }

        private StringBuilder generateXML(XmlReader xmlReaderXSLT, string xml)
        {
            XslCompiledTransform xt = new XslCompiledTransform();
            StringBuilder resultString = new StringBuilder();
            XmlWriter xmlOutput = XmlWriter.Create(resultString);
            XmlReader xmlReader = XmlReader.Create(new StringReader(xml));

            xt.Load(xmlReaderXSLT);
            xt.Transform(xmlReader, xmlOutput);

            return resultString;
        }
    }
}
