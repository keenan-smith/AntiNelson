using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Enumerables;

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

                try
                {

                    _doc.Load(path);

                }
                catch (Exception e)
                {

                    PBLogging.logError("Failed to load xml file: ", e);

                }

                _root = _doc.DocumentElement;

            }
            else
            {

                _doc = new XmlDocument();
                _doc.Save(path);
                _root = _doc.DocumentElement;

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
                        PBGroup group = Array.Find(PBServer.groups.ToArray(), a => a.name == tmp.InnerText);
                        if (group != null)
                        {
                            player.groups.Add(group);
                        }
                    }

                    foreach (XmlElement tmp in ele.SelectSingleNode("permissions").SelectNodes("permission"))
                    {
                        player.permissions.Add(tmp.InnerText);
                    }

                    foreach (XmlElement tmp in ele.SelectSingleNode("customVariables").SelectNodes("variable"))
                    {

                        string key = tmp.SelectSingleNode("key").InnerText;
                        ECustomVariableType type = (ECustomVariableType)Enum.Parse(typeof(ECustomVariableType), tmp.SelectSingleNode("type").InnerText);

                        switch (type)
                        {

                            case ECustomVariableType.BOOLEAN:
                                player.setCustomVariable(key, bool.Parse(tmp.SelectSingleNode("value").InnerText));
                                break;
                            case ECustomVariableType.BYTE:
                                player.setCustomVariable(key, byte.Parse(tmp.SelectSingleNode("value").InnerText));
                                break;
                            case ECustomVariableType.SHORT:
                                player.setCustomVariable(key, short.Parse(tmp.SelectSingleNode("value").InnerText));
                                break;
                            case ECustomVariableType.INT:
                                player.setCustomVariable(key, int.Parse(tmp.SelectSingleNode("value").InnerText));
                                break;
                            case ECustomVariableType.LONG:
                                player.setCustomVariable(key, long.Parse(tmp.SelectSingleNode("value").InnerText));
                                break;
                            case ECustomVariableType.DOUBLE:
                                player.setCustomVariable(key, double.Parse(tmp.SelectSingleNode("value").InnerText));
                                break;
                            case ECustomVariableType.FLOAT:
                                player.setCustomVariable(key, float.Parse(tmp.SelectSingleNode("value").InnerText));
                                break; ;
                            case ECustomVariableType.STRING:
                                player.setCustomVariable(key, tmp.SelectSingleNode("value").InnerText);
                                break;

                        }

                        player.saveKeys.Add(key);

                    }
                }
            }
        }

        public void savePlayer(PBPlayer player)
        {
            XmlElement ele = null;
            foreach (XmlElement tmp in rootElement.SelectNodes("player"))
            {

                if (tmp.SelectSingleNode("steam64").InnerText == player.steam64.ToString())
                {
                    ele = tmp;
                }

            }

            if (ele != null)
                ele = document.CreateElement("player");

        }
        #endregion

        #region Functions - Groups
        public void loadGroups()
        {
            foreach (XmlElement ele in rootElement.SelectNodes("group"))
            {
                string name = ele.SelectSingleNode("name").InnerText;
                bool isDefault = (ele.SelectSingleNode("default").InnerText == "true");
                List<string> perms = new List<string>();

                foreach (XmlElement tmp in ele.SelectSingleNode("permissions").SelectNodes("permission"))
                {
                    perms.Add(tmp.InnerText);
                }

                PBServer.groups.Add(new PBGroup(name, isDefault, perms.ToArray()));
            }
        }
        #endregion
    }
}
