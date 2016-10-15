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
        /// <summary>
        /// Ran after everything is executed.
        /// </summary>
        public static event PBPostInit OnPBPostInit;
        /// <summary>
        /// Ran before everything is executed.
        /// </summary>
        public static event PBPreInit OnPBPreInit;
        #endregion

        #region Functions
        internal static void preInit()
        {
            OnPBPreInit();
        }

        internal static void postInit()
        {
            OnPBPostInit();
        }

        /// <summary>
        /// Checks if the mod is the server or client.
        /// </summary>
        /// <returns>If the mod is on the server.</returns>
        public static bool isServer()
        {
            return (Provider.isServer || Dedicator.isDedicated);
        }

        /// <summary>
        /// Gets the current working directory(if server then server location, if client then client location).
        /// </summary>
        /// <returns>Current working directory</returns>
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
