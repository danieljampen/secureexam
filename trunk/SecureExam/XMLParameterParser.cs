using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SecureExam
{
    public class XMLParameterParser : IParameterParser
    {
        private XmlDocument xmlDoc;

        public void parse(String parameterPath)
        {
            this.xmlDoc = new XmlDocument();
            this.xmlDoc.Load(parameterPath);

            string NumberOfRandomCharsInStudentSecret = getElementByTagName("NumberOfRandomCharsInStudentSecret");
            if (NumberOfRandomCharsInStudentSecret != "")
            {
                BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = int.Parse(NumberOfRandomCharsInStudentSecret);
            }
            else
            {
                throw new InvalidImportException("<NumberOfRandomCharsInStudentSecret> is not set or invalid");
            }

            //AES
            if (this.xmlDoc.GetElementsByTagName("AESSettings").Count > 0)
            {
                foreach (XmlNode node in this.xmlDoc.GetElementsByTagName("AESSettings")[0].ChildNodes)
                {
                    if (node.Name == "keyLength")
                    {
                        BasicSettings.getInstance().Encryption.AES.KeyLength = int.Parse(node.InnerText);
                    }
                    else if (node.Name == "ivLength")
                    {
                        BasicSettings.getInstance().Encryption.AES.IvLength = int.Parse(node.InnerText);
                    }
                }
            }
            else
            {
                throw new InvalidImportException("AESSettings are not set");
            }

            //SHA
            if (this.xmlDoc.GetElementsByTagName("SHA256Setttings").Count > 0)
            {
                foreach (XmlNode node in this.xmlDoc.GetElementsByTagName("SHA256Setttings")[0].ChildNodes)
                {
                    if (node.Name == "iterations")
                    {
                        BasicSettings.getInstance().Encryption.SHA256.Iterations = int.Parse(node.InnerText);
                    }
                    else if (node.Name == "saltLength")
                    {
                        BasicSettings.getInstance().Encryption.SHA256.SaltLength = int.Parse(node.InnerText);
                    }
                }
            }
            else
            {
                throw new InvalidImportException("SHA256Setttings are not set");
            }
        }

        private string getElementByTagName(string tag)
        {
            string result = "";
            if (this.xmlDoc.GetElementsByTagName(tag).Count > 0)
            {
                result = this.xmlDoc.GetElementsByTagName(tag)[0].InnerText;
            }
            return result;
        }
    }
}
