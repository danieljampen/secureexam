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
        public bool readData(FormularType formularType, Path formularPath, StudentFileType studentType, Path studentPath)
        {
            switch (formularType)
            {
                case FormularType.WordHTML:
                    this.formularParser = new WordFormularParser();
                    this.questions = this.formularParser.parse(formularPath);
                    break;
                default:
                    throw new InvalidFormularTypeException(formularType.ToString());
            }

            switch(studentType)
            {
                case StudentFileType.XML:
                    this.studentParser = new XMLStudentParser();
                    this.students = this.studentParser.parse(studentPath);
                    break;
                default:
                    throw new InvalidStudentFileTypeException(studentType.ToString());
            }

            return (this.questions.Count != 0 && this.students.Count != 0);
        }

        public bool export(ExportType type, Path path)
        {
            switch (type) 
            {
                case ExportType.HTMLJS:
                    this.exporter = new HTMLJSExport();
                    return this.exporter.export(path);
                default:    
                    throw new InvalidExportTypeException(type.ToString());
            }
        }
    }
}
