using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            StreamWriter writer = new StreamWriter("log.txt", true);
            //Datetime:
            writer.WriteLine("DateTime: " + DateTime.Now);
            //message:
            writer.WriteLine("Message: " + ex.Message);
            writer.WriteLine("-----------------------------");
            writer.Close();
            //File.AppendAllLines("Log.txt", new List<string>() { DateTime.Now + " : " + ex.Message + "---------------------------\n" });
            // for each exception write its details associated with datetime 
        }
    }
}
