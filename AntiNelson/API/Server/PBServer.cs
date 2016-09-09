using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Server.Extensions;

namespace PointBlank.API.Server
{
    public class PBServer
    {
        #region Variables
        private static PBServer _instance;
        private static List<PBPlayer> _players = new List<PBPlayer>();
        private static List<PBCommand> _commands = new List<PBCommand>();
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

        public static List<PBCommand> commands
        {
            get
            {
                return _commands;
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
        public static PBCommand findCommand(string permissionOrName)
        {
            return Array.Find(commands.ToArray(), a => a.permission == permissionOrName || a.name == permissionOrName);
        }

        public static PBPlayer findPlayer(string name)
        {
            return Array.Find(players.ToArray(), a => a.playerID.playerName == name || a.playerID.nickName == name);
        }

        public static PBPlayer findPlayer(CSteamID steamID)
        {
            return Array.Find(players.ToArray(), a => a.steamID == steamID);
        }

        public static PBPlayer findPlayer(ulong sID)
        {
            return Array.Find(players.ToArray(), a => a.steamID.m_SteamID == sID);
        }

        public static PBPlayer findPlayer(SteamPlayer sPlayer)
        {
            return Array.Find(players.ToArray(), a => a.steamPlayer == sPlayer);
        }

        public static PBPlayer findPlayer(Player player)
        {
            return Array.Find(players.ToArray(), a => a.player == player);
        }

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

        #region Event Functions
        public static void ClientConnect(SteamPlayer player)
        {
            if (findPlayer(player) == null)
                players.Add(new PBPlayer(player));
        }

        public static void ClientDisconnect(SteamPlayer player)
        {
            PBPlayer ply = findPlayer(player);
            if (ply != null)
                players.Remove(ply);
        }
        #endregion
    }
}
