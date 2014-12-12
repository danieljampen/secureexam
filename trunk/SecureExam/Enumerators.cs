using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    /// <summary>
    /// Type of the exam output file
    /// </summary>
    public enum OutputType
    {
        /// <summary>
        /// Is type of HTML and JavaScript
        /// </summary>
        HTMLJS
    }

    /// <summary>
    /// Type of the exam questions import formular
    /// </summary>
    public enum QuestionFormularType
    {
        /// <summary>
        /// Questions defined in Open Office Document
        /// </summary>
        ODT,
        /// <summary>
        /// Questions defined in XML
        /// </summary>
        XML
    }

    /// <summary>
    /// Type of the students import formular (XML or ODT)
    /// </summary>
    public enum StudentFileType
    {
        /// <summary>
        /// Students imports defined in XML
        /// </summary>
        XML
    }

    /// <summary>
    /// Type of the Question
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Answer type check box
        /// </summary>
        CHECK_BOX,
        /// <summary>
        /// Answer type text box
        /// </summary>
        TEXT_BOX
    }

    /// <summary>
    /// Type of the students output file
    /// </summary>
    public enum StudentSecretsFileFormat
    {
        /// <summary>
        /// Student secrets defined in XML
        /// </summary>
        XML
    }

    /// <summary>
    /// Sets the ViewMode SCROLL for scrollable exams or PAGE for exams with one question per page.
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// Scrollable page
        /// </summary>
        SCROLL,
        /// <summary>
        /// Non-scrollable page
        /// </summary>
        PAGE
    }
}
