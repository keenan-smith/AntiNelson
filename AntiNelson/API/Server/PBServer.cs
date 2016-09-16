using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Enumerables;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Extensions;

namespace PointBlank.API.Server
{
    public class PBServer
    {
        #region Variables
        private static PBServer _instance;
        private static List<PBPlayer> _players = new List<PBPlayer>();
        private static List<PBCommand> _commands = new List<PBCommand>();
        private static List<PBGroup> _groups = new List<PBGroup>();
        private static List<PBSteamGroup> _steamGroups = new List<PBSteamGroup>();

        private static PBSaving _playerSave;
        private static PBSaving _groupSave;
        private static PBSaving _steamGroupSave;
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

        public static List<PBGroup> groups
        {
            get
            {
                return _groups;
            }
        }

        public static List<PBSteamGroup> steamGroups
        {
            get
            {
                return _steamGroups;
            }
        }

        public static PBSaving playerSave
        {
            get
            {
                return _playerSave;
            }
        }

        public static PBSaving groupSave
        {
            get
            {
                return _groupSave;
            }
        }

        public static PBSaving steamGroupSave
        {
            get
            {
                return _steamGroupSave;
            }
        }
        #endregion

        #region Handlers
        public delegate void ClientJoinHandler(PBPlayer player);
        public delegate void ClientLeaveHandler(PBPlayer player);
        #endregion

        #region Events
        public static event ClientJoinHandler OnPlayerJoin;
        public static event ClientLeaveHandler OnPlayerLeave;
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
        public static PBCommand findCommand(string command)
        {
            return Array.Find(commands.ToArray(), a => a.command == command || Array.Exists(a.alias.ToArray(), b => b == command));
        }

        public static PBCommand findCommandPermission(string permission)
        {
            return Array.Find(commands.ToArray(), a => a.permission == permission);
        }

        public static PBPlayer findPlayer(string name)
        {
            return Array.Find(players.ToArray(), a => a.playerID.playerName.Contains(name) || a.playerID.nickName.Contains(name));
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

        public static void broadcastChat(string message, Color color)
        {
            foreach (PBPlayer player in players)
            {
                player.sendChatMessage(message, color);
            }
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
        public static void PBPostInit()
        {
            _steamGroupSave = new PBSaving(Variables.currentPath + "\\Saves\\SteamGroups.dat", ESaveType.STEAMGROUP);
            _steamGroupSave.loadSteamGroups();
            _groupSave = new PBSaving(Variables.currentPath + "\\Saves\\Groups.dat", ESaveType.GROUP);
            _groupSave.loadGroups();
            _playerSave = new PBSaving(Variables.currentPath + "\\Saves\\Players.dat", ESaveType.PLAYER);
        }

        public static void PBPreInit()
        {

        }

        public static void PlayerJoin(PBPlayer player)
        {
            playerSave.loadPlayer(player);
        }

        public static void PlayerLeave(PBPlayer player)
        {
            playerSave.savePlayer(player);
        }

        public static void ClientConnect(SteamPlayer player)
        {
            if (findPlayer(player) == null)
            {
                PBPlayer ply = new PBPlayer(player);
                OnPlayerJoin(ply);
                players.Add(ply);
            }
        }

        public static void ClientDisconnect(SteamPlayer player)
        {
            PBPlayer ply = findPlayer(player);
            if (ply != null)
            {
                OnPlayerLeave(ply);
                players.Remove(ply);
            }
        }
        #endregion
    }
}
