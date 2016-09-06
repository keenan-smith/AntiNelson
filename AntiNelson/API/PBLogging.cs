using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.API
{
    public class PBLogging
    {
        #region Functions
        public static void log(string text, bool inConsole = true)
        {
            Debug.Log("[PointBlank] " + text);
            //Console.WriteLine(text);
            if (inConsole)
                CommandWindow.Log("[PointBlank] " + text);
        }

        public static void logError(string text, Exception ex, bool inConsole = true)
        {
            Debug.LogError("[PointBlank] " + ex);
            //Console.WriteLine(ex.Message);
            if (inConsole)
                CommandWindow.LogError("[PointBlank] " + text);
        }

        public static void logWarning(string text, bool inConsole = true)
        {
            Debug.LogWarning("[PointBlank] " + text);
            //Console.WriteLine(text);
            if (inConsole)
                CommandWindow.LogWarning("[PointBlank] " + text);
        }
        #endregion
    }
}
