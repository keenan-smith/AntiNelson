﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank
{
    internal class Variables
    {
        public static GameObject obj_Commands = null;

        public static string serverPath = Directory.GetCurrentDirectory() + @"\Servers\" + Tool.getServerName();

        public static string modsPathServer = serverPath + @"\Mods\";

        public static string pluginsPathServer = serverPath + @"\Plugins\";

        public static string librariesPathServer = serverPath + @"\Libraries\";

        public static string modsPathClient = Directory.GetCurrentDirectory() + @"\Mods\";

        public static string currentPath = AppDomain.CurrentDomain.Id != 0 ? serverPath : Directory.GetCurrentDirectory();

        public static string pathManaged = Directory.GetCurrentDirectory() + @"\Unturned_Data\Managed\";

        public static bool isServer = false;

        public static bool isLoaded = false;

        public static string[] serverDirectories = new string[]
        {
            "Plugins",
            "Mods",
            "Settings",
            "Locals",
            "Saves",
            "Libraries",
        };

        public static string[] serverSaveFiles = new string[]
        {
            "Players.dat",
            "Groups.dat",
            "SteamGroups.dat",
        };

        public static string[] clientDirectories = new string[]
        {
            "Mods",
            "Settings",
            "Locals",
        };
    }
}
