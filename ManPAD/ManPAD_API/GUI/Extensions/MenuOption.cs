using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API.GUI.Attributes;

namespace ManPAD.ManPAD_API.GUI.Extensions
{
    public abstract class MenuOption : MonoBehaviour
    {
        #region Variables
        private MenuOptionAttribute _attrib;
        #endregion

        #region Properties
        public MenuOptionAttribute attrib
        {
            get
            {
                if (_attrib == null)
                    _attrib = (MenuOptionAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(MenuOptionAttribute));
                return _attrib;
            }
            set
            {
                if (_attrib == null)
                    _attrib = value;
            }
        }

        public bool open { get; set; }

        public Vector2 scrollPos { get; set; }
        #endregion

        #region Abstract Functions
        public abstract void runGUI();
        #endregion
    }
}
