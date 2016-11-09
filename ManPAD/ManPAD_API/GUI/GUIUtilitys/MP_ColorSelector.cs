using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_API.GUI.GUIUtilitys
{
    public class MP_ColorSelector
    {
        #region Variables
        public Color selectedColor;
        #endregion

        public MP_ColorSelector(ref Color selectedColor)
        {
            this.selectedColor = selectedColor;
        }

        #region Functions
        public void draw()
        {
        }
        #endregion
    }
}
