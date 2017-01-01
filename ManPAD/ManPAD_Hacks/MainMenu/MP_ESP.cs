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

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(2, "ESP", 250f)]
    public class MP_ESP : MenuOption
    {
        #region Variables
        public static List<ESPDraw> draw = new List<ESPDraw>();
        private Texture2D _ESPTexture = new Texture2D(1, 1);
        private WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        private WaitForSeconds wfs = new WaitForSeconds(0.015f);

        public static bool ESP_Enabled = false;
        public static bool ESP_Chams = true;
        public static bool ESP_Glow = false;
        public static bool ESP_Box = false;
        public static bool ESP_IgnoreDistance = false;
        public static float ESP_Distance = 1000f;

        public static bool ESP_TextScale_Enabled = false;
        public static int ESP_TextScale_Start = 17;
        public static int ESP_TextScale_End = 10;
        public static float ESP_TextScale_Distance = 500f;
        public static int ESP_TextScale_Static = 11;

        public static bool ESP_Players_Enabled = true;
        public static bool ESP_Players_ShowNames = true;
        public static bool ESP_Players_ShowDistances = true;
        public static bool ESP_Players_ShowWeapons = true;
        public static bool ESP_Players_FilterFriends = false;
        public static bool ESP_Players_ShowIsAdmin = false;
        public static MP_ColorSelector ESP_Players_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Players"));
        public static MP_ColorSelector ESP_Friends_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Friends"));

        public static bool ESP_Zombies_Enabled = false;
        public static bool ESP_Zombies_ShowNames = true;
        public static bool ESP_Zombies_ShowDistances = true;
        public static MP_ColorSelector ESP_Zombies_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Zombies"));

        public static bool ESP_Animals_Enabled = false;
        public static bool ESP_Animals_ShowNames = true;
        public static bool ESP_Animals_ShowDistances = true;
        public static MP_ColorSelector ESP_Animals_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Animals"));

        public static bool ESP_Items_Enabled = false;
        public static bool ESP_Items_ShowNames = true;
        public static bool ESP_Items_ShowDistances = true;
        public static MP_ColorSelector ESP_Items_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Items"));
        public static MP_ItemPicker ESP_Items_Types = new MP_ItemPicker(MP_Config.instance.getItemTypes("ESP"));

        public static bool ESP_Vehicles_Enabled = false;
        public static bool ESP_Vehicles_ShowNames = true;
        public static bool ESP_Vehicles_IgnoreDestroyed = false;
        public static bool ESP_Vehicles_IgnoreEmpty = false;
        public static bool ESP_Vehicles_IgnoreLocked = false;
        public static bool ESP_Vehicles_ShowFuel = true;
        public static bool ESP_Vehicles_ShowLocked = true;
        public static bool ESP_Vehicles_ShowDistances = true;
        public static MP_ColorSelector ESP_Vehicles_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Vehicles"));

        public static bool ESP_Storages_Enabled = false;
        public static bool ESP_Storages_IgnoreLocked = false;
        public static bool ESP_Storages_ShowLocked = true;
        public static bool ESP_Storages_ShowDistances = true;
        public static MP_ColorSelector ESP_Storages_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Storages"));

        public static bool ESP_Sentrys_Enabled = false;
        public static bool ESP_Sentrys_ShowNames = true;
        public static bool ESP_Sentrys_ShowDistances = true;
        public static MP_ColorSelector ESP_Sentrys_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Sentrys"));

        
        #endregion

        #region Mono Functions

        public void OnGUI()
        {
            if (!Variables.isInGame || !ESP_Enabled || Variables.isSpying)
                return;

            if (Event.current.type != EventType.Repaint)
                return;
            lock (draw)
            {
                for(int i = 0; i < draw.Count; i++)
                {
                    ESPDraw drawing = draw[i];

                    GUI.color = drawing.color;
                    if (!string.IsNullOrEmpty(drawing.text))
                    {
                        float size = ESP_TextScale_Static;
                        if (ESP_TextScale_Enabled)
                        {
                            float ratio = ESP_TextScale_Distance / drawing.distance;
                            float finalratio = 1 - (1 / ratio);
                            if (ratio >= 1)
                            {
                                size = ESP_TextScale_Start * finalratio;
                            }
                            if (size <= ESP_TextScale_End || ratio < 1)
                                size = ESP_TextScale_End;
                        }
                        Tools.DrawLabel(drawing.screenPoint, drawing.text, drawing.color, (int)size);
                    }
                    if (ESP_Box)
                        Tools.Outline(drawing.box, _ESPTexture);
                }
            }
            
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            ESP_Enabled = GUILayout.Toggle(ESP_Enabled, "Enabled");
#if !FREE
            ESP_Chams = GUILayout.Toggle(ESP_Chams, "Chams");
            ESP_Box = GUILayout.Toggle(ESP_Box, "Box");
#endif
            ESP_Glow = GUILayout.Toggle(ESP_Glow, "Glow");
            ESP_IgnoreDistance = GUILayout.Toggle(ESP_IgnoreDistance, "Ignore Distance");
            GUILayout.Label("Distance: " + ESP_Distance);
            ESP_Distance = (float)Math.Round(GUILayout.HorizontalSlider(ESP_Distance, 0f, 4000f));

            GUILayout.Space(10f);
            ESP_TextScale_Enabled = GUILayout.Toggle(ESP_TextScale_Enabled, "Scale ESP Text With Distance");
            GUILayout.Label("Font Size Closest: " + ESP_TextScale_Start);
            ESP_TextScale_Start = (int)(GUILayout.HorizontalSlider(ESP_TextScale_Start, (ESP_TextScale_End + 1), 50f));
            GUILayout.Label("Font Size Farthest: " + ESP_TextScale_End);
            ESP_TextScale_End = (int)(GUILayout.HorizontalSlider(ESP_TextScale_End, 0f, (ESP_TextScale_Start - 1)));
            GUILayout.Label("Text Scale Dropoff Distance: " + ESP_TextScale_Distance);
            ESP_TextScale_Distance = (int)(GUILayout.HorizontalSlider(ESP_TextScale_Distance, 0f, 4000f));
            GUILayout.Label("Static ESP Text Size: " + ESP_TextScale_Static);
            ESP_TextScale_Static = (int)(GUILayout.HorizontalSlider(ESP_TextScale_Static, 0f, 50f));

            GUILayout.Space(10f);
            ESP_Players_Enabled = GUILayout.Toggle(ESP_Players_Enabled, "Player ESP");
            ESP_Players_ShowNames = GUILayout.Toggle(ESP_Players_ShowNames, "Show Player Names");
            ESP_Players_ShowDistances = GUILayout.Toggle(ESP_Players_ShowDistances, "Show Player Distances");
            ESP_Players_ShowWeapons = GUILayout.Toggle(ESP_Players_ShowWeapons, "Show Weapons");
            ESP_Players_ShowIsAdmin = GUILayout.Toggle(ESP_Players_ShowIsAdmin, "Show Admin");
            ESP_Players_FilterFriends = GUILayout.Toggle(ESP_Players_FilterFriends, "Filter Friends");
            ESP_Players_Color.draw("Player ESP Color", "Players");
            ESP_Friends_Color.draw("Friend ESP Color", "Friends");

            GUILayout.Space(10f);
            ESP_Zombies_Enabled = GUILayout.Toggle(ESP_Zombies_Enabled, "Zombie ESP");
            ESP_Zombies_ShowNames = GUILayout.Toggle(ESP_Zombies_ShowNames, "Show Zombie Names");
            ESP_Zombies_ShowDistances = GUILayout.Toggle(ESP_Zombies_ShowDistances, "Show Zombie Distances");
            ESP_Zombies_Color.draw("Zombie ESP Color", "Zombies");

            GUILayout.Space(10f);
            ESP_Animals_Enabled = GUILayout.Toggle(ESP_Animals_Enabled, "Animal ESP");
            ESP_Animals_ShowNames = GUILayout.Toggle(ESP_Animals_ShowNames, "Show Animal Names");
            ESP_Animals_ShowDistances = GUILayout.Toggle(ESP_Animals_ShowDistances, "Show Animal Distances");
            ESP_Animals_Color.draw("Animal ESP Color", "Animals");

            GUILayout.Space(10f);
            ESP_Items_Enabled = GUILayout.Toggle(ESP_Items_Enabled, "Item ESP");
            ESP_Items_ShowNames = GUILayout.Toggle(ESP_Items_ShowNames, "Show Item Names");
            ESP_Items_ShowDistances = GUILayout.Toggle(ESP_Items_ShowDistances, "Show Item Distances");
            ESP_Items_Color.draw("Item ESP Color", "Items");
            ESP_Items_Types.draw("Item ESP Types", "ESP");

            GUILayout.Space(10f);
            ESP_Vehicles_Enabled = GUILayout.Toggle(ESP_Vehicles_Enabled, "Vehicle ESP");
            ESP_Vehicles_ShowNames = GUILayout.Toggle(ESP_Vehicles_ShowNames, "Show Vehicle Names");
            ESP_Vehicles_ShowDistances = GUILayout.Toggle(ESP_Vehicles_ShowDistances, "Show Vehicle Distances");
            ESP_Vehicles_ShowFuel = GUILayout.Toggle(ESP_Vehicles_ShowFuel, "Show Fuel");
            ESP_Vehicles_ShowLocked = GUILayout.Toggle(ESP_Vehicles_ShowLocked, "Show Locked");
            ESP_Vehicles_IgnoreDestroyed = GUILayout.Toggle(ESP_Vehicles_IgnoreDestroyed, "Filter Destroyed");
            ESP_Vehicles_IgnoreEmpty = GUILayout.Toggle(ESP_Vehicles_IgnoreEmpty, "Filter Empty");
            ESP_Vehicles_IgnoreLocked = GUILayout.Toggle(ESP_Vehicles_IgnoreLocked, "Filter Locked");
            ESP_Vehicles_Color.draw("Vehicle ESP Color", "Vehicles");

            GUILayout.Space(10f);
            ESP_Storages_Enabled = GUILayout.Toggle(ESP_Storages_Enabled, "Storage ESP");
            ESP_Storages_ShowDistances = GUILayout.Toggle(ESP_Storages_ShowDistances, "Show Storage Distances");
            ESP_Storages_ShowLocked = GUILayout.Toggle(ESP_Storages_ShowLocked, "Show Locked");
            ESP_Storages_IgnoreLocked = GUILayout.Toggle(ESP_Storages_IgnoreLocked, "Filter Locked");
            ESP_Storages_Color.draw("Storage ESP Color", "Storages");

            GUILayout.Space(10f);
            ESP_Sentrys_Enabled = GUILayout.Toggle(ESP_Sentrys_Enabled, "Sentry ESP");
            ESP_Sentrys_ShowNames = GUILayout.Toggle(ESP_Sentrys_ShowNames, "Show Sentry Names");
            ESP_Sentrys_ShowDistances = GUILayout.Toggle(ESP_Sentrys_ShowDistances, "Show Sentry Distances");
            ESP_Sentrys_Color.draw("Sentry ESP Color", "Sentrys");
        }

        public static void clearDraw(EESPItem espItem)
        {
            draw.RemoveAll(a => a.type == espItem);
        }
        #endregion

        #region Coroutines
        #endregion
    }
}