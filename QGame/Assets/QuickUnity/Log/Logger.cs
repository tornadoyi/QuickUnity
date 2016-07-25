using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class Logger : BaseManager<Logger>
    {
        struct LogCache
        {
            public LogCache(LogType logLevel, string log)
            {
                this.logLevel = logLevel;
                this.log = log;
            }
            public LogType logLevel;
            public string log;
        }

        private List<LogCache> logCaches;

        public Logger()
        {
            logCaches = new List<LogCache>();
        }

        public static void Log(string format) { instance.Print(LogType.Log, Format(LogType.Log, format)); }
        public static void LogFormat(string format, params string[] parms) { instance.Print(LogType.Log, Format(LogType.Log, format, parms)); }
        public static void LogError(string format) { instance.Print(LogType.Error, Format(LogType.Error, format)); }
        public static void LogErrorFormat(string format, params string[] parms) { instance.Print(LogType.Error, Format(LogType.Error, format, parms)); }
        public static void LogException(System.Exception e) { instance.Print(LogType.Error, Format(LogType.Exception, e.ToString())); }
        public static void LogWarning(string format) { instance.Print(LogType.Warning, Format(LogType.Warning, format)); }
        public static void LogWarningFormat(string format, params string[] parms) { instance.Print(LogType.Warning, Format(LogType.Warning, format, parms)); }
        public static void LogAssertion(string format) { instance.Print(LogType.Assert, Format(LogType.Assert, format)); }
        public static void LogAssertionFormat(string format, params string[] parms) { instance.Print(LogType.Assert, Format(LogType.Assert, format, parms)); }

        void Update()
        {
            Print();
        }

        public void Print()
        {
            lock (logCaches)
            {
                for (int i = 0; i < logCaches.Count; ++i)
                {
                    var cache = logCaches[i];
                    switch(cache.logLevel)
                    {
                        case LogType.Exception:
                        case LogType.Error: UnityEngine.Debug.LogError(cache.log); break;
                        case LogType.Warning: UnityEngine.Debug.LogWarning(cache.log); break;
                        case LogType.Assert: UnityEngine.Debug.LogAssertion(cache.log); break;
                        default: UnityEngine.Debug.Log(cache.log); break;
                    }
                }
                logCaches.Clear();
            }
               
        }

        protected static string Format(LogType level, string format, params string[] parms)
        {
            var content = string.Format(format, parms);
            content = string.Format("[{0}] ", level) + content;
            return content;
        }

        protected void Print(LogType level, string log)
        {
            if (string.IsNullOrEmpty(log)) return;
            lock(logCaches)
            {
                logCaches.Add(new LogCache(level, log));
            }
        }
    }
}


