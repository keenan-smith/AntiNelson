using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API.Server;
using PointBlank.API.Attributes;

namespace PointBlank.PB_Overridables
{
    public class Override_CommandWindow
    {
        [ReplaceCode(typeof(CommandWindow), "onInputText", BindingFlags.NonPublic | BindingFlags.Static)]
        public static void onInputText(string command)
        {
            PBServer.consoleInput(command);
        }

        [ReplaceCode(typeof(CommandWindow), "onOutputText", BindingFlags.NonPublic | BindingFlags.Static)]
        public static void onOutputText(string text, string stack, LogType type)
        {
            PBServer.consoleOutput(text, stack, type);
        }
    }
}
