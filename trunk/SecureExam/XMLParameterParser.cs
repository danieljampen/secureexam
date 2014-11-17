using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SecureExam
{
    class XMLParameterParser : IParameterParser
    {
        private XmlDocument xmlDoc;

        public Boolean parse(String parameterPath)
        {
            try
            {
                this.xmlDoc = new XmlDocument();
                this.xmlDoc.Load(parameterPath);
                
                BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret = int.Parse(getElementByTagName("NumberOfRandomCharsInStudentSecret"));
                
                //AES
                BasicSettings.getInstance().Encryption.AES.KeyLength = int.Parse(getElementByTagName("keyLength"));
                BasicSettings.getInstance().Encryption.AES.IvLength = int.Parse(getElementByTagName("ivLength"));
                
                //SHA
                BasicSettings.getInstance().Encryption.SHA256.Iterations = int.Parse(getElementByTagName("iterations"));
                BasicSettings.getInstance().Encryption.SHA256.SaltLength = int.Parse(getElementByTagName("saltLength"));
                BasicSettings.getInstance().Encryption.SHA256.Length = int.Parse(getElementByTagName("length"));
                

            }
            catch (DirectoryNotFoundException e)
            {
                throw new NotImplementedException(e.ToString());
            }
            catch (XmlException e)
            {
                throw new NotImplementedException(e.ToString());
            }

            return true;
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
