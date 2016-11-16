using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using ManPAD.ManPAD_API.GUI.Enumerables;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(5, "Themes", 130f, 105f)]
    public class MP_Themes : MenuOption
    {
        #region Functions
        public override void runGUI()
        {
            if (GUILayout.Button("White"))
                MP_Config.instance.setTheme(EThemes.WHITE);
            if (GUILayout.Button("Inverted"))
                MP_Config.instance.setTheme(EThemes.INVERTED);
            if (GUILayout.Button("Aqua"))
                MP_Config.instance.setTheme(EThemes.AQUA);
            if (GUILayout.Button("Magic"))
                MP_Config.instance.setTheme(EThemes.MAGIC);
        }
        #endregion
    }
}
