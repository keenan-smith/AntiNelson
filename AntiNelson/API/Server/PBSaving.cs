using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.API.Server
{
    public class PBSaving
    {
        #region Variables
        private XmlDocument _doc;
        private XmlElement _root;
        private string _path;
        #endregion

        #region Properties
        public XmlDocument document
        {
            get
            {
                return _doc;
            }
        }

        public XmlElement rootElement
        {
            get
            {
                return _root;
            }
        }

        public string path
        {
            get
            {
                return _path;
            }
        }
        #endregion

        public PBSaving(string path)
        {
            _path = path;

            if (ReadWrite.fileExists(path, false, false))
            {
                _doc = new XmlDocument();
                _doc.Load(path);
                _root = _doc.DocumentElement;
            }
            else
            {
                PBLogging.logWarning("Could not load players!");
            }
        }

        #region Functions
        public void save()
        {
            document.Save(path);
        }
        #endregion

        #region Functions - Player
        public void loadPlayer(PBPlayer player)
        {
            foreach (XmlElement ele in rootElement.SelectNodes("player"))
            {
                if (ele.SelectSingleNode("steam64").InnerText == player.steam64.ToString())
                {
                    foreach (XmlElement tmp in ele.SelectSingleNode("groups").SelectNodes("group"))
                    {
                        
                    }

                    foreach (XmlElement tmp in ele.SelectSingleNode("permissions").SelectNodes("permission"))
                    {
                        player.permissions.Add(tmp.InnerText);
                    }

                    foreach (XmlElement tmp in ele.SelectSingleNode("customVariables").SelectNodes("variable"))
                    {
                        string key = tmp.SelectSingleNode("key").InnerText;
                        player.customVariables.Add(key, tmp.SelectSingleNode("value").InnerText);
                        player.saveKeys.Add(key);
                    }
                }
            }
        }

        public void savePlayer(PBPlayer player)
        {

        }
        #endregion

        #region Functions - Groups
        #endregion
    }
}
