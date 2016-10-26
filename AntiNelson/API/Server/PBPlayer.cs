using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Extensions;
using PointBlank.API.Server.Types;
using SDG.SteamworksProvider;

namespace PointBlank.API.Server
{
    public class PBPlayer
    {
        #region Variables
        private SteamPlayer _steamPlayer;
        private uint _IP;
        private PlayerProfile _profile;
        private List<string> _permissions = new List<string>();
        private List<PBGroup> _groups = new List<PBGroup>();
        private List<PBCooldown> _cooldowns = new List<PBCooldown>();
        private Dictionary<string, CustomVariable> _customVariables = new Dictionary<string, CustomVariable>();
        private List<string> _saveKeys = new List<string>();
        private Color _playerColor;
        private bool _isServer;
        private List<PBPlayer> _visiblePlayers = new List<PBPlayer>();
        #endregion

        #region Properties
        /// <summary>
        /// The player permissions.
        /// </summary>
        public List<string> permissions
        {
            get
            {
                return _permissions;
            }
        }

        /// <summary>
        /// The player groups.
        /// </summary>
        public List<PBGroup> groups
        {
            get
            {
                return _groups;
            }
        }

        /// <summary>
        /// The player cooldowns.
        /// </summary>
        public List<PBCooldown> cooldowns
        {
            get
            {
                return _cooldowns;
            }
        }

        /// <summary>
        /// The steam player instance.
        /// </summary>
        public SteamPlayer steamPlayer
        {
            get
            {
                return _steamPlayer;
            }
        }

        /// <summary>
        /// The player instance.
        /// </summary>
        public Player player
        {
            get
            {
                return _steamPlayer.player;
            }
        }

        /// <summary>
        /// The steam player ID instance.
        /// </summary>
        public SteamPlayerID playerID
        {
            get
            {
                return _steamPlayer.playerID;
            }
        }

        /// <summary>
        /// The steamID instance.
        /// </summary>
        public CSteamID steamID
        {
            get
            {
                return _steamPlayer.playerID.steamID;
            }
        }

        /// <summary>
        /// The steam64 of the player.
        /// </summary>
        public ulong steam64
        {
            get
            {
                return _steamPlayer.playerID.steamID.m_SteamID;
            }
        }

        /// <summary>
        /// The IP of the player.
        /// </summary>
        public uint IP
        {
            get
            {
                return _IP;
            }
        }

        /// <summary>
        /// The steam profile instance of the player.
        /// </summary>
        public PlayerProfile steamProfile
        {
            get
            {
                return _profile;
            }
        }

        /// <summary>
        /// The steam groups of the player.
        /// </summary>
        public ulong[] steamGroups
        {
            get
            {
                return steamProfile.groups;
            }
        }

        /// <summary>
        /// The player skills.
        /// </summary>
        public PBSkills[] skills
        {
            get
            {
                return PBSkills.getSkills(this);
            }
        }

        /// <summary>
        /// Custom variables for the player.
        /// </summary>
        public Dictionary<string, CustomVariable> customVariables
        {
            get
            {
                return _customVariables;
            }
        }

        /// <summary>
        /// The keys that get saved when the server shuts down.
        /// </summary>
        public List<string> saveKeys
        {
            get
            {
                return _saveKeys;
            }
        }

        /// <summary>
        /// The chat color of the player.
        /// </summary>
        public Color playerColor
        {
            get
            {
                return _playerColor;
            }
            set
            {
                _playerColor = value;
            }
        }

        /// <summary>
        /// Returns if the player is a server.
        /// </summary>
        public bool isServer
        {
            get
            {
                return _isServer;
            }
        }

        /// <summary>
        /// List of players visible to this specific player.
        /// </summary>
        public List<PBPlayer> visiblePlayers
        {
            get
            {
                return _visiblePlayers;
            }
        }

        /// <summary>
        /// List of all players this player is visible to.
        /// </summary>
        public PBPlayer[] visibleTo
        {
            get
            {
                List<PBPlayer> ls = new List<PBPlayer>();
                foreach (PBPlayer ply in PBServer.players)
                    if (ply != this && ply.visiblePlayers.Contains(this))
                        ls.Add(ply);
                return ls.ToArray();
            }
        }
        #endregion

