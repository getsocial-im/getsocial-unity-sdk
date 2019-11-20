#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal delegate void OnLogWritten(string text, GetSocialLogs.LogLevel level);
    
    internal static class GetSocialLogs
    {
#pragma warning disable 0649
        internal static OnLogWritten Delegate;
#pragma warning restore 0649

        internal enum LogLevel
        {
            Debug = 0, Info, Warn 
        }

        private const LogLevel Level = NativeBuildConfig.Debug ? LogLevel.Debug : LogLevel.Info;

        public static void D(string text)
        {
            WriteLog(LogLevel.Debug, text, Debug.Log);
        }

        public static void I(string text)
        {
            WriteLog(LogLevel.Info, text, Debug.Log);
        }

        public static void W(string text)
        {
            WriteLog(LogLevel.Warn, text, Debug.LogWarning);
        }

        private static void WriteLog(LogLevel level, string text, Action<string> writer)
        {
            if (level < Level) return;
            
            writer(text);
            if (Delegate != null)
            {
                Delegate(text, LogLevel.Warn);
            }
        }
    }
}
#endif
