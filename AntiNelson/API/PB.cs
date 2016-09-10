using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API.Server;

namespace PointBlank.API
{
    public class PB
    {
        #region Handlers
        public delegate void PBPostInit();
        public delegate void PBPreInit();
        #endregion

        #region Events
        public static event PBPostInit OnPBPostInit;
        public static event PBPreInit OnPBPreInit;
        #endregion

        #region Functions
        public static void preInit()
        {
            OnPBPreInit();
        }

        public static void postInit()
        {
            OnPBPostInit();
        }

        public static bool isServer()
        {
            return (Provider.isServer || Dedicator.isDedicated);
        }

        public static string getWorkingDirectory()
        {
            string path = Directory.GetCurrentDirectory();
            if (isServer())
                path = Variables.serverPath;
            return path;
        }
        #endregion
    }
}
