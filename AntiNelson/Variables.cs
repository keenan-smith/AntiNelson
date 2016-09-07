using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using SDG.Unturned;

namespace PointBlank
{
    public static class Variables
    {

        public static string serverPath = Directory.GetCurrentDirectory() + @"\Servers\" + Provider.serverID;

        public static string modsPathServer = serverPath + @"\Mods\";

        public static string pluginsPathServer = serverPath + @"\Plugins\";

        public static string modsPathClient = Directory.GetCurrentDirectory() + @"\Mods\";

        public static string currentPath = Directory.GetCurrentDirectory() + @"\Unturned_Data\Managed\";

        public static bool isServer = false;

    }
}
