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
        private ulong _steamID;
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

        public ulong steamID
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

        public PBSteamGroup(string name, ulong steamID, string[] perms)
        {
            _name = name;
            _steamID = steamID;
            _permissions.AddRange(perms);
        }
    }
}
