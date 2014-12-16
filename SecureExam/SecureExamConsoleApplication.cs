using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Is the main application, reads parameter, starts console
    /// </summary>
    public class SecureExamConsoleApplication
    {
        private const int RETURNERROR = -1;
        private const int RETURNOK = 0;

        /// <summary>
        /// Starts the main console application and uses array of arguments
        /// </summary>
        /// <param name="args">array of arguments</param>
        /// <returns>Returns returncode</returns>
        public static int Main(string[] args)
        {
            try
            {
                if (args == null || args.Length < 8 || args.Length > 14)
                    throw new ArgumentException();
                Facade facade = new Facade();
                Dictionary<string, string> arguments = new Dictionary<string, string>();
                String questionFile = "", studentFile = "", outputFile = "", settingsFile = "";
                QuestionFormularType questionFormularType = QuestionFormularType.XML;
                StudentFileType studentFileType = StudentFileType.XML;
                OutputType outputType = OutputType.HTMLJS;
                StudentSecretsFileFormat studentsSecretFileFormat = StudentSecretsFileFormat.XML;

                // fill arguments in dictionary
                for (int i = 0; i < (args.Length / 2); i++)
                {
                    arguments.Add(args[i * 2].ToString().ToLower().Trim(), args[i * 2 + 1].ToString().ToLower().Trim());
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
                        case "-p":
                            settingsFile = pair.Value;
                            break;
                        case "-qtype":
                            switch (pair.Value)
                            {
                                case "odt":
                                    questionFormularType = QuestionFormularType.ODT;
                                    break;
                                case "xml":
                                    questionFormularType = QuestionFormularType.XML;
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
                        case "-ostudentsecretsfileformat":
                            switch (pair.Value)
                            {
                                case "xml":
                                    studentsSecretFileFormat = StudentSecretsFileFormat.XML;
                                    break;
                                default:
                                    throw new ArgumentException();
                            }
                            break;
                        
                        default:
                            throw new ArgumentException();
                    }
                }

                // SecureExam calls
                facade.readData(questionFormularType, questionFile, studentFileType, studentFile, settingsFile);
                facade.export(outputType, outputFile, studentsSecretFileFormat);
                return RETURNOK;
            }
            catch (ArgumentException)
            {
                printUsage();
            }
            catch (DataReadException e)
            {
                printError("Data Read failed: " + e.Message);
            }
            catch (ExportException e)
            {
                printError("Export failed: " + e.Message);
            }
            catch (NotImplementedException e)
            {
                printError("Unimplemented method called! " + e.Message);
            }
            return RETURNERROR;
        }

        /// <summary>
        /// Prints the usage of the application
        /// </summary>
        public static void printUsage()
        {
            Console.WriteLine("Error: invalid arguments");
            Console.WriteLine("");
            Console.WriteLine("usage: secureExam -q questionFile -s studentsFile -o outputFile -p settingsFile");
            Console.WriteLine("       secureExam -q questionFile [-qType QuestionFileType] -s studentsFile [-sType StudentsFileType] -o outputFile [-oType OutputFileType] [-oStudentSecretsFileFormat studentSecretsFileFormat] -p settingsFile");
            Console.WriteLine("");
            Console.WriteLine("QuestionFileTypes: XML, ODT");
            Console.WriteLine("StudentFileTypes: XML");
            Console.WriteLine("OutputFileTypes: HTMLJS");
            Console.WriteLine("StudentSecretsFileFormat: XML");
        }

        /// <summary>
        /// Print errors to console
        /// </summary>
        /// <param name="message">describes error</param>
        public static void printError( string message )
        {
            if (message == null)
                throw new ArgumentNullException("message null");
            if (message.Length == 0)
                throw new ArgumentException("message length 0");

            Console.WriteLine("[ERROR] " + DateTime.Now.ToString("u") + ": " + message);
        }
    }
}
