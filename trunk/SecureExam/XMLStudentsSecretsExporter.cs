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
        void IStudentsSecretExport.export(string filename)
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
                        if( student.GetType() == typeof(Student)) {
                            writer.WriteElementString("Vorname", ((Student)student).studentPreName);
                            writer.WriteElementString("Nachname", ((Student)student).studentSurName);
                            writer.WriteElementString("Immatrikulationsnummer", ((Student)student).studentID);
                            writer.WriteElementString("Passwort", ((Student)student).secret);
                        }
                        else if (student.GetType() == typeof(Professor))
                        {
                            writer.WriteElementString("Name", ((Professor)student).preName);
                            writer.WriteElementString("Name", ((Professor)student).surName);
                            writer.WriteElementString("Passwort", ((Professor)student).secret);
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch( Exception e)
            {
                throw e;
            }
        }
    }
}