        #region Handlers
        public delegate void PlayerHurtHandler(PBPlayer victim, PBPlayer attacker, byte damage, ELimb limb, Vector3 force);
        public delegate void PlayerKilledHandler(PBPlayer victim, PBPlayer attacker, byte damage, EDeathCause cause, ELimb limb, Vector3 force);
        public delegate void PlayerMoveHandler(PBPlayer player, Vector3 position, Quaternion rotation);
        #endregion

        #region Events
        /// <summary>
        /// Called when player is hurt.
        /// </summary>
        public static event PlayerHurtHandler OnPlayerHurt;
        /// <summary>
        /// Called when player is killed.
        /// </summary>
        public static event PlayerKilledHandler OnPlayerKilled;
        /// <summary>
        /// Called when a player moves.
        /// </summary>
        public static event PlayerMoveHandler OnPlayerMove;
        #endregion

        /// <summary>
        /// Create a normal player from the steam player.
        /// </summary>
        /// <param name="p">The steam player instance.</param>
        public PBPlayer(SteamPlayer p)
        {
            _steamPlayer = p;

            P2PSessionState_t sState;
            if (SteamGameServerNetworking.GetP2PSessionState(steamID, out sState))
                _IP = sState.m_nRemoteIP;
            else
                _IP = 0u;

            _profile = new PlayerProfile("http://steamcommunity.com/profiles/" + steam64 + "/?xml=1");
            _playerColor = Color.white;
        }

        /// <summary>
        /// Create a fake/server player.
        /// </summary>
        public PBPlayer()
        {
            _isServer = true;
            _playerColor = Color.white;
        }

        #region Functions
        internal static void playerHurtEvent(Player ply, byte damage, Vector3 force, EDeathCause cause, ELimb limb, CSteamID killer)
        {
            if (OnPlayerHurt != null)
                OnPlayerHurt(PBServer.findPlayer(ply), PBServer.findPlayer(killer), damage, limb, force);

            if (ply.life.isDead || ply.life.health < 1 || ply.life.health - damage < 1)
                if (OnPlayerKilled != null)
                    OnPlayerKilled(PBServer.findPlayer(ply), PBServer.findPlayer(killer), damage, cause, limb, force);
        }

        internal static void playerMoveEvent(PBPlayer player, Vector3 position, Quaternion rotation)
        {
            if (OnPlayerMove != null)
                OnPlayerMove(player, position, rotation);
        }

        /// <summary>
        /// Bans the player.
        /// </summary>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="duration">The duration of the ban.</param>
        /// <param name="IPBan">Should the person be IP banned.</param>
        /// <param name="judge">The person who is banning the player.</param>
        public void ban(string reason, uint duration, bool IPBan, CSteamID judge)
        {
            if (isServer)
                return;
            SteamBlacklist.ban(
                steamID,
                (IPBan ? IP : 0u),
                judge,
                reason,
                duration
            );
        }

        /// <summary>
        /// Kicks the player.
        /// </summary>
        /// <param name="reason">The reason for the kick.</param>
        public void kick(string reason)
        {
            if (isServer)
                return;
            Provider.kick(steamID, reason);
        }

        /// <summary>
        /// Opens the URL on the client.
        /// </summary>
        /// <param name="url">The URL you want to open.</param>
        /// <param name="message">The message that comes with it.</param>
        public void openURL(string url, string message)
        {
            if (isServer)
                return;
            player.sendBrowserRequest(message, url);
        }

        /// <summary>
        /// Teleports to another player.
        /// </summary>
        /// <param name="player">The player you want to teleport to.</param>
        public void teleportToPlayer(Player player)
        {
            if (isServer)
                return;
            this.player.transform.position = player.transform.position;
        }

        /// <summary>
        /// Teleports to a position.
        /// </summary>
        /// <param name="pos">The position you want to teleport to.</param>
        public void teleportToPosition(Vector3 pos)
        {
            if (isServer)
                return;
            player.transform.position = pos;
        }

