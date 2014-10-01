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
        public LinkedList<Student> parse(String studentPath)
        {
            LinkedList<Student> students = new LinkedList<Student>();
            XmlReader reader = XmlReader.Create(studentPath);
            
            while( reader.Read() )
            {
                switch(reader.NodeType)
                {
                    case XmlNodeType.Element:
                        
                }
            }
            throw new NotImplementedException();
        }
    }
}
