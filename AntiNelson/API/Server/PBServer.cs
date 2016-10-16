using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Enumerables;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Extensions;
using PointBlank.API.Server.Attributes;

namespace PointBlank.API.Server
{
    public class PBServer
    {
        #region Variables
        private static PBPlayer _server;
        private static List<PBPlayer> _players = new List<PBPlayer>();
        private static List<PBCommand> _commands = new List<PBCommand>();
        private static List<PBGroup> _groups = new List<PBGroup>();
        private static List<PBSteamGroup> _steamGroups = new List<PBSteamGroup>();

        private static PBSaving _playerSave;
        private static PBSaving _groupSave;
        private static PBSaving _steamGroupSave;
        #endregion

        #region Properties
        /// <summary>
        /// Max players in the server.
        /// </summary>
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

        /// <summary>
        /// Gets the server name.
        /// </summary>
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

        /// <summary>
        /// Gets the server save name.
        /// </summary>
        public static string serverSaveName
        {
            get
            {
                return serverName.ToUpper();
            }
        }

        /// <summary>
        /// Gets the server password.
        /// </summary>
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

        /// <summary>
        /// Gets all the clients/players in the server.
        /// </summary>
        public static SteamPlayer[] clients
        {
            get
            {
                return Provider.clients.ToArray();
            }
        }

        /// <summary>
        /// Gets all the players in the server.
        /// </summary>
        public static List<PBPlayer> players
        {
            get
            {
                return _players;
            }
        }

