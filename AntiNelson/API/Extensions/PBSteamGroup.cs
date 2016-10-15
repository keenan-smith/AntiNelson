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
        /// <summary>
        /// Steam group name.
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Steam group steam ID
        /// </summary>
        public ulong steamID
        {
            get
            {
                return _steamID;
            }
        }

        /// <summary>
        /// Steam group permissions
        /// </summary>
        public List<string> permissions
        {
            get
            {
                return _permissions;
            }
        }
        #endregion

        /// <summary>
        /// The steam group instance. These can be found player.
        /// </summary>
        /// <param name="name">Steam group name.</param>
        /// <param name="steamID">Steam group steam ID.</param>
        /// <param name="perms">Steam group permissions.</param>
        public PBSteamGroup(string name, ulong steamID, string[] perms)
        {
            _name = name;
            _steamID = steamID;
            _permissions.AddRange(perms);
        }
    }
}
