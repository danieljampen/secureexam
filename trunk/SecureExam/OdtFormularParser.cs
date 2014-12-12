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
    /// <summary>
    /// Uses the interface IFormularParser<para />
    /// Is used to parse the odt question import file.
    /// </summary>
    public class OdtFormularParser : IFormularParser
    {
        /// <summary>
        /// Parses an odt file by a given stream.
        /// </summary>
        /// <param name="streamReader">stream of the document to parse</param>
        /// <returns>Returns linkedList of questions</returns>
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

        /// <summary>
        /// Generates an XML string, using XSLT and an XML file.
        /// </summary>
        /// <param name="xmlReaderXSLT">XMLReader containing XSLT to load</param>
        /// <param name="xml">XML to import</param>
        /// <returns>Returns generated XML</returns>
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




