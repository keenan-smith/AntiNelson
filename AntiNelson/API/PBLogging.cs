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
        /// <summary>
        /// Logs any text you specify as normal.
        /// </summary>
        /// <param name="text">The text you want to log.</param>
        /// <param name="inConsole">Should the log be printed in the console.</param>
        public static void log(string text, bool inConsole = true)
        {
            Debug.Log("[PointBlank] " + text);
            if (inConsole)
                CommandWindow.Log("[PointBlank] " + text);
        }

        /// <summary>
        /// Logs any error you provide with the text you provide. Mostly for exceptions.
        /// </summary>
        /// <param name="text">The text that will be logged.</param>
        /// <param name="ex">The exception that will be logged.</param>
        /// <param name="inConsole">Should the error be logged in the console.</param>
        public static void logError(string text, Exception ex, bool inConsole = true)
        {
            Debug.LogError("[PointBlank] " + ex);
            if (inConsole)
                CommandWindow.LogError("[PointBlank] " + text);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="text">The text that will be logged.</param>
        /// <param name="inConsole">Should it be logged in the console.</param>
        public static void logWarning(string text, bool inConsole = true)
        {
            Debug.LogWarning("[PointBlank] " + text);
            if (inConsole)
                CommandWindow.LogWarning("[PointBlank] " + text);
        }

        /// <summary>
        /// Logs important messages.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="inConsole">Print in console.</param>
        public static void logImportant(string text, bool inConsole = true)
        {
            Debug.Log("[PointBlank] " + text);
            if (inConsole)
                Tool.runMethod(typeof(CommandWindow), "Log", new object[] { text, ConsoleColor.Cyan });
        }
        #endregion
    }
}
