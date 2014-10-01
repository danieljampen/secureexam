using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureExam
{
    class HTMLJSExport:IExport
    {
        public bool export(String filename, Func<DataProviderExportType,String>getQuestions, Func<DataProviderExportType,String> getUserKeyDB )
        {
            StreamReader htmlSkeleton = new StreamReader(BasicSettings.getInstance().exportSkeletons[OutputType.HTMLJS]);
            StreamWriter outFile = new StreamWriter(filename);

            try
            {
                // read skeleton
                String html = htmlSkeleton.ReadToEnd();
                
                // Replace the placeholders in HTML code with real data
                html.Replace("$ENCRYPTEDDATA$", getQuestions(DataProviderExportType.XML));
                html.Replace("$USERKEYDB$", getQuestions(DataProviderExportType.XML));

                // write data to file
                outFile.Write(html);
            }
            catch( Exception e )
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                htmlSkeleton.Close();
                outFile.Close();
            }
            return true;
        }
    }
}
