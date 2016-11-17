using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(1, "Aimbot", 130f, 130f)]
    public class MP_Aimbot : MenuOption
    {
        #region Functions
        public override void runGUI()
        {
            Variables.silentAim = GUILayout.Toggle(Variables.silentAim, "Silent Aim");
            Variables.fovBased = GUILayout.Toggle(Variables.fovBased, "FOV Based");
            GUILayout.Label("Aim FOV: " + Variables.aimFov);
            Variables.aimFov = GUILayout.HorizontalSlider(Variables.aimFov, 1, 45);
        }
        #endregion
    }
}
