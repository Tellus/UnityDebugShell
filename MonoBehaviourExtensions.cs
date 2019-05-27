using System;
using System.Diagnostics;
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
            GenericLog(obj, UnityEngine.Debug.Log, msg);
        }

        public static void LogError(this UnityEngine.Component obj, string msg)
        {
            GenericLog(obj, UnityEngine.Debug.LogError, msg);
        }

        public static void LogWarning(this UnityEngine.Component obj, string msg)
        {
            GenericLog(obj, UnityEngine.Debug.LogWarning, msg);
        }

        protected delegate void LogFunction(string msg, UnityEngine.Object obj);

        private static void GenericLog(UnityEngine.Component obj, LogFunction logFn, string msg)
        {
            // Ignore logging if not a debug build.
            if (UnityEngine.Debug.isDebugBuild)
            {
                StackTrace trace = new StackTrace();

                logFn($"{obj.GetType().Name}.{trace.GetFrame(2).GetMethod().Name}: {msg}", obj);
            }
        }
    }
}
