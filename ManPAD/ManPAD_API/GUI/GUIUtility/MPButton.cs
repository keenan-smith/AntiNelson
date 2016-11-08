using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API.GUI.Extensions;

namespace ManPAD.ManPAD_API.GUI.GUIUtility
{
    public class MPButton : MPGUIElement
    {
        #region Variables
        private bool _toggleable;
        private bool _toggled;
        private string _text;
        #endregion

        #region Properties
        public bool toggled
        {
            get
            {
                return (_toggleable != null ? _toggled : false);
            }
        }

        public string text
        {
            get
            {
                return _text;
            }
        }
        #endregion

        public MPButton(string text)
        {
            this._toggleable = false;
            this._text = text;
        }

        public MPButton(string text, bool toggled)
        {
            this._text = text;
            this._toggleable = true;
            this._toggled = toggled;
        }

        #region Functions
        public override void draw()
        {
            if (toggled)
            {

            }
            else
            {

            }
            GUILayout.Button(text);
        }

        public override void OnHover()
        {
            if (toggled)
            {

            }
            else
            {

            }
            GUILayout.Button(text);
        }
        #endregion
    }
}
