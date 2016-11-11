using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.GUIUtilitys;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using ManPAD.ManPAD_API.Types;
using SDG.Unturned;
using UnityEngine;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(1, "ESP", 200f, 500f)]
    public class MP_ESP : MenuOption
    {
        #region Variables
        private List<ESPDraw> _draw = new List<ESPDraw>();

        public bool ESP_Enabled = false;
        public bool ESP_Chams = true;
        public bool ESP_Box = false;
        public bool ESP_ShowNames = true;
        public bool ESP_ShowDistances = true;

        public bool ESP_Players_Enabled = true;
        public bool ESP_Players_ShowWeapons = true;
        public bool ESP_Players_FilterFriends = false;
        public bool ESP_Players_ShowIsAdmin = false;
        public MP_ColorSelector ESP_Players_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Players"));
        public MP_ColorSelector ESP_Friends_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Friends"));

        public bool ESP_Zombies_Enabled = false;
        public MP_ColorSelector ESP_Zombies_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Zombies"));

        public bool ESP_Animals_Enabled = false;
        public MP_ColorSelector ESP_Animals_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Animals"));

        public bool ESP_Items_Enabled = false;
        public MP_ColorSelector ESP_Items_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Items"));
        public MP_ItemPicker ESP_Items_Types = new MP_ItemPicker(MP_Config.instance.getItemTypes("ESP"));

        public bool ESP_Vehicles_Enabled = false;
        public bool ESP_Vehicles_IgnoreDestroyed = true;
        public bool ESP_Vehicles_IgnoreEmpty = true;
        public bool ESP_Vehicles_IgnoreLocked = true;
        public bool ESP_Vehicles_ShowFuel = true;
        public bool ESP_Vehicles_ShowLocked = true;
        public MP_ColorSelector ESP_Vehicles_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Vehicles"));

        public bool ESP_Storages_Enabled = true;
        public bool ESP_Storages_IgnoreLocked = true;
        public bool ESP_Storages_ShowLocked = true;
        public MP_ColorSelector ESP_Storages_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Storages"));

        public bool ESP_Sentrys_Enabled = false;
        public bool ESP_Sentrys_ShowFriendly = true;
        public bool ESP_Sentrys_ShowBroken = true;
        public bool ESP_Sentrys_IgnoreFriendly = true;
        public bool ESP_Sentrys_IgnoreBroken = true;
        public MP_ColorSelector ESP_Sentrys_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Sentrys"));
        #endregion

        #region Mono Functions
        public void Update()
        {
            if (!Variables.isInGame)
                return;
        }

        public void OnGUI()
        {
            if (!Variables.isInGame)
                return;
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            ESP_Enabled = GUILayout.Toggle(ESP_Enabled, "Enabled");
            ESP_Chams = GUILayout.Toggle(ESP_Chams, "Chams");
            ESP_Box = GUILayout.Toggle(ESP_Box, "Box");
            ESP_ShowNames = GUILayout.Toggle(ESP_ShowNames, "Show Names");
            ESP_ShowDistances = GUILayout.Toggle(ESP_ShowDistances, "Show Distances");

            GUILayout.Label(""); // you can just do GUILayout.Space(12f)(pixels) but ok :u
            ESP_Players_Enabled = GUILayout.Toggle(ESP_Players_Enabled, "Player ESP");
            ESP_Players_ShowWeapons = GUILayout.Toggle(ESP_Players_ShowWeapons, "Show Weapons");
            ESP_Players_ShowIsAdmin = GUILayout.Toggle(ESP_Players_ShowIsAdmin, "Show Admin");
            ESP_Players_FilterFriends = GUILayout.Toggle(ESP_Players_FilterFriends, "Filter Friends");
            ESP_Players_Color.draw("Player ESP Color:", "Players");
            ESP_Friends_Color.draw("Friend ESP Color:", "Friends");

            GUILayout.Label("");
            ESP_Zombies_Enabled = GUILayout.Toggle(ESP_Zombies_Enabled, "Zombie ESP");
            ESP_Zombies_Color.draw("Zombie ESP Color:", "Zombies");

            GUILayout.Label("");
            ESP_Animals_Enabled = GUILayout.Toggle(ESP_Animals_Enabled, "Animal ESP");
            ESP_Animals_Color.draw("Animal ESP Color:", "Animals");

            GUILayout.Label("");
            ESP_Items_Enabled = GUILayout.Toggle(ESP_Items_Enabled, "Item ESP");
            ESP_Items_Color.draw("Item ESP Color:", "Items");
            ESP_Items_Types.draw("Item ESP Types:", "ESP");

            GUILayout.Label("");
            ESP_Vehicles_Enabled = GUILayout.Toggle(ESP_Vehicles_Enabled, "Vehicle ESP");
            ESP_Vehicles_ShowFuel = GUILayout.Toggle(ESP_Vehicles_ShowFuel, "Show Fuel");
            ESP_Vehicles_ShowLocked = GUILayout.Toggle(ESP_Vehicles_ShowLocked, "Show Locked");
            ESP_Vehicles_IgnoreDestroyed = GUILayout.Toggle(ESP_Vehicles_IgnoreDestroyed, "Filter Destoryed");
            ESP_Vehicles_IgnoreEmpty = GUILayout.Toggle(ESP_Vehicles_IgnoreEmpty, "Filter Empty");
            ESP_Vehicles_IgnoreLocked = GUILayout.Toggle(ESP_Vehicles_IgnoreLocked, "Filter Locked");
            ESP_Vehicles_Color.draw("Vehicle ESP Color:", "Vehicles");

            GUILayout.Label("");
            ESP_Storages_Enabled = GUILayout.Toggle(ESP_Storages_Enabled, "Storage ESP");
            ESP_Storages_ShowLocked = GUILayout.Toggle(ESP_Storages_ShowLocked, "Show Locked");
            ESP_Storages_IgnoreLocked = GUILayout.Toggle(ESP_Storages_IgnoreLocked, "Filter Locked");
            ESP_Storages_Color.draw("Storage ESP Color:", "Storages");

            GUILayout.Label("");
            ESP_Sentrys_Enabled = GUILayout.Toggle(ESP_Sentrys_Enabled, "Sentry ESP");
            ESP_Sentrys_ShowFriendly = GUILayout.Toggle(ESP_Sentrys_ShowFriendly, "Show Friendly");
            ESP_Sentrys_ShowBroken = GUILayout.Toggle(ESP_Sentrys_ShowBroken, "Show Broken");
            ESP_Sentrys_IgnoreFriendly = GUILayout.Toggle(ESP_Sentrys_IgnoreFriendly, "Filter Friendly");
            ESP_Sentrys_IgnoreBroken = GUILayout.Toggle(ESP_Sentrys_IgnoreBroken, "Filter Broken");
            ESP_Sentrys_Color.draw("Sentry ESP Color:", "Sentrys");
        }

        private string getZombieName(Zombie z)
        {
            string str = "";
            switch (z.speciality)
            {
                case EZombieSpeciality.ACID:
                    str = "Acid Zombie";
                    break;
                case EZombieSpeciality.BURNER:
                    str = "Burner Zombie";
                    break;
                case EZombieSpeciality.CRAWLER:
                    str = "Crawler Zombie";
                    break;
                case EZombieSpeciality.FLANKER_FRIENDLY:
                    str = "Friendly Flanker Zombie";
                    break;
                case EZombieSpeciality.FLANKER_STALK:
                    str = "Flanker Zombie";
                    break;
                case EZombieSpeciality.MEGA:
                    str = "Mega Zombie";
                    break;
                case EZombieSpeciality.SPRINTER:
                    str = "Sprinter Zombie";
                    break;
                default:
                    str = "Normal Zombie";
                    break;
            }
            return str;
        }
        #endregion

        #region Coroutines
        #endregion
    }
}