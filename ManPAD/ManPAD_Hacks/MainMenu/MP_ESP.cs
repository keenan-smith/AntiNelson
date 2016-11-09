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
    [MenuOption(1, "ESP", 130f, 130f)]
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
        public bool ESP_Players_FilterFriends = true;
        public bool ESP_Players_ShowIsAdmin = true;
        public MP_ColorSelector ESP_Players_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Players"));
        public MP_ColorSelector ESP_Friends_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Friends"));

        public bool ESP_Zombies_Enabled = false;
        public MP_ColorSelector ESP_Zombies_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Zombies"));

        public bool ESP_Animals_Enabled = false;
        public MP_ColorSelector ESP_Animals_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Animals"));

        public bool ESP_Items_Enabled = false;
        public MP_ColorSelector ESP_Items_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Items"));

        public bool ESP_Vehicles_Enabled = false;
        public bool ESP_Vehicles_IgnoreDestroyed = true;
        public bool ESP_Vehicles_IgnoreEmpty = true;
        public bool ESP_Vehciles_ShowFuel = true;
        public MP_ColorSelector ESP_Vehicles_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Vehicles"));

        public bool ESP_Storages_Enabled = false;
        public bool ESP_Storages_IgnoreLocked = false;
        public bool ESP_Storages_ShowLocked = true;
        public MP_ColorSelector ESP_Storages_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Storages"));

        public bool ESP_Sentrys_Enabled = false;
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

        }

        private string getZombieName(Zombie z)
        {
            if (z.speciality == EZombieSpeciality.ACID)
            {
                return "Acid Zombie";
            }
            else if (z.speciality == EZombieSpeciality.BURNER)
            {
                return "Burner Zombie";
            }
            else if (z.speciality == EZombieSpeciality.CRAWLER)
            {
                return "Crawler Zombie";
            }
            else if (z.speciality == EZombieSpeciality.FLANKER_FRIENDLY)
            {
                return "Friendly Flanker Zombie";
            }
            else if (z.speciality == EZombieSpeciality.FLANKER_STALK)
            {
                return "Stalker Zombie";
            }
            else if (z.speciality == EZombieSpeciality.MEGA)
            {
                return "Mega Zombie";
            }
            else if (z.speciality == EZombieSpeciality.SPRINTER)
            {
                return "Spitter Zombie";
            }
            else
            {
                return "Normal Zombie";
            }
        }
        #endregion

        #region Coroutines
        #endregion
    }
}