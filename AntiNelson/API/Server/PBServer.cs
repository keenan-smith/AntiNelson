﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.API.Server
{
    public class PBServer
    {
        #region Variables
        private static PBServer _instance;
        private static List<PBPlayer> _players = new List<PBPlayer>();
        #endregion

        #region Properties
        public static byte maxPlayers
        {
            get
            {
                return Provider.maxPlayers;
            }
            set
            {
                Provider.maxPlayers = value;
            }
        }

        public static string serverName
        {
            get
            {
                return Provider.serverName;
            }
            set
            {
                Provider.serverName = value;
            }
        }

        public static string serverSaveName
        {
            get
            {
                return serverName.ToUpper();
            }
        }

        public static string serverPassword
        {
            get
            {
                return Provider.serverPassword;
            }
            set
            {
                Provider.serverPassword = value;
            }
        }

        public static SteamPlayer[] clients
        {
            get
            {
                return Provider.clients.ToArray();
            }
        }

        public static List<PBPlayer> players
        {
            get
            {
                return _players;
            }
        }

        public static float upTime
        {
            get
            {
                return Time.realtimeSinceStartup;
            }
        }

        public static PBServer instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        public PBServer()
        {
            _instance = this;

            foreach (SteamPlayer sp in clients)
            {
                players.Add(new PBPlayer(sp));
            }
        }

        #region Functions
        public static void broadcastChat(string message) // NOT DONE!
        {
        }

        public static bool restart() // NOT DONE!
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to restart server!", ex);
                return false;
            }
        }

        public static bool reloadPlugins() // NOT DONE!
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to reload plugins!", ex);
                return false;
            }
        }

        public static bool shutdown() // NOT DONE!
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to shutdown server!", ex);
                return false;
            }
        }
        #endregion
    }
}