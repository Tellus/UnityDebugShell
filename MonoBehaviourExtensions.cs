using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace UnityDebugShell
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Shorthand for Debug.Log. <b>Also</b> logs the source class and
        /// method.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        public static void Log(this UnityEngine.Component obj, string msg)
        {
            GenericLog(obj, UnityEngine.Debug.Log, LoggingLevel.Info, msg);
        }

        public static void LogError(this UnityEngine.Component obj, string msg)
        {
            GenericLog(obj, UnityEngine.Debug.LogError, LoggingLevel.Error, msg);
        }

        public static void LogWarning(this UnityEngine.Component obj, string msg)
        {
            GenericLog(obj, UnityEngine.Debug.LogWarning, LoggingLevel.Warning, msg);
        }

        protected delegate void LogFunction(string msg, UnityEngine.Object obj);

        private static void GenericLog(UnityEngine.Component obj, LogFunction logFn, LoggingLevel level, string msg)
        {
            StackTrace trace = new StackTrace();
            string fullLogMsg = $"{obj.GetType().Name}.{trace.GetFrame(2).GetMethod().Name}: {msg}";

            // Ignore logging to console if not running in the editor.
            if (Application.isEditor)
            {
                logFn(fullLogMsg, obj);
            }

            // File logging. If the message's level matches or exceeds the one
            // we're listening for, write it out.
            if (level >= LogLevel)
            {
                LogFile.WriteLine($"{ Now }: { fullLogMsg }");
            }
        }

        /// <summary>
        /// Configures the logging extension methods to additionally output to
        /// a file, restricted to a particular logging level.
        /// </summary>
        /// <param name="logPath">Path to where the log file should be stored.
        /// New logs will be <b>appended</b>, the file will not be overwritten.</param>
        /// <param name="logLevel">Highest logging level to allow. Warning will
        /// output Warning and Errors (but not anything else), while Error
        /// will only output Errors.</param>
        public static void SetLogging(string logPath, LoggingLevel logLevel)
        {
            if (!File.Exists(logPath))
            {
                // Create the file.
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                File.CreateText(logPath).Close();
            }

            LogFile = File.AppendText(logPath);

            LogLevel = logLevel;

            LogFile.WriteLine($"{ Now }: Logging begins.");
        }

        private static StreamWriter LogFile;

        private static LoggingLevel LogLevel = LoggingLevel.None;

        public enum LoggingLevel
        {
            None, Error, Warning, Info
        }

        private static string Now => DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
    }
}
