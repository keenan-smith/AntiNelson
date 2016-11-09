using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool ESP_Zombies_Enabled = false;

        public bool ESP_Items_Enabled = false;
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
        public IEnumerator setDraw()
        {

        }
        #endregion
    }
}
