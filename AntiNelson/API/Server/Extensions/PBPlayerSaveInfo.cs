using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API.Server.Enumerables;
using PointBlank.API.Server.Types;

namespace PointBlank.API.Server.Extensions
{
    public class PBPlayerSaveInfo
    {
        #region Variables
        private ulong _steam64;
        private List<PBGroup> _groups = new List<PBGroup>();
        private List<string> _permissions = new List<string>();
        private Color _color;
        private Dictionary<string, CustomVariable> _customVariables = new Dictionary<string, CustomVariable>();
        #endregion

        #region Properties
        public ulong steam64
        {
            get
            {
                return _steam64;
            }
        }

        public PBGroup[] groups
        {
            get
            {
                return _groups.ToArray();
            }
        }

        public string[] permissions
        {
            get
            {
                return _permissions.ToArray();
            }
        }

        public Color color
        {
            get
            {
                return _color;
            }
        }
        #endregion

        /// <summary>
        /// The save information of the player. Useful for reading if the player is offline.
        /// </summary>
        /// <param name="ele">The xml element of the player.</param>
        public PBPlayerSaveInfo(XmlElement ele)
        {
            XmlNode tmpC = ele.SelectSingleNode("color");
            _color = new Color(float.Parse(tmpC.SelectSingleNode("red").InnerText), float.Parse(tmpC.SelectSingleNode("green").InnerText), float.Parse(tmpC.SelectSingleNode("blue").InnerText), float.Parse(tmpC.SelectSingleNode("alpha").InnerText));

            foreach (XmlElement tmp in ele.SelectSingleNode("groups").SelectNodes("group"))
            {
                PBGroup group = Array.Find(PBServer.groups.ToArray(), a => a.name == tmp.InnerText);
                if (group != null)
                {
                    _groups.Add(group);
                }
            }
            if (_groups.Count < 1)
            {
                foreach (PBGroup group in PBServer.groups)
                {
                    if (group.isDefault)
                        _groups.Add(group);
                }
            }

            foreach (XmlElement tmp in ele.SelectSingleNode("permissions").SelectNodes("permission"))
            {
                _permissions.Add(tmp.InnerText);
            }

            foreach (XmlElement tmp in ele.SelectSingleNode("_customVariables").SelectNodes("variable"))
            {
                string key = tmp.SelectSingleNode("key").InnerText;
                ECustomVariableType type = (ECustomVariableType)Enum.Parse(typeof(ECustomVariableType), tmp.SelectSingleNode("type").InnerText);

                switch (type)
                {
                    case ECustomVariableType.BOOLEAN:
                        setCustomVariable(key, bool.Parse(tmp.SelectSingleNode("value").InnerText));
                        break;
                    case ECustomVariableType.BYTE:
                        setCustomVariable(key, byte.Parse(tmp.SelectSingleNode("value").InnerText));
                        break;
                    case ECustomVariableType.SHORT:
                        setCustomVariable(key, short.Parse(tmp.SelectSingleNode("value").InnerText));
                        break;
                    case ECustomVariableType.INT:
                        setCustomVariable(key, int.Parse(tmp.SelectSingleNode("value").InnerText));
                        break;
                    case ECustomVariableType.LONG:
                        setCustomVariable(key, long.Parse(tmp.SelectSingleNode("value").InnerText));
                        break;
                    case ECustomVariableType.DOUBLE:
                        setCustomVariable(key, double.Parse(tmp.SelectSingleNode("value").InnerText));
                        break;
                    case ECustomVariableType.FLOAT:
                        setCustomVariable(key, float.Parse(tmp.SelectSingleNode("value").InnerText));
                        break; ;
                    case ECustomVariableType.STRING:
                        setCustomVariable(key, tmp.SelectSingleNode("value").InnerText);
                        break;
                }
            }
        }

        #region Functions
        private void setCustomVariable(string key, bool value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        private void setCustomVariable(string key, byte value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        private void setCustomVariable(string key, short value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        private void setCustomVariable(string key, int value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        private void setCustomVariable(string key, long value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        private void setCustomVariable(string key, float value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        private void setCustomVariable(string key, double value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        private void setCustomVariable(string key, string value)
        {
            _customVariables.Remove(key);
            _customVariables.Add(key, new CustomVariable(value));
        }

        /// <summary>
        /// Returns the value of the custom variable specified.
        /// </summary>
        /// <param name="key">The key of the custom variable.</param>
        /// <returns>The custom variable value.</returns>
        public object getCustomVariable(string key)
        {
            if (!_customVariables.ContainsKey(key))
                return null;
            return _customVariables[key];
        }
        #endregion
    }
}
