using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.API
{
    public class PB
    {
        #region Functions
        public static bool isServer()
        {
            return (Provider.isServer || Dedicator.isDedicated);
        }

        public static string getWorkingDirectory()
        {
            string path = Directory.GetCurrentDirectory(); // Unturned directory...
            if (isServer())
                path = path + "\\Servers\\" + PBServer.serverSaveName; // I hope...
            return path;
        }
        #endregion
    }
}
