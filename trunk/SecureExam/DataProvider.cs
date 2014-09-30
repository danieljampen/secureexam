using System;
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
        private IExport exporter = new HTMLJSExport();
        private IFormularParser formularParser = new WordFormularParser();
        private IStudentParser studentParser = new XMLStudentParser();

        // methods
        public bool readData(System.IO.Path formularPath, System.IO.Path studentPath)
        {
            this.questions = this.formularParser.parse(formularPath);
            this.students = this.studentParser.parse(studentPath);

            return (this.questions.Count != 0 && this.students.Count != 0);
        }

        public bool export(System.IO.Path filename)
        {
            return this.exporter.export(filename);
        }
    }
}
