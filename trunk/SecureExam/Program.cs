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
                    //StreamReader sr = new StreamReader(token);
                    //String line = sr.ReadLine();
                    //while(line != ){


                    }
                    
                }
            }

        }
    }
}
