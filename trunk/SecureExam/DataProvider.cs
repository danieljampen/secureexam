using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class DataProvider: IDataProvider
    {
        // members
        private LinkedList<Question> questions = new LinkedList<Question>();
        private LinkedList<Student> students = new LinkedList<Student>();
        private IExport exporter;
        private IFormularParser formularParser;
        private IStudentParser studentParser;

        // methods
        public bool readData(QuestionFormularType formularType, String formularPath, StudentFileType studentType, String studentPath)
        {
            switch (formularType)
            {
                case QuestionFormularType.WordHTML:
                    this.formularParser = new WordFormularParser();
                    break;
                default:
                    throw new InvalidFormularTypeException(formularType.ToString());
            }
            this.questions = this.formularParser.parse(formularPath);

            switch(studentType)
            {
                case StudentFileType.XML:
                    this.studentParser = new XMLStudentParser();
                    break;
                default:
                    throw new InvalidStudentFileTypeException(studentType.ToString());
            }
            this.students = this.studentParser.parse(studentPath);

            return (this.questions.Count != 0 && this.students.Count != 0);
        }

        public bool export(OutputType type, String path)
        {
            switch (type) 
            {
                case OutputType.HTMLJS:
                    this.exporter = new HTMLJSExport();
                    return this.exporter.export(path);
                default:    
                    throw new InvalidExportTypeException(type.ToString());
            }
        }
    }
}
