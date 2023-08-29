//T4P.cs Team 4 "The Drowned" project 

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Team 4 Project Utilities Class
/// </summary>
public static class T4P
{
    public static class Utilities
    {
        public static Vector3 RandomPointInCircle(Vector3 center, float radius)
        {
            Vector3 point = center;

            float angle = Random.Range(0, Mathf.PI * 2);
            point.x = center.x + radius * Mathf.Cos(angle);
            point.y = center.y + radius * Mathf.Sin(angle);
            point.z = center.z;

            return point;
        }
    }

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
        //Level Details
        public const int LevelFlagsMAX = 4;

        //Level ViewLimits
        public static Vector2 XVisualLimit = new Vector2(-15f, 40f);
        public static Vector2 YVisualLimit = new Vector2(-13f, 13f);

        //Lanes
        public static List<Vector3> LanePosition = new List<Vector3>
        {
            new Vector3 (0, 0, 0),      //0.top
            new Vector3 (0, -3, 0),     //1.middle
            new Vector3 (0, -6, 0)      //2.bottom
        };

        //Pickups
        public enum PickupsType
        {
            None = -1,
            Cannonball,
            Doubloon,
            Flag,
            ALL = 3,
        }

        public enum LaneType
        {
            AboveWater,
            UnderWater,
            SeaBed,
        }
    }
}
