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
        #endregion

        #region Properties
        public List<string> permissions
        {
            get
            {
                return _permissions;
            }
        }

        public List<PBGroup> groups
        {
            get
            {
                return _groups;
            }
        }

        public List<PBCooldown> cooldowns
        {
            get
            {
                return _cooldowns;
            }
        }

        public SteamPlayer steamPlayer
        {
            get
            {
                return _steamPlayer;
            }
        }

        public Player player
        {
            get
            {
                return _steamPlayer.player;
            }
        }

        public SteamPlayerID playerID
        {
            get
            {
                return _steamPlayer.playerID;
            }
        }

        public CSteamID steamID
        {
            get
            {
                return _steamPlayer.playerID.steamID;
            }
        }

        public ulong steam64
        {
            get
            {
                return _steamPlayer.playerID.steamID.m_SteamID;
            }
        }

        public uint IP
        {
            get
            {
                return _IP;
            }
        }

        public PlayerProfile steamProfile
        {
            get
            {
                return _profile;
            }
        }

        public SteamGroup[] steamGroups
        {
            get
            {
                return Provider.provider.communityService.getGroups();
            }
        }

        public PBSkills[] skills
        {
            get
            {
                return PBSkills.getSkills(this);
            }
        }

        public Dictionary<string, CustomVariable> customVariables
        {
            get
            {
                return _customVariables;
            }
        }

        public List<string> saveKeys
        {
            get
            {
                return _saveKeys;
            }
        }

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

        public bool isServer
        {
            get
            {
                return _isServer;
            }
        }
        #endregion

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

        public PBPlayer()
        {
            _isServer = true;
            _playerColor = Color.white;
        }

        #region Functions
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

        public void kick(string reason)
        {
            if (isServer)
                return;
            Provider.kick(steamID, reason);
        }

        public void openURL(string url, string message)
        {
            if (isServer)
                return;
            player.sendBrowserRequest(message, url);
        }

        public void teleportToPlayer(Player player)
        {
            if (isServer)
                return;
            this.player.transform.position = player.transform.position;
        }

        public void teleportToPosition(Vector3 pos)
        {
            if (isServer)
                return;
            player.transform.position = pos;
        }

        public void giveItem(ushort id, byte amount = 1, byte quality = 255)
        {
            if (isServer)
                return;
            player.inventory.tryAddItem(new Item(id, amount, quality), true);
        }

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
                if (Array.Exists(steamGroups, a => a.steamID.m_SteamID == steamGroup.steamID))
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

        public bool hasCooldown(PBCommand command)
        {
            if (isServer)
                return false;
            PBCooldown cDown = Array.Find(cooldowns.ToArray(), a => a.command == command);
            return (cDown != null && cDown.cooldown);
        }

        public bool hasReachedLimit(PBCommand command, int maxUse)
        {
            if (isServer)
                return false;
            PBCooldown cDown = Array.Find(cooldowns.ToArray(), a => a.command == command);
            return (cDown != null && cDown.usage >= maxUse);
        }

        public object getCustomVariable(string key)
        {
            KeyValuePair<string, CustomVariable> kvp = Array.Find(customVariables.ToArray(), a => a.Key == key);
            if (kvp.Equals(default(KeyValuePair<string, CustomVariable>)))
                return null;
            return kvp.Value.getValue();
        }

        public void setCustomVariable(string key, bool value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void setCustomVariable(string key, byte value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void setCustomVariable(string key, short value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void setCustomVariable(string key, int value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void setCustomVariable(string key, long value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void setCustomVariable(string key, float value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void setCustomVariable(string key, double value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void setCustomVariable(string key, string value, bool save = false)
        {
            customVariables.Remove(key);
            customVariables.Add(key, new CustomVariable(value));
            if (save)
                saveKeys.Add(key);
        }

        public void removeCustomVariable(string key)
        {
            customVariables.Remove(key);
            saveKeys.Remove(key);
        }

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
