using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace PointBlank.API.Extensions
{
    public class PBSteamGroup
    {
        #region Variables
        private string _name;
        private Texture2D _icon;
        private CSteamID _steamID;
        private List<string> _permissions = new List<string>();
        #endregion

        #region Properties
        public string name
        {
            get
            {
                return _name;
            }
        }

        public Texture2D icon
        {
            get
            {
                return _icon;
            }
        }

        public CSteamID steamID
        {
            get
            {
                return _steamID;
            }
        }

        public List<string> permissions
        {
            get
            {
                return _permissions;
            }
        }
        #endregion

        public PBSteamGroup(string name, Texture2D icon, CSteamID steamID)
        {
            _name = name;
            _icon = icon;
            _steamID = steamID;
        }
    }
}
