using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SecureExam
{
    /// <summary>
    /// Uses interface IStudentsSecretExport<para />
    /// Is used to export the student details with secrets into a file.
    /// </summary>
    public class XMLStudentsSecretsExporter : IStudentsSecretExport
    {
        /// <summary>
        /// Exports student secrets file to a given filename
        /// </summary>
        /// <param name="filename">Filename, where to export the students secrets file</param>
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
