using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(10, "Stats", 300f)]
    public class MP_Stats : MenuOption
    {
        #region Variables
        private float amount = 10000f;
        #endregion

        #region Functions
        public override void runGUI()
        {
            GUILayout.Label("Amount: " + Mathf.Round(amount));
            amount = GUILayout.HorizontalSlider(amount, 1f, 100000f);

            if (GUILayout.Button("Accuracy"))
                Tools.IncStat(EPlayerStat.ACCURACY, amount);
            if (GUILayout.Button("Arena Wins"))
                Tools.IncStat(EPlayerStat.ARENA_WINS, amount);
            if (GUILayout.Button("Buildables"))
                Tools.IncStat(EPlayerStat.FOUND_BUILDABLES, amount);
            if (GUILayout.Button("Crafts"))
                Tools.IncStat(EPlayerStat.FOUND_CRAFTS, amount);
            if (GUILayout.Button("Experience"))
                Tools.IncStat(EPlayerStat.FOUND_EXPERIENCE, amount);
            if (GUILayout.Button("Fish"))
                Tools.IncStat(EPlayerStat.FOUND_FISHES, amount);
            if (GUILayout.Button("Items"))
                Tools.IncStat(EPlayerStat.FOUND_ITEMS, amount);
            if (GUILayout.Button("Plants"))
                Tools.IncStat(EPlayerStat.FOUND_PLANTS, amount);
            if (GUILayout.Button("Resources"))
                Tools.IncStat(EPlayerStat.FOUND_RESOURCES, amount);
            if (GUILayout.Button("Throwables"))
                Tools.IncStat(EPlayerStat.FOUND_THROWABLES, amount);
            if (GUILayout.Button("Headshots"))
                Tools.IncStat(EPlayerStat.HEADSHOTS, amount);
            if (GUILayout.Button("Animals"))
                Tools.IncStat(EPlayerStat.KILLS_ANIMALS, amount);
            if (GUILayout.Button("Players"))
                Tools.IncStat(EPlayerStat.KILLS_PLAYERS, amount);
            if (GUILayout.Button("Mega zombies"))
                Tools.IncStat(EPlayerStat.KILLS_ZOMBIES_MEGA, amount);
            if (GUILayout.Button("Zombies"))
                Tools.IncStat(EPlayerStat.KILLS_ZOMBIES_NORMAL, amount);
            if (GUILayout.Button("Foot"))
                Tools.IncStat(EPlayerStat.TRAVEL_FOOT, amount);
            if (GUILayout.Button("Vehicle"))
                Tools.IncStat(EPlayerStat.TRAVEL_VEHICLE, amount);
        }
        #endregion
    }
}
