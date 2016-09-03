﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Extensions;
using SDG.SteamworksProvider;

namespace PointBlank.API
{
    public class PBPlayer
    {

        #region RandomVars
        private byte[] prevHash = null;
        private bool testingAntiSpy = false;
        #endregion

        #region Variables
        private SteamPlayer _steamPlayer;
        private uint _IP;
        private PlayerProfile _profile;
        private List<string> _permissions = new List<string>();
        private List<PBGroup> _groups = new List<PBGroup>();
        private List<PBCooldown> _cooldowns = new List<PBCooldown>();
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
        }

        #region Functions
        public void ban(string reason, uint duration, bool IPBan, CSteamID judge)
        {
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
            Provider.kick(steamID, reason);
        }

        public void openURL(string url, string message)
        {
            player.sendBrowserRequest(message, url);
        }

        public void teleportToPlayer(Player player)
        {
            this.player.transform.position = player.transform.position;
        }

        public void teleportToPosition(Vector3 pos)
        {
            player.transform.position = pos;
        }

        public void giveItem(ushort id, byte amount = 1, byte quality = 255)
        {
            player.inventory.tryAddItem(new Item(id, amount, quality), true);
        }

        public bool hasPermission(string permission)
        {
            string[] sPerm = permission.Split('.');
            bool pUser1 = Array.Exists(permissions.ToArray(), a => a == sPerm[0]);
            bool pUser2 = Array.Exists(permissions.ToArray(), a => a == permission);
            bool pGroup1 = Array.Exists(groups.ToArray(), a => Array.Exists(a.permissions.ToArray(), b => b == sPerm[0]));
            bool pGroup2 = Array.Exists(groups.ToArray(), a => Array.Exists(a.permissions.ToArray(), b => b == permission));

            return (pUser1 || pUser2 || pGroup1 || pGroup2);
        }

        public bool hasCooldown(PBCommand command)
        {
            PBCooldown cDown = Array.Find(cooldowns.ToArray(), a => a.command == command);
            return (cDown != null && !cDown.cooldown);
        }

        public bool hasReachedLimit(PBCommand command, int maxUse)
        {
            PBCooldown cDown = Array.Find(cooldowns.ToArray(), a => a.command == command);
            return (cDown != null && cDown.usage > maxUse);
        }

        public void testAntiSpy() // NOT DONE!
        {
        }
        #endregion

        #region Events
        private void onSpyReady(object sender, PlayerSpyReady args) // NOT DONE!
        {
            if (testingAntiSpy)
            {
                if (prevHash == null)
                {
                    
                }
            }
        }
        #endregion
    }
}
