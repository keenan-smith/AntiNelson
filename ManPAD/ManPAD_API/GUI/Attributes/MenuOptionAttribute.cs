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
        private float _UI_Height;
        private MenuOption _option;

        public float UI_X;
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

        public Rect button
        {
            get
            {
                return new Rect(UI_X, MP_MainMenu.menu_Start_Y, _UI_Width, MP_MainMenu.button_Height);
            }
        }

        public Rect window
        {
            get
            {
                return new Rect(UI_X, MP_MainMenu.menu_Start_Y + MP_MainMenu.button_Height + 2f, _UI_Width, (_UI_Height > MP_MainMenu.option_Max_Height ? MP_MainMenu.option_Max_Height : _UI_Height));
            }
        }

        public Rect trigger
        {
            get
            {
                return new Rect(UI_X, MP_MainMenu.menu_Start_Y, _UI_Width, (_option.open ? MP_MainMenu.button_Height + window.height + 2f : MP_MainMenu.button_Height));
            }
        }

        public bool isActive
        {
            get
            {
                return trigger.Contains(MP_MainMenu.mouse_position);
            }
        }
        #endregion

        public MenuOptionAttribute(int ID, string text, float UI_Width, float UI_Height)
        {
            this.ID = ID;
            this.text = text;
            this._UI_Width = UI_Width;
            this._UI_Height = UI_Height;
        }
    }
}
