using System;
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
        private void fixConfig()
        {
            MP_Logging.Log("Integrity broken! Reloading....");
            File.Delete(_path);
            createConfig(_path);
            MP_Logging.Log("Config reloaded!");
        }

        private void checkConfig()
        {
            try
            {
                if (_root.SelectSingleNode("Friends") == null)
                    fixConfig();
                if (_root.SelectSingleNode("ItemTypes") == null)
                    fixConfig();
                if (_root.SelectSingleNode("ESPColors") == null)
                    fixConfig();
                if (_root.SelectSingleNode("Keybinds") == null)
                    fixConfig();
                if (_root.SelectSingleNode("Overlay") == null)
                    fixConfig();
                if (_root.SelectSingleNode("Theme") == null)
                    fixConfig();
            }
            catch (Exception ex)
            {
                fixConfig();
            }
        }

        private void loadConfig(string path)
        {
            _doc.Load(path);
            _root = _doc.DocumentElement;
            checkConfig();
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

            XmlElement friends = _doc.CreateElement("Friends");
            _root.AppendChild(friends);

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

            for (int i = 0; i < getChildNodes(nodePath).Count; i++)
            {
                XmlNode node = getChildNodes(nodePath)[i];

                texts.Add(node.InnerText);
            }
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

        public EItemType[] getItemTypes(string itemTypeName)
        {
            if (string.IsNullOrEmpty(_root.SelectSingleNode("ItemTypes/" + itemTypeName).InnerText))
                return new EItemType[0];
            List<EItemType> itemTypes = new List<EItemType>();

            for (int i = 0; i < _root.SelectSingleNode("ItemTypes/" + itemTypeName).InnerText.Split(',').Length; i++)
            {
                string s = _root.SelectSingleNode("ItemTypes/" + itemTypeName).InnerText.Split(',')[i];
                itemTypes.Add((EItemType)Enum.Parse(typeof(EItemType), s));
            }

            return itemTypes.ToArray();
        }

        public ulong[] getFriends()
        {
            if (string.IsNullOrEmpty(_root.SelectSingleNode("Friends").InnerText))
                return new ulong[0];

            List<ulong> friends = new List<ulong>();

            for(int i = 0; i < _root.SelectSingleNode("Friends").InnerText.Split(',').Length; i++)
            {
                string s = _root.SelectSingleNode("Friends").InnerText.Split(',')[i];
                friends.Add(ulong.Parse(s));
            }

            return friends.ToArray();
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

        public void setItemType(string itemTypeName, EItemType[] itemTypes)
        {
            _root.SelectSingleNode("ItemTypes/" + itemTypeName).InnerText = string.Join(",", itemTypes.Select(a => a.ToString()) as string[]);
            save();
        }

        public void setFriends(ulong[] friends)
        {
            _root.SelectSingleNode("Friends").InnerText = string.Join(",", friends.Select(a => a.ToString()) as string[]);
            save();
        }
        #endregion
    }
}
