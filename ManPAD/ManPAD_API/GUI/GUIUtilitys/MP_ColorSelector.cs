﻿using System;
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

        public MP_ColorSelector(Color selectedColor)
        {
            this.selectedColor = selectedColor;
        }

        #region Functions
        public void draw()
        {
            GUILayout.Label("Red: " + (selectedColor.r * 100f));
            selectedColor.r = (float)Math.Round(GUILayout.HorizontalSlider(selectedColor.r, 0f, 1f), 3);
            GUILayout.Label("Green: " + (selectedColor.g * 100f));
            selectedColor.g = (float)Math.Round(GUILayout.HorizontalSlider(selectedColor.g, 0f, 1f), 3);
            GUILayout.Label("Blue: " + (selectedColor.b * 100f));
            selectedColor.b = (float)Math.Round(GUILayout.HorizontalSlider(selectedColor.b, 0f, 1f), 3);
        }
        #endregion
    }
}
