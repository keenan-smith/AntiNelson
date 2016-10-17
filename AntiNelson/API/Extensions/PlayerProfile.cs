using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API.Enumerables;

namespace PointBlank.API.Extensions
{
    public class PlayerProfile
    {
        #region Variables
        private bool _isVisible;
        private bool _vacBanned;
        private bool _tradeBanned;
        private bool _isLimited;
        private int _privacyState; // 0 = Public, 1 = Friends Only, 2 = Private
        private string _link;
        private List<ulong> _groups = new List<ulong>();
        #endregion

        #region Properties
        /// <summary>
        /// Is the profile visible(non private).
        /// </summary>
        public bool isVisible
        {
            get
            {
                return _isVisible;
            }
        }

        /// <summary>
        /// Is the player VAC banned in any game.
        /// </summary>
        public bool isVacBanned
        {
            get
            {
                return _vacBanned;
            }
        }

        /// <summary>
        /// Is the steam player trade banned.
        /// </summary>
        public bool isTradeBanned
        {
            get
            {
                return _tradeBanned;
            }
        }

        /// <summary>
        /// Is the steam player account limited.
        /// </summary>
        public bool isLimited
        {
            get
            {
                return _isLimited;
            }
        }

        /// <summary>
        /// The steam player profile link.
        /// </summary>
        public string link
        {
            get
            {
                return _link;
            }
        }

        /// <summary>
        /// The privacy state of the player profile.
        /// </summary>
        public PrivacyState privacyState
        {
            get
            {
                if (_privacyState == 0)
                    return PrivacyState.PUBLIC;
                else if (_privacyState == 1)
                    return PrivacyState.FRIENDS_ONLY;
                else if (_privacyState == 2)
                    return PrivacyState.PRIVATE;
                else
                    return PrivacyState.NONE;
            }
        }

        /// <summary>
        /// Returns all the groups the player is in. Only works if the profile is not private.
        /// </summary>
        public ulong[] groups
        {
            get
            {
                return _groups.ToArray();
            }
        }
        #endregion

        /// <summary>
        /// Steam player profile, with the information provided by steam.
        /// </summary>
        /// <param name="url">The URL of the steam player's profile.</param>
        public PlayerProfile(string url)
        {
            _link = url;

            XmlDocument doc = new XmlDocument();
            doc.Load(url);
            XmlElement root = doc.DocumentElement;
            _isVisible = (root.SelectSingleNode("visibilityState").InnerText == "1");
            _vacBanned = (root.SelectSingleNode("vacBanned").InnerText == "1");
            _tradeBanned = (root.SelectSingleNode("tradeBanState").InnerText != "None");
            _isLimited = (root.SelectSingleNode("isLimitedAccount").InnerText == "1");
            string privacy = root.SelectSingleNode("privacyState").InnerText;
            if (privacy == "public")
                _privacyState = 0;
            else if (privacy == "friendsonly")
                _privacyState = 1;
            else if (privacy == "private")
                _privacyState = 2;
            if (root.SelectNodes("groups").Count > 0)
                foreach (XmlElement group in root.SelectSingleNode("groups").SelectNodes("group"))
                    _groups.Add(ulong.Parse(group.SelectSingleNode("groupID64").InnerText));
        }
    }
}
