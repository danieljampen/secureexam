using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace SecureExam
{
    class XMLStudentsSecretsExporter: IStudentsSecretExport
    {
        bool IStudentsSecretExport.export(string filename)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("StudentsSecrets");

                    foreach (Student student in DataProvider.getInstance().Students)
                    {
                        writer.WriteStartElement("Student");

                        writer.WriteElementString("Secret", student.StudentSecret);

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch( Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
