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
                        if( student.GetType() == typeof(Student)) {
                            Student stud = (Student)student;
                            writer.WriteElementString("Vorname", stud.studentPreName);
                            writer.WriteElementString("Nachname", stud.studentSurName);
                            writer.WriteElementString("Immatrikulationsnummer", stud.studentID);
                            writer.WriteElementString("Passwort", stud.StudentSecret);
                        }
                        else if (student.GetType() == typeof(Professor))
                        {
                            Professor prof = (Professor)student;
                            writer.WriteElementString("Name", prof.name);
                            writer.WriteElementString("Passwort", prof.StudentSecret);
                        }


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
