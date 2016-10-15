using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Attributes;

namespace PointBlank.PB_Overridables
{
    internal class Override_CommandWindow
    {
        [ReplaceCode(typeof(CommandWindow), "onInputText", BindingFlags.NonPublic | BindingFlags.Static)]
        public static void onInputText(string command)
        {
            PBServer.consoleInput(command);
        }

        [ReplaceCode(typeof(CommandWindow), "Log", BindingFlags.NonPublic | BindingFlags.Static)]
        public static void Log(object text, ConsoleColor color)
        {
            ConsoleOutput output = (ConsoleOutput)typeof(CommandWindow).GetField("output", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            ConsoleInput input = (ConsoleInput)typeof(CommandWindow).GetField("input", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            if (output == null)
            {
                Debug.Log(text);
                return;
            }
            Console.ForegroundColor = color;
            if (Console.CursorLeft != 0)
            {
                input.clearLine();
            }
            Console.WriteLine(text);
            input.redrawInputLine();
            PBServer.consoleOutput(text.ToString());
        }
    }
}
