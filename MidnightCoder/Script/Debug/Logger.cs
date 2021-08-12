using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MidnightCoder.Game
{
    /// <summary>
    /// 向persistentDataPath输出日志。
    /// 用于安卓通信调试或非编辑器环境。日常勿使用。
    /// </summary>
    public static class Logger
    {
        //
        // Static Fields
        //
        private static string filename = string.Empty;

        public const string INFO = "INFO";

        public const string LOAD = "LOAD";

        public const string WARN = "WARN";

        public const string ERR = "ERR";

        public const string DERR = "DERR";

        public const int MAX_LOG = 4;

        public static bool enableStackTrace = true;

        private static int currIdx = 0;

        private static Logger.Data[] data = new Logger.Data[4];

        private static bool openLog = true;

        //
        // Static Methods
        //

        public static void CloseLog()
        {
            openLog = false;
        }
        private static void CompressOldLogs()
        {
        }

        public static void Flush()
        {
            string path = Path.Combine(Application.persistentDataPath, Logger.filename);
            try
            {
                using (StreamWriter streamWriter = File.AppendText(path))
                {
                    for (int i = 0; i < Logger.currIdx; i++)
                    {
                        if (Logger.data[i].IsEmpty())
                        {
                            break;
                        }
                        streamWriter.WriteLine(Logger.data[i].ToString());
                        streamWriter.WriteLine(string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            Logger.currIdx = 0;
        }
        public static void Log(object message, params object[] args)
        {
            if(!openLog) return;
            Logger.Log(Logger.INFO, message.ToString(), args);
        }
        public static void LogWarning(object message, params object[] args)
        {
            if (!openLog) return;
            Logger.Log(Logger.WARN, message.ToString(), args);
        }
        public static void LogError(object message, params object[] args)
        {
            if (!openLog) return;

            Logger.Log(Logger.ERR, message.ToString(), args);
        }

        private static void Log(string type, string messageFormat, params object[] args)
        {
            if (args.Length > 0)
            {
                Logger.LogInternal(type, string.Format(messageFormat, args), Logger.enableStackTrace);
            }
            else
            {
                Logger.LogInternal(type, messageFormat, Logger.enableStackTrace);
            }
        }
        private static void LogInternal(string type, string message, bool addStackTrace)
        {
            if (string.IsNullOrEmpty(Logger.filename))
            {
                string format = "yyyyMMdd-HHmmss";
                Logger.filename = DateTime.Now.ToString(format) + ".log";
                Logger.CompressOldLogs();
            }
            int num = Logger.currIdx % 4;
            Logger.data[num].time = DateTime.Now.ToString();
            Logger.data[num].type = type;
            Logger.data[num].message = message;
            if (addStackTrace)
            {
                Logger.data[num].stackTrace = Logger.StackTrace();
            }
            else
            {
                Logger.data[num].stackTrace = string.Empty;
            }
            if (Logger.onLog != null)
            {
                Logger.onLog(Logger.data[num]);
            }
            Logger.currIdx++;
            if (Logger.currIdx == 4)
            {
                Logger.Flush();
            }
        }

        private static string StackTrace()
        {
            if (!Logger.enableStackTrace)
            {
                return string.Empty;
            }
            string text = StackTraceUtility.ExtractStackTrace();
            int startIndex = text.LastIndexOf("MidnightCoder.Game.Debug");
            startIndex = text.IndexOf(' ', startIndex);
            return "	" + text.Substring(startIndex).Trim().Replace(" ", "		");
        }

        //
        // Static Events
        //
       // public static event Action<Logger.Data> onDataError;

        public static event Action<Logger.Data> onLog;

        //
        // Nested Types
        //
        public struct Data
        {
            public string time;

            public string type;

            public string message;

            public string stackTrace;

            public void Reset()
            {
                this.time = string.Empty;
                this.type = string.Empty;
                this.message = string.Empty;
                this.stackTrace = string.Empty;
            }

            public bool IsEmpty()
            {
                return string.IsNullOrEmpty(this.time);
            }

            public override string ToString()
            {
                if (this.IsEmpty())
                {
                    return string.Empty;
                }
                return string.Format("{0} [{1}] {2} {3}", new object[] {
                    this.time,
                    this.type,
                    this.message,
                    this.stackTrace
                });
            }

            public void Copy(Logger.Data rhs)
            {
                this.time = rhs.time;
                this.type = rhs.type;
                this.message = rhs.message;
                this.stackTrace = rhs.stackTrace;
            }
        }
    }
}
