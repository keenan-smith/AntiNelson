using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.GUIUtilitys;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.Types;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
#if DEBUG
    [MenuOption(12, "ESP(ic3)", 250f)]
    public class MP_ic3ESP : MenuOption
    {
        public static bool ESP_Enabled = false;
        public static bool PlayerESP = false;
        public static bool PlayerBox = false;
        public static bool PlayerLine = false;
        public static bool ItemESP = false;
        public static bool FilterItems = false;
        public static bool VehicleESP = false;
        public static bool FilterLockedVehicles = true;
        public static bool StorageESP = false;
        public static bool FilterLockedStorages = true;
        public static bool ClaimFlagESP = false;
        public static bool BedESP = false;
        public static bool GeneratorESP = false;
        public static bool GlowESP = false;
        public static bool CenteredLabels = false;
        public static bool InfiniteDistance = false;
        public static float Distance = 500f;

        public static bool isFriend(SteamPlayer player)
        {
            if (Characters.active.group != CSteamID.Nil && player.playerID.group == Characters.active.group)
                return true;

            return false;
        }
        public static bool isFriend(Player player)
        {
            if (Characters.active.group != CSteamID.Nil && player.channel.owner.playerID.group == Characters.active.group)
                return true;

            return false;
        }

        public override void runGUI()
        {
            GlowESP = GUILayout.Toggle(GlowESP, "Glow ESP");
            ESP_Enabled = GUILayout.Toggle(ESP_Enabled, "ESP Enabled");
            PlayerESP = GUILayout.Toggle(PlayerESP, "Player ESP");
            PlayerBox = GUILayout.Toggle(PlayerBox, "Player Box");
            PlayerLine = GUILayout.Toggle(PlayerLine, "Player Line");
            ItemESP = GUILayout.Toggle(ItemESP, "Item ESP");
            FilterItems = GUILayout.Toggle(FilterItems, "Filter Items");
            VehicleESP = GUILayout.Toggle(VehicleESP, "Vehicle ESP");
            FilterLockedVehicles = GUILayout.Toggle(FilterLockedVehicles, "Filter Locked Vehicles");
            StorageESP = GUILayout.Toggle(StorageESP, "Storage ESP");
            FilterLockedStorages = GUILayout.Toggle(FilterLockedStorages, "Filter Locked Storages");
            BedESP = GUILayout.Toggle(BedESP, "Bed ESP");
            GeneratorESP = GUILayout.Toggle(GeneratorESP, "Generator ESP");
            ClaimFlagESP = GUILayout.Toggle(ClaimFlagESP, "Claim Flag ESP");
            CenteredLabels = GUILayout.Toggle(CenteredLabels, "Centered Label");
            InfiniteDistance = GUILayout.Toggle(InfiniteDistance, "Infinite Distance");
            GUILayout.Label("Distance: " + Mathf.Round(Distance));
            Distance = GUILayout.HorizontalSlider(Mathf.Round(Distance), 50f, 1000f);
        }
    }
#endif
}
