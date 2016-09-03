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
        #endregion

        #region Properties
        public bool isVisible
        {
            get
            {
                return _isVisible;
            }
        }

        public bool isVacBanned
        {
            get
            {
                return _vacBanned;
            }
        }

        public bool isTradeBanned
        {
            get
            {
                return _tradeBanned;
            }
        }

        public bool isLimited
        {
            get
            {
                return _isLimited;
            }
        }

        public string link
        {
            get
            {
                return _link;
            }
        }

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
        #endregion

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
        }
    }
}
