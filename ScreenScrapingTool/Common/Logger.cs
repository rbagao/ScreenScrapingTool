using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenScrapingTool.Common
{
    public static class Logger
    {
        public static void Error(string message)
        {
            string filePath = @"TraceLog.txt";

            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(string.Format($"{DateTime.Now} Error: {message}"));
                streamWriter.Close();
            }

        }
        
    }
}
