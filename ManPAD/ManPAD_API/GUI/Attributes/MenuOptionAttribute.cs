using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API.GUI;
using ManPAD.ManPAD_API.GUI.Extensions;

namespace ManPAD.ManPAD_API.GUI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MenuOptionAttribute : Attribute
    {
        #region Variables
        private float _UI_Width;
        private MenuOption _option;
        #endregion

        #region Properties
        public string text { get; set; }

        public int ID { get; set; }

        public MenuOption option
        {
            get
            {
                return _option;
            }
            set
            {
                if (_option == null)
                    _option = value;
            }
        }

        public bool isActive
        {
            get
            {
                return option.open;
            }
            set
            {
                option.open = value;
            }
        }

        public Rect window
        {
            get
            {
                return new Rect(MP_MainMenu.mainMenu.x + MP_MainMenu.mainMenu.width + 3f, 1f, (_UI_Width > MP_MainMenu.option_Max_Width ? MP_MainMenu.option_Max_Width : _UI_Width), Screen.height - 2f);
            }
        }
        #endregion

        public MenuOptionAttribute(int ID, string text, float UI_Width)
        {
            this.ID = ID;
            this.text = text;
            this._UI_Width = UI_Width;
        }
    }
}
