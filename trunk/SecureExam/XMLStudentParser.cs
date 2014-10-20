using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class XMLStudentParser:IStudentParser
    {
        public LinkedList<Participant> parse(String studentPath)
        {
            LinkedList<Participant> participants = new LinkedList<Participant>();

            //Create an instance of the XmlTextReader and call Read method to read the file
            try
            {
                XmlTextReader textReader = new XmlTextReader(studentPath);
                textReader.Read();


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(textReader);

                //professor
                XmlNodeList professor = xmlDoc.GetElementsByTagName("professor");
                BasicSettings basicSettings = BasicSettings.getInstance();
                participants.AddLast(new Professor(professor[0].InnerText.ToLower()));


                //participant parsen
                XmlNodeList studentList = xmlDoc.GetElementsByTagName("student");
                for (int i = 0; i < studentList.Count; i++)
                {
                    XmlNodeList studentData = studentList[i].ChildNodes;
                    Student student = new Student();

                    for (int j = 0; j < studentData.Count; j++)
                    {
                        switch (studentData[j].Name)
                        {
                            case "name":
                                student.studentSurName = studentData[j].InnerText;
                                break;
                            case "vorname":
                                student.studentPreName = studentData[j].InnerText;
                                break;
                            case "number":
                                student.studentID = studentData[j].InnerText;
                                break;
                        }
                    }
                    participants.AddLast(student);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                throw new NotImplementedException(e.ToString());
            }
            return participants;
        }
    }
}
