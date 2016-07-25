using UnityEngine;
using System.Collections;
using System.IO;

namespace QuickUnity
{
    public static class LogManager
    {
        static LogManager()
        {
            Application.logMessageReceived += new Application.LogCallback(OnLog);
        }

        static void Startup(string path, eLogLevel level)
        {
            SetLogFilePath(path);
            logLevel = level;
        }

        public static bool SetLogFilePath(string path)
        {
            FileInfo fi = new FileInfo(path);
            StreamWriter sw = fi.CreateText();
            if (sw == null) return false;
            sw.Close();
            return true;
        }

        static private void OnLog(string condition, string stackTrace, LogType type)
        {
            // Check and filter
            if (logFilePath == null) return;
            if (logLevel > eLogLevel.Debug && type == LogType.Log) return;

            // Flush log
            FileInfo fi = new FileInfo(logFilePath);
            StreamWriter sw = fi.AppendText();
            if (sw == null) return;
            sw.WriteLine(string.Format("[{0}] {1}:{2}", type.ToString(), stackTrace, condition));
            sw.Close();
        }

        
        static private string logFilePath = null;

#if DEBUG
        private static eLogLevel logLevel = eLogLevel.Debug;
#else
        private static eLogLevel logLevel = eLogLevel.Error;
#endif

        // defines
        public enum eLogLevel { Debug = 0, Error, }
    }
}


