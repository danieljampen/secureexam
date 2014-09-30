using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class SecureExamConsoleApplication
    {
        private const int RETURNERROR = -1;
        private const int RETURNOK = 0;

        // usage: secureExam -qType WordHTML -q questionfile -sType xml -s studentfile -oType htmljs -o outputfile
        //   alt: secureExam -q questionfile -s studentfile -o outputfile
        static int Main(string[] args)
        {
            if (args.Length >= 6 && args.Length <= 12)
            {
                Facade facade = new Facade();
                Dictionary<string, string> arguments = new Dictionary<string, string>();
                String questionFile = "", studentFile = "", outputFile = "";
                QuestionFormularType questionFormularType = new QuestionFormularType();
                StudentFileType studentFileType = new StudentFileType();
                OutputType outputType = OutputType();
                
                // fill arguments in dictionary
                for( int i = 0; i < (args.Length / 2); i++ )
                {
                    arguments.Add(args[i * 2].ToString().ToLower(), args[i * 2 + 1].ToString().ToLower());
                }

                // set options
                foreach(KeyValuePair<string,string> pair in arguments)
                {
                    switch(pair.Key)
                    {
                        case "-q":
                            questionFile = pair.Value;
                            break;
                        case "-s":
                            studentFile = pair.Value;
                            break;
                        case "-o":
                            outputFile = pair.Value;
                            break;
                        case "-qtype":
                            switch( pair.Value )
                            {
                                case "wordhtml":
                                    questionFormularType = QuestionFormularType.WordHTML;
                                    break;
                                default:
                                    printUsage();
                                    return RETURNERROR;
                            }
                            break;
                        case "-stype":
                            switch( pair.Value )
                            {
                                case "xml":
                                    studentFileType = StudentFileType.XML;
                                    break;
                                default:
                                    printUsage();
                                    return RETURNERROR;
                            }
                            break;
                        case "-otype":
                            switch( pair.Value )
                            {
                                case "htmljs":
                                    outputType = OutputType.HTMLJS;
                                    break;
                                default:
                                    printUsage();
                                    return RETURNERROR;
                            }
                            break;
                        default:
                            printUsage();
                            return RETURNERROR;
                    }
                }


                // SecureExam calls
                if (facade.readData(questionFormularType, questionFile, studentFileType, studentFile))
                {
                    if (facade.export(outputType, outputFile))
                        return RETURNOK;
                }
                return RETURNERROR;
            }
            else
            {
                printUsage();
                return RETURNERROR;
            }               
        }
        private static void printUsage()
        {
            Console.WriteLine("");
        }
    }
}
