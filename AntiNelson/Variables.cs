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
        public static string serverPath = Directory.GetCurrentDirectory() + @"\Servers\" + Provider.serverName.ToUpper();

        public static string modsPathServer = serverPath + @"\Mods";
        public static string pluginsPathServer = serverPath + @"\Plugins";

        public static string modsPathClient = Directory.GetCurrentDirectory() + @"\Mods";

        public static AppDomainSetup ads = new AppDomainSetup();
        public static Dictionary<Assembly, AppDomain> plugins = new Dictionary<Assembly, AppDomain>();

        public static bool isServer = false;
    }
}
