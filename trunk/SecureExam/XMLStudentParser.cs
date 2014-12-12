using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Uses interface IStudentParser<para />
    /// Is used to parse the students file, which contains details about all students.
    /// </summary>
    public class XMLStudentParser : IStudentParser
    {
        /// <summary>
        /// Parses the students file, path is given.
        /// </summary>
        /// <param name="studentPath">Path of the students file</param>
        /// <returns>Returns LinkedList of participants</returns>
        public LinkedList<Participant> parse(String studentPath)
        {
            LinkedList<Participant> participants = new LinkedList<Participant>();

            //Create an instance of the XmlTextReader and call Read method to read the file
            XmlTextReader textReader = new XmlTextReader(studentPath);
            textReader.Read();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(textReader);

            //professor
            string preName = "";
            string surName = "";
            XmlNodeList professorNodeList = xmlDoc.GetElementsByTagName("professor");
            foreach (XmlNode node in professorNodeList[0].ChildNodes)
            {
                if(node.Name == "name"){
                    surName = node.InnerText;
                }
                else if(node.Name == "vorname"){
                    preName = node.InnerText;
                }
            }
            Professor professor = new Professor(preName, surName);
            participants.AddLast(professor);

            //participant parsen
            XmlNodeList studentList = xmlDoc.GetElementsByTagName("student");
            for (int i = 0; i < studentList.Count; i++)
            {
                XmlNodeList studentData = studentList[i].ChildNodes;
                String surname = "", prename = "", id = "";
                for (int j = 0; j < studentData.Count; j++)
                {
                    switch (studentData[j].Name)
                    {
                        case "name":
                            surname = studentData[j].InnerText;
                            break;
                        case "vorname":
                            prename = studentData[j].InnerText;
                            break;
                        case "number":
                            id = studentData[j].InnerText;
                            break;
                    }
                }
                Student student = new Student(prename,surname,id);
                participants.AddLast(student);
            }
            return participants;
        }
    }
}
