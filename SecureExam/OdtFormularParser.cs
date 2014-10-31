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
        public LinkedList<Question> parse(String formularPath)
        {
            // XSLT odt -> xml dann mit xml parser einlesen

            /*
            string fileContents = System.IO.File.ReadAllText(formularPath);

            fileContents = fileContents.Replace("<text:", "<text");
            fileContents = fileContents.Replace("</text:", "</text");

            
            fileContents = fileContents.Replace("<form:", "<form");
            fileContents = fileContents.Replace("</form:", "</form");

            fileContents = fileContents.Replace("form:id", "formid");
            fileContents = fileContents.Replace("form:label", "formlabel");

            fileContents = fileContents.Replace("draw:control", "drawcontrol");

            System.IO.File.WriteAllText(formularPath, fileContents);
            */


            
            string xslFilePath = "C:/Users/Simon/Documents/ZHAW/Projekt/Files/Beispieldateien/Formulare/ODT/odt.xslt";

            //XslTransform xt = new XslTransform();
            XslCompiledTransform xt = new XslCompiledTransform();
            xt.Load(xslFilePath);
            xt.Transform(formularPath, "test2.xml");
            
            /*TODO: XML ausgabe in string umleiten und an xmlformularparser übergeben.
             * */

            throw new NotImplementedException();
        }
    }
}
