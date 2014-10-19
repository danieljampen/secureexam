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
        public bool export(String filename)
        {
            StreamReader htmlSkeleton = new StreamReader(BasicSettings.getInstance().exportSkeletons[OutputType.HTMLJS]);
            StreamWriter outFile = new StreamWriter(filename);

            try
            {
                // read skeleton
                String html = htmlSkeleton.ReadToEnd();
                
                // Replace the placeholders in HTML code with real data
                html = html.Replace("$SHA256ITERATIONS$", BasicSettings.getInstance().Encryption.SHA256.ITERATIONS.ToString());
                html = html.Replace("$RANDOMCHARSINUSERSECRET$", BasicSettings.getInstance().NumberOfRandomCharsInStudentSecret.ToString());
                html = html.Replace("$ENCRYPTEDDATA$", DataProvider.getInstance().exportQuestions(DataProviderExportType.HTML));
                html = html.Replace("$USERKEYDB$", DataProvider.getInstance().exportUserKeyDB(DataProviderExportType.HTML));

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
