using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SecureExam
{
    class XMLFormularParser : IFormularParser
    {
        public LinkedList<Question> parse(String formularPath)
        {
            LinkedList<Question> questions = new LinkedList<Question>();

            //Create an instance of the XmlTextReader and call Read method to read the file
            try
            {
                XmlTextReader textReader = new XmlTextReader(formularPath);
                textReader.Read();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(textReader);

                //question
                XmlNodeList questionlist = xmlDoc.GetElementsByTagName("question");
                for (int i = 0; i < questionlist.Count; i++)
                {
                    XmlNodeList questionData = questionlist[i].ChildNodes;
                    Question question = new Question();

                    for (int j = 0; j < questionData.Count; j++)
                    {
                        switch (questionData[j].Name)
                        {
                            case "legend":
                                question.text = questionData[j].InnerText;
                                break;
                            case "input":
                                XmlNode inputElement = questionData[j];
                                XmlAttributeCollection attributeList = inputElement.Attributes;

                                Answer answer = new Answer();
                                foreach (XmlAttribute attribute in attributeList)
                                {
                                    switch(attribute.Name){
                                        case "type":
                                            if (attribute.Value == "checkbox")
                                            {
                                                question.questionType = QuestionType.CHECK_BOX;
                                            }
                                            else if (attribute.Value == "text")
                                            {
                                                question.questionType = QuestionType.TEXT_BOX;
                                            }
                                            break;
                                        case "value":
                                            answer.text = attribute.Value;
                                            question.answers.Add(answer);
                                            break;
                                        case "isCorrect":
                                            if (question.questionType == QuestionType.CHECK_BOX)// makes no sense with type textbox
                                            {
                                                if (attribute.Value == "true")
                                                {
                                                    answer.isCorrect = true;
                                                }
                                                else if (attribute.Value == "false")
                                                {
                                                    answer.isCorrect = false;
                                                }
                                                question.answers.Add(answer);
                                            }
                                            break;
                                        case "placeholder":
                                            if (question.answers.Count == 0)// If a answer already exists, a placeholder is not used
                                            {
                                                answer.placeHolder = attribute.Value;
                                                question.answers.Add(answer);
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    questions.AddLast(question);
                }
            }








            catch (DirectoryNotFoundException e)
            {
                throw new NotImplementedException(e.ToString());
            }
            return questions;
        }
    }
}