        /// <summary>
        /// Gives an item to the player.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <param name="amount">Item amount.</param>
        /// <param name="quality">Item quality.</param>
        public void giveItem(ushort id, byte amount = 1, byte quality = 255)
        {
            if (isServer)
                return;
            player.inventory.tryAddItem(new Item(id, amount, quality), true);
        }

        /// <summary>
        /// Check if a player has a specific permission.
        /// </summary>
        /// <param name="permission">The permission you want to check for.</param>
        /// <returns>Does the player have the permission.</returns>
        public bool hasPermission(string permission)
        {
            if (isServer)
                return true;
            string[] sPerm = permission.Split('.');

            if (permission == "")
                return true;
            foreach (string perm in permissions)
            {
                string[] pPerm = perm.Split('.');

                if (perm == "*" || perm == permission)
                    return true;
                for (int i = 0; i < sPerm.Length; i++)
                {
                    if (pPerm[i] == "*")
                        return true;
                    if (pPerm[i] != sPerm[i])
                        break;
                    if (i >= sPerm.Length)
                        return true;
                }
            }

            foreach (PBGroup group in groups)
            {
                foreach (string perm in group.permissions)
                {
                    string[] pPerm = perm.Split('.');

                    if (perm == "*" || perm == permission)
                        return true;
                    for (int i = 0; i < sPerm.Length; i++)
                    {
                        if (pPerm[i] == "*")
                            return true;
                        if (pPerm[i] != sPerm[i])
                            break;
                        if (i >= sPerm.Length)
                            return true;
                    }
                }
            }

            foreach (PBSteamGroup steamGroup in PBServer.steamGroups)
            {
                if (Array.Exists(steamGroups, a => a == steamGroup.steamID))
                {
                    foreach (string perm in steamGroup.permissions)
                    {
                        string[] pPerm = perm.Split('.');

                        if (perm == "*" || perm == permission)
                            return true;
                        for (int i = 0; i < sPerm.Length; i++)
                        {
                            if (pPerm[i] == "*")
                                return true;
                            if (pPerm[i] != sPerm[i])
                                break;
                            if (i >= sPerm.Length)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the player has a cooldown on a command.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>If the player has cooldown.</returns>
        public bool hasCooldown(PBCommand command)
        {
            if (isServer)
                return false;
            PBCooldown cDown = Array.Find(cooldowns.ToArray(), a => a.command == command);
            return (cDown != null && cDown.cooldown);
        }

        /// <summary>
        /// Checks if the player has reached max command execution.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="maxUse">How many times the command has been used.</param>
        /// <returns>If the player has reached the limit.</returns>
        public bool hasReachedLimit(PBCommand command, int maxUse)
        {
            if (isServer)
                return false;
            PBCooldown cDown = Array.Find(cooldowns.ToArray(), a => a.command == command);
            return (cDown != null && cDown.usage >= maxUse);
        }

        /// <summary>
        /// Return the custom variable of the player.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <returns>Custom variable value.</returns>
        public object getCustomVariable(string key)
        {
            KeyValuePair<string, CustomVariable> kvp = Array.Find(customVariables.ToArray(), a => a.Key == key);
            if (kvp.Equals(default(KeyValuePair<string, CustomVariable>)))
                return null;
            return kvp.Value.getValue();
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, bool value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, byte value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, short value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, int value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, long value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, float value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, double value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Adds/sets custom variable.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <param name="value">The value of the custom variable.</param>
        /// <param name="save">Save the custom variable.</param>
        public void setCustomVariable(string key, string value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        /// <summary>
        /// Removes custom variable.
        /// </summary>
        /// <param name="key">The key of the variable.</param>
        public void removeCustomVariable(string key)
        {
            customVariables.Remove(key);
            saveKeys.Remove(key);
        }

        /// <summary>
        /// Sends chat message to the player.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="color">The color to send.</param>
        public void sendChatMessage(string message, Color color)
        {
            if (isServer)
                CommandWindow.Log(message);
            else
                PBChat.sendChatToPlayer(this, message, color);
        }
        #endregion
    }
}
