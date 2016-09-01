using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Extensions;

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
        #endregion

        #region Propertys
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
        public bool ban(string reason, uint duration, bool IPBan, CSteamID judge)
        {
            try
            {
                SteamBlacklist.ban(
                    steamID,
                    (IPBan ? IP : 0u),
                    judge,
                    reason,
                    duration
                );
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Console.WriteLine("ERROR: Exception while attempting to ban player!");
                return false;
            }
        }

        public bool kick(string reason)
        {
            try
            {
                Provider.kick(steamID, reason);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Console.WriteLine("ERROR: Exception while attempting to kick player!");
                return false;
            }
        }

        public bool openURL(string url, string message)
        {
            try
            {
                player.sendBrowserRequest(message, url);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Console.WriteLine("ERROR: Exception while attempting to open url for player!");
                return false;
            }
        }

        public bool teleportToPlayer(Player player)
        {
            try
            {
                this.player.transform.position = player.transform.position;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Console.WriteLine("ERROR: Exception while attempting to teleport player!");
                return false;
            }
        }

        public bool teleportToPosition(Vector3 pos)
        {
            try
            {
                player.transform.position = pos;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Console.WriteLine("ERROR: Exception while attempting to teleport player!");
                return false;
            }
        }

        public bool testAntiSpy() // NOT DONE!
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Console.WriteLine("ERROR: Exception while attempting to test player for antispy!");
                return false;
            }
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
