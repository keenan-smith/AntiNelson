﻿using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API.GUI.Enumerables;

namespace ManPAD.ManPAD_API
{
    public class MP_Config
    {
        #region Variables
        private static MP_Config _instance;

        private XmlDocument _doc;
        private XmlElement _root;
        private string _path;
        #endregion

        #region Properties
        public static MP_Config instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        public MP_Config(string path)
        {
            _instance = this;

            this._path = path;
            this._doc = new XmlDocument();

            if (File.Exists(path))
                loadConfig(path);
            else
                createConfig(path);
        }

        #region Private Functions
        private void loadConfig(string path)
        {
            _doc.Load(path);
            _root = _doc.DocumentElement;
        }

        private void createConfig(string path)
        {
            _doc.AppendChild(_doc.CreateXmlDeclaration("1.0", null, null));
            _root = _doc.CreateElement("ManPAD");
            _doc.AppendChild(_root);
            setupConfig();
            save();
        }

        private void setupConfig()
        {
            XmlElement theme = _doc.CreateElement("Theme");
            theme.InnerText = EThemes.WHITE.ToString();
            _root.AppendChild(theme);

            XmlElement overlay = _doc.CreateElement("Overlay");
            overlay.InnerText = "false";
            _root.AppendChild(overlay);

            XmlElement keybinds = _doc.CreateElement("Keybinds");
            _root.AppendChild(keybinds);

            XmlElement keybind_menu = _doc.CreateElement("MainMenu");
            keybind_menu.InnerText = KeyCode.F1.ToString();
            keybinds.AppendChild(keybind_menu);

            XmlElement itemTypes = _doc.CreateElement("ItemTypes");
            _root.AppendChild(itemTypes);

            XmlElement itemTypes_ESP = _doc.CreateElement("ESP");
            itemTypes.AppendChild(itemTypes_ESP);

            XmlElement ESPColors = _doc.CreateElement("ESPColors");
            _root.AppendChild(ESPColors);

            XmlElement ESPColors_Players = _doc.CreateElement("Players");
            Color tmp = Color.red;
            ESPColors_Players.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Players);

            XmlElement ESPColors_Friends = _doc.CreateElement("Friends");
            tmp = new Color(1f, 0.41f, 0.7f);
            ESPColors_Friends.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Friends);

            XmlElement ESPColors_Zombies = _doc.CreateElement("Zombies");
            tmp = Color.green;
            ESPColors_Zombies.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Zombies);

            XmlElement ESPColors_Animals = _doc.CreateElement("Animals");
            tmp = new Color(0.63f, 0.32f, 0.18f);
            ESPColors_Animals.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Animals);

            XmlElement ESPColors_Items = _doc.CreateElement("Items");
            tmp = Color.blue;
            ESPColors_Items.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Items);

            XmlElement ESPColors_Vehicles = _doc.CreateElement("Vehicles");
            tmp = Color.yellow;
            ESPColors_Vehicles.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Vehicles);

            XmlElement ESPColors_Storages = _doc.CreateElement("Storages");
            tmp = Color.cyan;
            ESPColors_Storages.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Storages);

            XmlElement ESPColors_Sentrys = _doc.CreateElement("Sentrys");
            tmp = new Color(1f, 0.65f, 0f);
            ESPColors_Sentrys.InnerText = tmp.r + "," + tmp.g + "," + tmp.b;
            ESPColors.AppendChild(ESPColors_Sentrys);
        }
        #endregion

        #region Public Functions
        public XmlNode getNode(string nodePath)
        {
            return _root.SelectSingleNode(nodePath);
        }
        
        public string getText(string nodePath)
        {
            return getNode(nodePath).InnerText;
        }

        public XmlNodeList getChildNodes(string nodePath)
        {
            return _root.SelectSingleNode(nodePath).ChildNodes;
        }

        public string[] getChildNodesText(string nodePath)
        {
            List<string> texts = new List<string>();

            foreach (XmlNode node in getChildNodes(nodePath))
                texts.Add(node.InnerText);
            return texts.ToArray();
        }

        public void save()
        {
            _doc.Save(_path);
        }
        #endregion

        #region Get Functions
        public EThemes getTheme()
        {
            return (EThemes)Enum.Parse(typeof(EThemes), _root.SelectSingleNode("Theme").InnerText);
        }

        public bool getOverlay()
        {
            return _root.SelectSingleNode("Overlay").InnerText.ToLower() == "true";
        }

        public KeyCode getKeybind(string bindName)
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), _root.SelectSingleNode("Keybinds/" + bindName).InnerText);
        }

        public Color getESPColor(string ESPName)
        {
            string[] colorCodes = _root.SelectSingleNode("ESPColors/" + ESPName).InnerText.Split(',');

            return new Color(float.Parse(colorCodes[0]), float.Parse(colorCodes[1]), float.Parse(colorCodes[2]));
        }

        public EUseableType[] getItemTypes(string itemTypeName)
        {
            if (string.IsNullOrEmpty(_root.SelectSingleNode("ItemTypes/" + itemTypeName).InnerText))
                return new EUseableType[0];
            List<EUseableType> itemTypes = new List<EUseableType>();

            foreach (string s in _root.SelectSingleNode("ItemTypes/" + itemTypeName).InnerText.Split(','))
                itemTypes.Add((EUseableType)Enum.Parse(typeof(EUseableType), s));

            return itemTypes.ToArray();
        }
        #endregion

        #region Set Functions
        public void setTheme(EThemes theme)
        {
            _root.SelectSingleNode("Theme").InnerText = theme.ToString();
            save();
        }

        public void setOverlay(bool overlay)
        {
            _root.SelectSingleNode("Overlay").InnerText = overlay.ToString().ToLower();
            save();
        }

        public void setKeybind(string bindName, KeyCode bind)
        {
            _root.SelectSingleNode("Keybinds/" + bindName).InnerText = bind.ToString();
            save();
        }

        public void setESPColor(string ESPName, Color color)
        {
            _root.SelectSingleNode("ESPColors/" + ESPName).InnerText = color.r + "," + color.g + "," + color.b;
            save();
        }

        public void setItemType(string itemTypeName, EUseableType[] itemTypes)
        {
            _root.SelectSingleNode("ItemTypes/" + itemTypeName).InnerText = string.Join(",", itemTypes.Select(a => a.ToString()) as string[]);
            save();
        }
        #endregion
    }
}
