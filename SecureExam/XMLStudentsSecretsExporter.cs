using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace SecureExam
{
    public class XMLStudentsSecretsExporter : IStudentsSecretExport
    {
        public void export(string filename)
        {
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ParticipantSecrets");

                foreach (Participant student in DataProvider.getInstance().Participants)
                {
                    writer.WriteStartElement("Participant");
                    if (student.GetType() == typeof(Student))
                    {
                        writer.WriteElementString("Vorname", ((Student)student).preName);
                        writer.WriteElementString("Nachname", ((Student)student).surName);
                        writer.WriteElementString("Immatrikulationsnummer", ((Student)student).ID);
                        writer.WriteElementString("Passwort", ((Student)student).secret);
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
