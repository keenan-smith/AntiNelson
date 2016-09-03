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
            Debug.Log(text);
            Console.WriteLine(text);
            if (inConsole)
                CommandWindow.Log(text);
        }

        public static void logError(string text, Exception ex, bool inConsole = true)
        {
            Debug.LogException(ex);
            Console.WriteLine(ex.Message);
            if (inConsole)
                CommandWindow.LogError(text);
        }

        public static void logWarning(string text, bool inConsole = true)
        {
            Debug.LogWarning(text);
            Console.WriteLine(text);
            if (inConsole)
                CommandWindow.LogWarning(text);
        }
        #endregion
    }
}
