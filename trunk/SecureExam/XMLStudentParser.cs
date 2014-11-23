using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    public class XMLStudentParser : IStudentParser
    {
        public LinkedList<Participant> parse(String studentPath)
        {
            LinkedList<Participant> participants = new LinkedList<Participant>();

            //Create an instance of the XmlTextReader and call Read method to read the file
            XmlTextReader textReader = new XmlTextReader(studentPath);
            textReader.Read();


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(textReader);

            //professor
            XmlNodeList professor = xmlDoc.GetElementsByTagName("professor");
            //BasicSettings basicSettings = BasicSettings.getInstance();
            participants.AddLast(new Professor(professor[0].InnerText.ToLower()));


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
