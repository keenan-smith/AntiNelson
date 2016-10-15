using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.API.Extensions;

namespace PointBlank.API.Client
{
    internal class PBPlayer
    {
        #region Variables
        private static PBPlayer _instance;
        private SteamPlayer _steamPlayer;
        private PlayerProfile _profile;
        private Dictionary<string, object> _customVariables = new Dictionary<string, object>();
        #endregion

        #region Properties
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
                return PBSkills.getSkills();
            }
        }

        public Dictionary<string, object> customVariables
        {
            get
            {
                return _customVariables;
            }
        }

        public static PBPlayer instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        public PBPlayer()
        {
            _steamPlayer = PlayerTool.getSteamPlayer(Provider.client);

            _instance = this;

            _profile = new PlayerProfile("http://steamcommunity.com/profiles/" + steam64 + "/?xml=1");
        }

        #region Functions
        #endregion
    }
}
