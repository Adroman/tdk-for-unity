using JetBrains.Annotations;
using Scrips.CustomTypes;
using UnityEngine;

namespace Scrips.Utils
{
    [PublicAPI]
    public static class DebugUtils
    {
        public static void LogDebug(LogLevel level, string message)
        {
            if (level <= LogLevel.Debug) Debug.Log(message);
        }

        public static void LogWarn(LogLevel level, string message)
        {
            if (level <= LogLevel.Warn) Debug.LogWarning(message);
        }

        public static void LogError(LogLevel level, string message)
        {
            if (level <= LogLevel.Error) Debug.LogError(message);
        }
    }
}