        /// <summary>
        /// Gets the uptime of the server.
        /// </summary>
        public static float upTime
        {
            get
            {
                return Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// All commands in the server.
        /// </summary>
        public static List<PBCommand> commands
        {
            get
            {
                return _commands;
            }
        }
        
        /// <summary>
        /// All the custom groups in the server.
        /// </summary>
        public static List<PBGroup> groups
        {
            get
            {
                return _groups;
            }
        }

        /// <summary>
        /// All the steam groups in the server.
        /// </summary>
        public static List<PBSteamGroup> steamGroups
        {
            get
            {
                return _steamGroups;
            }
        }

        /// <summary>
        /// Gets the player save.
        /// </summary>
        public static PBSaving playerSave
        {
            get
            {
                return _playerSave;
            }
        }

        /// <summary>
        /// Get the group save.
        /// </summary>
        public static PBSaving groupSave
        {
            get
            {
                return _groupSave;
            }
        }

        /// <summary>
        /// Get the steam group save.
        /// </summary>
        public static PBSaving steamGroupSave
        {
            get
            {
                return _steamGroupSave;
            }
        }

        /// <summary>
        /// Gets the server player.
        /// </summary>
        public static PBPlayer server
        {
            get
            {
                return _server;
            }
        }
        #endregion

        #region Handlers
        public delegate void ClientJoinHandler(PBPlayer player);
        public delegate void ClientLeaveHandler(PBPlayer player);
        public delegate void ConsoleInputTextHandler(string command);
        public delegate void ConsoleOutputTextHandler(string text);
        #endregion

        #region Events
        /// <summary>
        /// Gets called when a player joins.
        /// </summary>
        public static event ClientJoinHandler OnPlayerJoin;
        /// <summary>
        /// Gets called when a player leaves.
        /// </summary>
        public static event ClientLeaveHandler OnPlayerLeave;
        /// <summary>
        /// Gets called when a command is entered into the console.
        /// </summary>
        public static event ConsoleInputTextHandler OnConsoleInput;
        /// <summary>
        /// Gets called when the console outputs text.
        /// </summary>
        public static event ConsoleOutputTextHandler OnConsoleOutput;
        #endregion

        #region Functions
        /// <summary>
        /// Find a command based on the execution command.
        /// </summary>
        /// <param name="command">The execution command.</param>
        /// <returns>The command instance.</returns>
        public static PBCommand findCommand(string command)
        {
            return Array.Find(commands.ToArray(), a => a.command.ToLower() == command.ToLower() || Array.Exists(a.alias.ToArray(), b => b.ToLower() == command.ToLower()));
        }

        /// <summary>
        /// Find the command based on the permission.
        /// </summary>
        /// <param name="permission">The command permission.</param>
        /// <returns>The command instance.</returns>
        public static PBCommand findCommandPermission(string permission)
        {
            return Array.Find(commands.ToArray(), a => a.permission == permission);
        }

        /// <summary>
        /// Find a player based on the name.
        /// </summary>
        /// <param name="name">Player name.</param>
        /// <returns>Player instance.</returns>
        public static PBPlayer findPlayer(string name)
        {
            return Array.Find(players.ToArray(), a => a.playerID.playerName.Contains(name) || a.playerID.nickName.Contains(name));
        }

        /// <summary>
        /// Finds the player based on steamid instance.
        /// </summary>
        /// <param name="steamID">SteamID instance.</param>
        /// <returns>The player instance.</returns>
        public static PBPlayer findPlayer(CSteamID steamID)
        {
            return Array.Find(players.ToArray(), a => a.steamID == steamID);
        }

        /// <summary>
        /// Finds player based on the steam64 id.
        /// </summary>
        /// <param name="sID">Steam 64 ID.</param>
        /// <returns>The player instance.</returns>
        public static PBPlayer findPlayer(ulong sID)
        {
            return Array.Find(players.ToArray(), a => a.steamID.m_SteamID == sID);
        }

        /// <summary>
        /// Gets the player based on the steam player instance.
        /// </summary>
        /// <param name="sPlayer">Steam player instance.</param>
        /// <returns>The player instance.</returns>
        public static PBPlayer findPlayer(SteamPlayer sPlayer)
        {
            return Array.Find(players.ToArray(), a => a.steamPlayer == sPlayer);
        }

        /// <summary>
        /// Gets the player based on the player object instance.
        /// </summary>
        /// <param name="player">Player object instance.</param>
        /// <returns>The player instance.</returns>
        public static PBPlayer findPlayer(Player player)
        {
            return Array.Find(players.ToArray(), a => a.player == player);
        }

        /// <summary>
        /// Broadcasts a message to all the players in the server.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="color">The color of the message.</param>
        public static void broadcastChat(string message, Color color)
        {
            foreach (PBPlayer player in players)
            {
                player.sendChatMessage(message, color);
            }
        }

        internal static void consoleInput(string command)
        {
            if(OnConsoleOutput != null)
                OnConsoleInput(command);
        }

        internal static void consoleOutput(string text)
        {
            if(OnConsoleInput != null)
                OnConsoleOutput(text);
        }

        /// <summary>
        /// Restarts the server.
        /// </summary>
        /// <returns>If the server has been restarted.</returns>
        public static bool restart()
        {
            try
            {
                string p = "";
                foreach(string path in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.lnk", SearchOption.TopDirectoryOnly))
                {
                    if (Tool.IsShortcut(path) && Tool.ResolveShortcut(path).ToLower().Contains("unturned.exe"))
                    {
                        p = path;
                        break;
                    }
                }
                Process.Start(p);
                Application.Quit();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to restart server!", ex);
                return false;
            }
        }

        /// <summary>
        /// Reloads all the plugins.
        /// </summary>
        /// <returns>If the reload was successful.</returns>
        public static bool reloadPlugins()
        {
            try
            {
                Instances.codeReplacer.shutdown();
                PBServer.commands.Clear();
                Instances.pluginManager.unloadAllPlugins();
                Instances.pluginManager.loadPlugins();
                Instances.commandManager.reload();
                Instances.codeReplacer.reload();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to reload plugins!", ex);
                return false;
            }
        }

        /// <summary>
        /// Shuts down the server.
        /// </summary>
        /// <returns>If server shutdown was successful.</returns>
        public static bool shutdown()
        {
            try
            {
                Provider.shutdown();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to shutdown server!", ex);
                return false;
            }
        }

        /// <summary>
        /// Unloads all the plugins.
        /// </summary>
        /// <returns>If the unload was successful.</returns>
        public static bool unloadPlugins()
        {
            try
            {
                Instances.codeReplacer.shutdown();
                PBServer.commands.Clear();
                Instances.pluginManager.unloadAllPlugins();
                Instances.commandManager.reload();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to unload plugins!", ex);
                return false;
            }
        }

        /// <summary>
        /// Loads a plugin.
        /// </summary>
        /// <param name="path">The path to the plugin.</param>
        /// <returns>If the plugin was loaded successfully.</returns>
        public static bool loadPlugin(string path)
        {
            try
            {
                Instances.pluginManager.loadPlugin(path);
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to load plugin!", ex);
                return false;
            }
        }

        /// <summary>
        /// Loads all the plugins.
        /// </summary>
        /// <returns>If plugins are loaded.</returns>
        public static bool loadPlugins()
        {
            try
            {
                Instances.pluginManager.loadPlugins();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to load plugins!", ex);
                return false;
            }
        }

        /// <summary>
        /// Gets all the plugins currently loaded.
        /// </summary>
        /// <returns>Loaded plugins.</returns>
        public static PluginAttribute[] getPlugins()
        {
            return Instances.pluginManager.plgs;
        }
        #endregion

        #region Event Functions
        internal static void PBPostInit()
        {
            _server = new PBPlayer();
            _steamGroupSave = new PBSaving(Variables.currentPath + "\\Saves\\SteamGroups.dat", ESaveType.STEAMGROUP);
            _steamGroupSave.loadSteamGroups();
            _groupSave = new PBSaving(Variables.currentPath + "\\Saves\\Groups.dat", ESaveType.GROUP);
            _groupSave.loadGroups();
            _playerSave = new PBSaving(Variables.currentPath + "\\Saves\\Players.dat", ESaveType.PLAYER);


        }

        internal static void PBPreInit()
        {

        }

        internal static void PlayerJoin(PBPlayer player)
        {
            playerSave.loadPlayer(player);
        }

        internal static void PlayerLeave(PBPlayer player)
        {
            playerSave.savePlayer(player);
        }

        internal static void ParseInputCommand(string command)
        {
            string[] info = command.Split(' ');
            PBCommand cmd = PBServer.findCommand(info[0]);
            if (cmd != null)
            {
                PBLogging.log("Calling: " + info[0], false);
                string[] args = (info.Length > 1 ? info[1].Split('/') : new string[0]);
                try
                {
                    cmd.onCall(server, args);
                }
                catch (Exception ex)
                {
                    PBLogging.logError("ERROR, while running command!", ex);
                }
            }
            else
            {
                Command uCmd = Array.Find(Commander.commands.ToArray(), a => a.command.ToLower() == info[0].ToLower());
                if (uCmd != null)
                {
                    PBLogging.log("Calling: " + info[0], false);
                    try
                    {
                        uCmd.check(CSteamID.Nil, info[0], (info.Length > 1 ? info[1] : ""));
                    }
                    catch (Exception ex)
                    {
                        PBLogging.logError("ERROR, while running command!", ex);
                    }
                }
            }
        }

        internal static void ClientConnect(SteamPlayer player)
        {
            if (findPlayer(player) == null)
            {
                PBPlayer ply = new PBPlayer(player);
                OnPlayerJoin(ply);
                players.Add(ply);
            }
        }

        internal static void ClientDisconnect(SteamPlayer player)
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
