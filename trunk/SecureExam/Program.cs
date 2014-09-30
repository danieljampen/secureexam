using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class Program
    {
        static void Main(string[] args)
        {
            // word und xml file angeben
            foreach(string token in args){
                if (token.EndsWith(".xml"))
                {

                }
                if (token.EndsWith(".doc") || token.EndsWith(".docx"))
                {
                    //
                    //FileInfo f = new FileInfo(token);
                    StreamReader sr = new StreamReader(token);
                    String line = sr.ReadLine();
                    while(!line.Contains("\n")){


                    }
                    
                }


                Question q = new Question();
                q.text = "Wie hoch ist der Mount Everest?";
                List<Answer> answers = new List<Answer>();
                Answer a1 = new Answer("1m");
                Answer a2 = new Answer("1345m");
                Answer a3 = new Answer("6890m");
                Answer a4 = new Answer("8807m");
                answers.Add(a1);
                answers.Add(a2);
                answers.Add(a3);
                answers.Add(a4);
                q.answers = answers;

            }






        }
    }
}
