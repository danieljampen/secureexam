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

        static int Main(string[] args)
        {
            try
            {
                if (args.Length < 6 || args.Length > 12)
                    throw new ArgumentException();

                Facade facade = new Facade();
                Dictionary<string, string> arguments = new Dictionary<string, string>();
                String questionFile = "", studentFile = "", outputFile = "";
                QuestionFormularType questionFormularType = new QuestionFormularType();
                StudentFileType studentFileType = new StudentFileType();
                OutputType outputType = new OutputType();

                // fill arguments in dictionary
                for (int i = 0; i < (args.Length / 2); i++)
                {
                    arguments.Add(args[i * 2].ToString().ToLower(), args[i * 2 + 1].ToString().ToLower());
                }

                // set options
                foreach (KeyValuePair<string, string> pair in arguments)
                {
                    switch (pair.Key)
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
                            switch (pair.Value)
                            {
                                case "wordhtml":
                                    questionFormularType = QuestionFormularType.WordHTML;
                                    break;
                                default:
                                    throw new ArgumentException();
                            }
                            break;
                        case "-stype":
                            switch (pair.Value)
                            {
                                case "xml":
                                    studentFileType = StudentFileType.XML;
                                    break;
                                default:
                                    throw new ArgumentException();
                            }
                            break;
                        case "-otype":
                            switch (pair.Value)
                            {
                                case "htmljs":
                                    outputType = OutputType.HTMLJS;
                                    break;
                                default:
                                    throw new ArgumentException();
                            }
                            break;
                        default:
                            throw new ArgumentException();
                    }

                    // SecureExam calls
                    if (facade.readData(questionFormularType, questionFile, studentFileType, studentFile))
                    {
                        if (facade.export(outputType, outputFile))
                        {
                            return RETURNOK;
                        }
                        else
                            throw new ExportException(outputFile);
                    }
                    else
                        throw new DataReadException(questionFile + studentFile);
                }
            }
            catch (ArgumentException)
            {
                printUsage();
            }
            catch (DataReadException e)
            {
                Console.WriteLine("Data Read failed: " + e.Message);
            }
            catch (ExportException e)
            {
                Console.WriteLine("Export failed: " + e.Message);
            }
            Console.ReadLine();
            return RETURNERROR;
        }

        private static void printUsage()
        {
            Console.WriteLine("Error: invalid arguments");
            Console.WriteLine("");
            Console.WriteLine("usage: secureExam -q questionFile -s studentsFile -o Outputfile");
            Console.WriteLine("       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o Outputfile [-oType OutputFileType]");
            Console.WriteLine("");
            Console.WriteLine("QuestionFileTypes: WordHTML");
            Console.WriteLine("StudentFileTypes: XML");
            Console.WriteLine("OutputFileTypes: HTMLJS");
        }
    }
}
