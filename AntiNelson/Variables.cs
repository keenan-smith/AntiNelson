using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using SDG.Unturned;

namespace PointBlank
{
    public static class Variables
    {
        public static string serverPath = Directory.GetCurrentDirectory() + @"\Servers\" + Provider.serverName.ToUpper();

        public static string modsPath = Directory.GetCurrentDirectory() + @"\Mods";
        public static string pluginsPath = Directory.GetCurrentDirectory() + @"\Plugins";
    }
}
