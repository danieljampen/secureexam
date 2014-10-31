using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Collections;

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

                //exam title
                if (xmlDoc.GetElementsByTagName("examTitle").Count > 0)
                {
                    string examTitle = xmlDoc.GetElementsByTagName("examTitle")[0].InnerText;
                    BasicSettings.getInstance().ExamTitle = examTitle;
                }

                if (xmlDoc.GetElementsByTagName("subject").Count > 0)
                {
                    string subject = xmlDoc.GetElementsByTagName("subject")[0].InnerText;
                    BasicSettings.getInstance().Subject = subject;
                }

                if (xmlDoc.GetElementsByTagName("hint").Count > 0)
                {
                    string hints = xmlDoc.GetElementsByTagName("hint")[0].InnerText;
                    DataProvider.getInstance().examNotes = hints;
                    //BasicSettings.getInstance().Subject = subject;
                }
                //get all Answers
                Hashtable answerToQuestionHashTable = new Hashtable();
                Hashtable answerQuestionTypeHashTable = new Hashtable();
                
                XmlNodeList answerlist = xmlDoc.GetElementsByTagName("answer");
                for (int i = 0; i < answerlist.Count; i++)
                {
                    Answer answer = new Answer();
                    answer.isCorrect = false;

                    int questionNr = int.Parse(answerlist[i].Attributes["questionNr"].InnerText);
                    XmlNodeList answerChildNodes = answerlist[i].ChildNodes;
                    foreach (XmlNode answerChildData in answerChildNodes)
                    {
                        if (answerChildData.Name == "input")
                        {
                            XmlAttributeCollection attributes = answerChildData.Attributes;
                            foreach(XmlAttribute attribute in attributes){
                                if (attribute.Name == "type")
                                {
                                    QuestionType questionType = QuestionType.TEXT_BOX;
                                    switch (attribute.InnerText)
                                    {
                                        case "checkbox":
                                            questionType = QuestionType.CHECK_BOX;
                                            break;
                                        case "text":
                                            questionType = QuestionType.TEXT_BOX;
                                            break;
                                    }
                                    if (!answerQuestionTypeHashTable.ContainsKey(questionNr))
                                    {
                                        answerQuestionTypeHashTable[questionNr] = questionType;
                                    }
                                }
                                else if (attribute.Name == "placeholder")
                                {
                                    answer.placeHolder = attribute.InnerText;
                                }
                                else if (attribute.Name == "isCorrect")
                                {
                                    if (attribute.InnerText == "true")
                                    {
                                        answer.isCorrect = true;
                                    }
                                }
                                else if (attribute.Name == "value")
                                {
                                    answer.text = attribute.InnerText;
                                }
                            }
                        }
                    }
                    if (answerToQuestionHashTable.ContainsKey(questionNr))
                    {
                        //get List and add Answer
                        ((List<Answer>)answerToQuestionHashTable[questionNr]).Add(answer);
                    }
                    else
                    {
                        //create new List
                        List<Answer> list = new List<Answer>();
                        list.Add(answer);
                        answerToQuestionHashTable[questionNr] = list;
                    }
                }

                //question
                XmlNodeList questionlist = xmlDoc.GetElementsByTagName("question");
                for (int i = 0; i < questionlist.Count; i++)
                {
                    Question question = new Question();

                    //get all answers to this question
                    int questionNr = int.Parse(questionlist[i].Attributes["nr"].InnerText);
                    question.answers = (List<Answer>)answerToQuestionHashTable[questionNr];
                    question.questionType = (QuestionType)answerQuestionTypeHashTable[questionNr];

                    XmlNodeList questionDataList = questionlist[i].ChildNodes;
                    foreach(XmlNode questionData in questionDataList){
                        if(questionData.Name == "legend"){
                            question.text = questionData.InnerText;
                        }
                    }
                    questions.AddLast(question);
                }

            }
            catch (DirectoryNotFoundException e)
            {
                throw new NotImplementedException(e.ToString());
            }
            catch (FileNotFoundException e)
            {
                throw new NotImplementedException(e.ToString());
            }
            return questions;
        }
    }
}
