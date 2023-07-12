//T4P.cs Team 4 "The Drowned" project 

using UnityEngine;

/// <summary>
/// Team 4 Project Utilities Class
/// </summary>
public static class T4P
{
    public static class T4Debug
    {
        public enum LogType
        {
            Normal,
            Warning,
            Error,
        }

        /// <summary>
        /// Custom debug.log message for build.
        /// </summary>
        /// <param name="logMessage">string message</param>
        /// <param name="type">type of message: Normal, Warning, Error</param>
        public static void Log(string logMessage, LogType type = LogType.Normal)
        {
            #if UNITY_EDITOR
            switch (type)
            {
                case LogType.Normal: { Debug.Log($"<color=orange>T4Debug</color> <color=white>[LOG]:</color> {logMessage}"); break; }
                case LogType.Warning: { Debug.LogWarning($"<color=orange>T4Debug</color> <color=yellow>[WARNING]:</color> {logMessage}"); break; }
                case LogType.Error: { Debug.LogError($"<color=orange>T4Debug</color> <color=red>[ERROR]:</color> {logMessage}"); break; }
                default: { break; }
            }
            #endif
        }
    }

    public static class T4Project
    {
        public static Vector2 XVisualLimit = new Vector2(-15f, 15f);
        public static Vector2 YVisualLimit = new Vector2(-13f, 13f);
        public enum PickupsType
        {
            None = -1,
            Cannonball,
            Doubloon,
            ALL = 2,
        }
    }
}
