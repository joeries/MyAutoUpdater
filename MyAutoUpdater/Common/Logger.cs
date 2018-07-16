using System;
using System.IO;

namespace MyAutoUpdater.Common
{
    public class Logger
    {
        public static void Log(string level, string message)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updater.log");
            File.AppendAllText(logFilePath, string.Format("{0} {1} {2}{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), level, message, Environment.NewLine));
        }
    }
}