using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SecureExam
{
    /// <summary>
    /// Uses interface IForlmularParser<para />
    /// Is used to parse the XML question import file.
    /// </summary>
    public class XMLFormularParser : IFormularParser
    {
        private XmlDocument xmlDoc;

        /// <summary>
        /// Parses a document by a given stream.
        /// </summary>
        /// <param name="streamReader">stream of the document to parse</param>
        /// <returns>Returns LinkedList of questions</returns>
        public LinkedList<Question> parse(StreamReader streamReader)
        {
            LinkedList<Question> questions = new LinkedList<Question>();

            //Create an instance of the XmlTextReader and call Read method to read the file
            this.xmlDoc = new XmlDocument();
            this.xmlDoc.Load(streamReader);

            //question
            XmlNodeList questionlist = this.xmlDoc.GetElementsByTagName("question");
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
                                switch (attribute.Name)
                                {
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
                                        }
                                        break;
                                    case "placeholder":
                                        if (question.answers.Count == 0)// If a answer already exists, a placeholder is not used
                                        {
                                            answer.placeHolder = attribute.Value;
                                        }
                                        break;
                                }
                            }
                            question.answers.Add(answer);
                            break;
                    }
                }
                questions.AddLast(question);
            }
            return questions;
        }

        /// <summary>
        /// Gets text by a given tag name
        /// </summary>
        /// <param name="tag">tag name</param>
        /// <returns>Returns the inner text of the XML tag</returns>
        private string getTextByTagName(string tag)
        {
            string result = "";
            if (this.xmlDoc.GetElementsByTagName(tag).Count > 0)
            {
                result = this.xmlDoc.GetElementsByTagName(tag)[0].InnerText;
            }
            return result;
        }
    }
}