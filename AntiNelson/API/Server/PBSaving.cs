using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API.Enumerables;
using PointBlank.API.Extensions;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Enumerables;
using PointBlank.API.Server.Types;
using Steamworks;

namespace PointBlank.API.Server
{
    internal class PBSaving
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

        public PBSaving(string path, ESaveType saveType)
        {
            _path = path;
            _doc = new XmlDocument();

            if (ReadWrite.fileExists(path, false, false))
            {
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
                document.AppendChild(document.CreateXmlDeclaration("1.0", null, null));
                _root = document.CreateElement("PointBlank");
                document.AppendChild(rootElement);
                if (saveType == ESaveType.GROUP)
                    firstRunGroup();
                else if (saveType == ESaveType.STEAMGROUP)
                    firstRunSteamGroup();
                document.Save(path);
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
                    XmlNode tmpC = ele.SelectSingleNode("color");
                    player.playerColor = new Color(float.Parse(tmpC.SelectSingleNode("red").InnerText), float.Parse(tmpC.SelectSingleNode("green").InnerText), float.Parse(tmpC.SelectSingleNode("blue").InnerText), float.Parse(tmpC.SelectSingleNode("alpha").InnerText));

                    foreach (XmlElement tmp in ele.SelectSingleNode("groups").SelectNodes("group"))
                    {
                        PBGroup group = Array.Find(PBServer.groups.ToArray(), a => a.name == tmp.InnerText);
                        if (group != null)
                        {
                            player.groups.Add(group);
                        }
                    }
                    if (player.groups.Count < 1)
                    {
                        foreach (PBGroup group in PBServer.groups)
                        {
                            if (group.isDefault)
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
                                player.setCustomVariable(key, bool.Parse(tmp.SelectSingleNode("value").InnerText), true);
                                break;
                            case ECustomVariableType.BYTE:
                                player.setCustomVariable(key, byte.Parse(tmp.SelectSingleNode("value").InnerText), true);
                                break;
                            case ECustomVariableType.SHORT:
                                player.setCustomVariable(key, short.Parse(tmp.SelectSingleNode("value").InnerText), true);
                                break;
                            case ECustomVariableType.INT:
                                player.setCustomVariable(key, int.Parse(tmp.SelectSingleNode("value").InnerText), true);
                                break;
                            case ECustomVariableType.LONG:
                                player.setCustomVariable(key, long.Parse(tmp.SelectSingleNode("value").InnerText), true);
                                break;
                            case ECustomVariableType.DOUBLE:
                                player.setCustomVariable(key, double.Parse(tmp.SelectSingleNode("value").InnerText), true);
                                break;
                            case ECustomVariableType.FLOAT:
                                player.setCustomVariable(key, float.Parse(tmp.SelectSingleNode("value").InnerText), true);
                                break; ;
                            case ECustomVariableType.STRING:
                                player.setCustomVariable(key, tmp.SelectSingleNode("value").InnerText, true);
                                break;
                        }
                    }
                }
            }
        }

        public void savePlayer(PBPlayer player)
        {
            XmlElement ele = null;
            XmlNode tmp_steam64 = null;
            XmlNode tmp_color = null;
            XmlNode tmp_groups = null;
            XmlNode tmp_permissions = null;
            XmlNode tmp_customVariables = null;

            foreach (XmlElement tmp in rootElement.SelectNodes("player"))
            {
                if (tmp.SelectSingleNode("steam64").InnerText == player.steam64.ToString())
                    ele = tmp;
            }

            if (ele == null)
            {
                ele = document.CreateElement("player");
                tmp_steam64 = document.CreateElement("steam64");
                tmp_steam64.InnerText = player.steam64.ToString();
                tmp_color = document.CreateElement("color");
                tmp_groups = document.CreateElement("groups");
                tmp_permissions = document.CreateElement("permissions");
                tmp_customVariables = document.CreateElement("customVariables");

                ele.AppendChild(tmp_steam64);
                ele.AppendChild(tmp_color);
                ele.AppendChild(tmp_groups);
                ele.AppendChild(tmp_permissions);
                ele.AppendChild(tmp_customVariables);
                rootElement.AppendChild(ele);
            }
            else
            {
                tmp_steam64 = ele.SelectSingleNode("steam64");
                tmp_color = ele.SelectSingleNode("color");
                tmp_color.InnerXml = "";
                tmp_groups = ele.SelectSingleNode("groups");
                tmp_groups.InnerXml = "";
                tmp_permissions = ele.SelectSingleNode("permissions");
                tmp_permissions.InnerXml = "";
                tmp_customVariables = ele.SelectSingleNode("customVariables");
                tmp_customVariables.InnerXml = "";
            }

            XmlElement tmpCRed = document.CreateElement("red");
            XmlElement tmpCGreen = document.CreateElement("green");
            XmlElement tmpCBlue = document.CreateElement("blue");
            XmlElement tmpCAlpha = document.CreateElement("alpha");
            Color tmpWhite = Color.white;

            tmpCRed.InnerText = tmpWhite.r.ToString();
            tmpCGreen.InnerText = tmpWhite.g.ToString();
            tmpCBlue.InnerText = tmpWhite.b.ToString();
            tmpCAlpha.InnerText = tmpWhite.a.ToString();

            tmp_color.AppendChild(tmpCRed);
            tmp_color.AppendChild(tmpCGreen);
            tmp_color.AppendChild(tmpCBlue);
            tmp_color.AppendChild(tmpCAlpha);
            foreach (PBGroup group in player.groups)
            {
                XmlElement tmp = document.CreateElement("group");
                tmp.InnerText = group.name;
                tmp_groups.AppendChild(tmp);
            }
            foreach (string perm in player.permissions)
            {
                XmlElement tmp = document.CreateElement("permission");
                tmp.InnerText = perm;
                tmp_permissions.AppendChild(tmp);
            }
            foreach (KeyValuePair<string, CustomVariable> customVar in player.customVariables)
            {
                if (!Array.Exists(player.saveKeys.ToArray(), a => a == customVar.Key))
                    continue;
                XmlElement tmp = document.CreateElement("variable");
                XmlElement tmpKey = document.CreateElement("key");
                XmlElement tmpSync = document.CreateElement("sync");
                XmlElement tmpType = document.CreateElement("type");
                XmlElement tmpValue = document.CreateElement("value");

                tmpKey.InnerText = customVar.Key;
                tmpType.InnerText = customVar.Value.getValueType().ToString();
                tmpValue.InnerText = customVar.Value.getValue().ToString();

                tmp.AppendChild(tmpKey);
                tmp.AppendChild(tmpSync);
                tmp.AppendChild(tmpType);
                tmp.AppendChild(tmpValue);
                tmp_customVariables.AppendChild(tmp);
            }

            save();
        }
        #endregion

        #region Functions - Groups
        public void firstRunGroup()
        {
            XmlElement ele = document.CreateElement("group");
            XmlElement name = document.CreateElement("name");
            XmlElement isDefault = document.CreateElement("default");
            XmlElement permissions = document.CreateElement("permissions");

            name.InnerText = "Guest";
            isDefault.InnerText = "true";

            ele.AppendChild(name);
            ele.AppendChild(isDefault);
            ele.AppendChild(permissions);
            rootElement.AppendChild(ele);
        }

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

        #region Functions - Steam Groups
        public void firstRunSteamGroup()
        {
            XmlElement ele = document.CreateElement("steamGroup");
            XmlElement name = document.CreateElement("name");
            XmlElement steam64 = document.CreateElement("steam64");
            XmlElement permissions = document.CreateElement("permissions");

            name.InnerText = "Unturned Fan Club";
            steam64.InnerText = "103582791435804732";

            ele.AppendChild(name);
            ele.AppendChild(steam64);
            ele.AppendChild(permissions);
            rootElement.AppendChild(ele);
        }

        public void loadSteamGroups()
        {
            foreach (XmlElement ele in rootElement.SelectNodes("steamGroup"))
            {
                string name = ele.SelectSingleNode("name").InnerText;
                ulong steam64 = ulong.Parse(ele.SelectSingleNode("steam64").InnerText);
                List<string> perms = new List<string>();

                foreach (XmlElement tmp in ele.SelectSingleNode("permissions").SelectNodes("permission"))
                {
                    perms.Add(tmp.InnerText);
                }

                PBServer.steamGroups.Add(new PBSteamGroup(name, steam64, perms.ToArray()));
            }
        }
        #endregion
    }
}
