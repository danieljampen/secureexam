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
        public LinkedList<Question> parse(StreamReader streamReader)
        {
            // XSLT odt -> xml dann mit xml parser einlesen

            string xslFilePath = "../../../../Files/Beispieldateien/Formulare/ODT/odt.xslt";
            
            XslCompiledTransform xt = new XslCompiledTransform();
            
            StringBuilder resultString = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(resultString);

            string fileContent = streamReader.ReadToEnd();
            XmlReader xmlReader = XmlReader.Create(new StringReader(fileContent));
            

            xt.Load(xslFilePath);
            xt.Transform(xmlReader, writer);

            string xslFilePath2 = "../../../../Files/Beispieldateien/Formulare/ODT/odt2.xslt";


            


            StringBuilder resultString2 = new StringBuilder();
            XmlWriter writer2 = XmlWriter.Create(resultString2);

            xt.Load(xslFilePath2);


            XmlReader xmlReader2 = XmlReader.Create(new StringReader(resultString.ToString()));
            
            
            xt.Transform(xmlReader2, writer2);

            XMLFormularParser xmlFormularParser = new XMLFormularParser();

            MemoryStream stream = new MemoryStream();
            StreamWriter writer22 = new StreamWriter(stream);
            writer22.Write(resultString2.ToString());
            writer22.Flush();
            stream.Position = 0;

            return xmlFormularParser.parse(new StreamReader(stream));
        }
    }
}
