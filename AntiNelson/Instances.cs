using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.PB_Library;
using PointBlank.PB_Extensions;

namespace PointBlank
{
    internal class Instances
    {
        public static lib_PluginManager pluginManager;
        public static lib_AutoSave autoSave;
        public static lib_CodeReplacer codeReplacer;
        public static lib_CommandManager commandManager;
        public static lib_EventInitalizer eventInitalizer;
        public static lib_RCON RCON;
        public static lib_Sync sync;
    }
}
