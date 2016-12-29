using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(6, "Server", 200f)]
    public class MP_Server : MenuOption
    {
        #region Variables
        public static float salvageSpeed = 0.5f;
        public static bool instantDisconnect = false;
        #endregion

        #region Functions
        public override void runGUI()
        {
            instantDisconnect = GUILayout.Toggle(instantDisconnect, "Instant Disconnect");
#if !FREE
            GUILayout.Label("Salvage Speed: " + salvageSpeed);
            salvageSpeed = (float)Math.Round(GUILayout.HorizontalSlider(salvageSpeed, 0f, 1f), 1);
#endif
        }
        #endregion
    }
}
