using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroService
{
    public class Logger
    {
        public void ExceptionFileLogging(string exceptionReason,string methodName)
        {
            string logFilePath = "C:\\Users\\cogdotnet1277\\Desktop\\TamoghnaFSE\\ProductMicroserviceLogger.txt";
            using (StreamWriter writer = new StreamWriter(logFilePath))
            {
                writer.WriteLine("Exception Occurs :" + DateTime.Now);
                writer.WriteLine("Custom Exception Reason :" + exceptionReason);
                writer.WriteLine("Full Method Name :" + methodName);
                writer.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            }
        }
    }
}
