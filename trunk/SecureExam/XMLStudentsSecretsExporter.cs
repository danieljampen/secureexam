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
                    writer.WriteStartElement("ParticipantSecrets");

                    foreach (Participant student in DataProvider.getInstance().Participants)
                    {
                        writer.WriteStartElement("Participant");

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
