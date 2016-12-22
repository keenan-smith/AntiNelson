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
        public static bool ESP_Box = false;
        public static bool ESP_ShowNames = true;
        public static bool ESP_ShowDistances = true;
        public static bool ESP_IgnoreDistance = false;
        public static float ESP_Distance = 1000f;

        public static bool ESP_Players_Enabled = true;
        public static bool ESP_Players_ShowWeapons = true;
        public static bool ESP_Players_FilterFriends = false;
        public static bool ESP_Players_ShowIsAdmin = false;
        public static MP_ColorSelector ESP_Players_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Players"));
        public static MP_ColorSelector ESP_Friends_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Friends"));

        public static bool ESP_Zombies_Enabled = false;
        public static MP_ColorSelector ESP_Zombies_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Zombies"));

        public static bool ESP_Animals_Enabled = false;
        public static MP_ColorSelector ESP_Animals_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Animals"));

        public static bool ESP_Items_Enabled = false;
        public static MP_ColorSelector ESP_Items_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Items"));
        public static MP_ItemPicker ESP_Items_Types = new MP_ItemPicker(MP_Config.instance.getItemTypes("ESP"));

        public static bool ESP_Vehicles_Enabled = false;
        public static bool ESP_Vehicles_IgnoreDestroyed = false;
        public static bool ESP_Vehicles_IgnoreEmpty = false;
        public static bool ESP_Vehicles_IgnoreLocked = false;
        public static bool ESP_Vehicles_ShowFuel = true;
        public static bool ESP_Vehicles_ShowLocked = true;
        public static MP_ColorSelector ESP_Vehicles_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Vehicles"));

        public static bool ESP_Storages_Enabled = false;
        public static bool ESP_Storages_IgnoreLocked = false;
        public static bool ESP_Storages_ShowLocked = true;
        public static MP_ColorSelector ESP_Storages_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Storages"));

        public static bool ESP_Sentrys_Enabled = false;
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
                    if (ESP_ShowNames && !string.IsNullOrEmpty(drawing.text))
                        Tools.DrawLabel(drawing.screenPoint, drawing.text);
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
            ESP_Chams = GUILayout.Toggle(ESP_Chams, "Chams");
            ESP_Box = GUILayout.Toggle(ESP_Box, "Box");
            ESP_ShowNames = GUILayout.Toggle(ESP_ShowNames, "Show Names");
            ESP_ShowDistances = GUILayout.Toggle(ESP_ShowDistances, "Show Distances");
            ESP_IgnoreDistance = GUILayout.Toggle(ESP_IgnoreDistance, "Ignore Distance");
            GUILayout.Label("Distance: " + ESP_Distance);
            ESP_Distance = (float)Math.Round(GUILayout.HorizontalSlider(ESP_Distance, 0f, 50000f));

            GUILayout.Space(10f);
            ESP_Players_Enabled = GUILayout.Toggle(ESP_Players_Enabled, "Player ESP");
            ESP_Players_ShowWeapons = GUILayout.Toggle(ESP_Players_ShowWeapons, "Show Weapons");
            ESP_Players_ShowIsAdmin = GUILayout.Toggle(ESP_Players_ShowIsAdmin, "Show Admin");
            ESP_Players_FilterFriends = GUILayout.Toggle(ESP_Players_FilterFriends, "Filter Friends");
            ESP_Players_Color.draw("Player ESP Color", "Players");
            ESP_Friends_Color.draw("Friend ESP Color", "Friends");

            GUILayout.Space(10f);
            ESP_Zombies_Enabled = GUILayout.Toggle(ESP_Zombies_Enabled, "Zombie ESP");
            ESP_Zombies_Color.draw("Zombie ESP Color", "Zombies");

            GUILayout.Space(10f);
            ESP_Animals_Enabled = GUILayout.Toggle(ESP_Animals_Enabled, "Animal ESP");
            ESP_Animals_Color.draw("Animal ESP Color", "Animals");

            GUILayout.Space(10f);
            ESP_Items_Enabled = GUILayout.Toggle(ESP_Items_Enabled, "Item ESP");
            ESP_Items_Color.draw("Item ESP Color", "Items");
            ESP_Items_Types.draw("Item ESP Types", "ESP");

            GUILayout.Space(10f);
            ESP_Vehicles_Enabled = GUILayout.Toggle(ESP_Vehicles_Enabled, "Vehicle ESP");
            ESP_Vehicles_ShowFuel = GUILayout.Toggle(ESP_Vehicles_ShowFuel, "Show Fuel");
            ESP_Vehicles_ShowLocked = GUILayout.Toggle(ESP_Vehicles_ShowLocked, "Show Locked");
            ESP_Vehicles_IgnoreDestroyed = GUILayout.Toggle(ESP_Vehicles_IgnoreDestroyed, "Filter Destoryed");
            ESP_Vehicles_IgnoreEmpty = GUILayout.Toggle(ESP_Vehicles_IgnoreEmpty, "Filter Empty");
            ESP_Vehicles_IgnoreLocked = GUILayout.Toggle(ESP_Vehicles_IgnoreLocked, "Filter Locked");
            ESP_Vehicles_Color.draw("Vehicle ESP Color", "Vehicles");

            GUILayout.Space(10f);
            ESP_Storages_Enabled = GUILayout.Toggle(ESP_Storages_Enabled, "Storage ESP");
            ESP_Storages_ShowLocked = GUILayout.Toggle(ESP_Storages_ShowLocked, "Show Locked");
            ESP_Storages_IgnoreLocked = GUILayout.Toggle(ESP_Storages_IgnoreLocked, "Filter Locked");
            ESP_Storages_Color.draw("Storage ESP Color", "Storages");

            GUILayout.Space(10f);
            ESP_Sentrys_Enabled = GUILayout.Toggle(ESP_Sentrys_Enabled, "Sentry ESP");
            ESP_Sentrys_Color.draw("Sentry ESP Color", "Sentrys");
        }

        public static void clearDraw(EESPItem espItem)
        {
            draw.RemoveAll(a => a.type == espItem);
        }
        #endregion

        #region Coroutines
        private IEnumerator updateAnimalESP()
        {
            while (true)
            {
                clearDraw(EESPItem.ANIMAL);
                if ((!ESP_Enabled || !ESP_Animals_Enabled) || (Variables.animals == null || Variables.animals.Length < 1) || !Variables.isInGame)
                {
                    yield return wfs;
                    continue;
                }

                for(int i = 0; i < Variables.animals.Length; i++)
                {
                    Animal a = Variables.animals[i];
                    if (a == null || a.gameObject == null || a.isDead)
                        continue;

                    float distance = (float)Math.Round(Tools.getDistance(a.transform.position));
                    Vector3 screenPosition = MainCamera.instance.WorldToScreenPoint(a.transform.position);
                    Rect box = new Rect(0f, 0f, 0f, 0f);
                    string text = "";
                    Collider collider = a.gameObject.GetComponent<Collider>();

                    if (screenPosition.z <= 0)
                        continue;
                    screenPosition.x -= 64f;
                    screenPosition.y = (Screen.height - (screenPosition.y + 1f)) - 12f;

                    if (distance > ESP_Distance && !ESP_IgnoreDistance)
                        continue;

                    if (ESP_ShowNames)
                        text += a.asset.animalName + "\n";
                    if (ESP_ShowDistances)
                        text += "Distance: " + distance + "\n";
                    if (ESP_Box && collider != null)
                        box = Tools.BoundsToScreenRect(collider.bounds);

                    draw.Add(new ESPDraw(text, a.gameObject, EESPItem.ANIMAL, screenPosition, box, ESP_Animals_Color.selectedColor));
                }

                yield return wfs;
            }
        }

        private IEnumerator updateItemESP()
        {
            while (true)
            {
                clearDraw(EESPItem.ITEM);
                if ((!ESP_Enabled || !ESP_Items_Enabled) || (Variables.items == null || Variables.items.Length < 1) || !Variables.isInGame)
                {
                    yield return wfs;
                    continue;
                }

                for(int ii = 0; ii < Variables.items.Length; ii++)
                {
                    InteractableItem i = Variables.items[ii];
                    if (i == null || i.gameObject == null)
                        continue;

                    float distance = (float)Math.Round(Tools.getDistance(i.transform.position));
                    Vector3 screenPosition = MainCamera.instance.WorldToScreenPoint(i.transform.position);
                    Rect box = new Rect(0f, 0f, 0f, 0f);
                    string text = "";
                    Collider collider = i.gameObject.GetComponent<Collider>();

                    if (screenPosition.z <= 0)
                        continue;
                    screenPosition.x -= 64f;
                    screenPosition.y = (Screen.height - (screenPosition.y + 1f)) - 12f;

                    bool on;
                    if (!ESP_Items_Types.filter.TryGetValue(i.asset.type, out on))
                        on = false;
                    if (!on)
                        continue;
                    if (distance > ESP_Distance && !ESP_IgnoreDistance)
                        continue;

                    if (ESP_ShowNames)
                        text += i.asset.itemName + "\n";
                    if (ESP_ShowDistances)
                        text += "Distance: " + distance + "\n";
                    if (ESP_Box && collider != null)
                        box = Tools.BoundsToScreenRect(collider.bounds);

                    draw.Add(new ESPDraw(text, i.gameObject, EESPItem.ITEM, screenPosition, box, ESP_Items_Color.selectedColor));
                }

                yield return wfs;
            }
        }

        private IEnumerator updateVehicleESP()
        {
            while (true)
            {
                clearDraw(EESPItem.VEHICLE);
                if ((!ESP_Enabled || !ESP_Vehicles_Enabled) || (Variables.vehicles == null || Variables.vehicles.Length < 1) || !Variables.isInGame)
                {
                    yield return wfs;
                    continue;
                }

                for(int i = 0; i < Variables.vehicles.Length; i++)
                {
                    InteractableVehicle v = Variables.vehicles[i];
                    if (v == null || v.gameObject == null)
                        continue;

                    float distance = (float)Math.Round(Tools.getDistance(v.transform.position));
                    Vector3 screenPosition = MainCamera.instance.WorldToScreenPoint(v.transform.position);
                    Rect box = new Rect(0f, 0f, 0f, 0f);
                    string text = "";
                    Collider collider = v.gameObject.GetComponent<Collider>();

                    if (screenPosition.z <= 0)
                        continue;
                    screenPosition.x -= 64f;
                    screenPosition.y = (Screen.height - (screenPosition.y + 1f)) - 12f;

                    if (distance > ESP_Distance && !ESP_IgnoreDistance)
                        continue;
                    if (ESP_Vehicles_IgnoreDestroyed && (v.isDead || v.isDrowned))
                        continue;
                    if (ESP_Vehicles_IgnoreEmpty && v.fuel < 1)
                        continue;
                    if (ESP_Vehicles_IgnoreLocked && v.isLocked)
                        continue;

                    if (ESP_ShowNames)
                        text += v.name + "\n";
                    if (ESP_ShowDistances)
                        text += "Distance: " + distance + "\n";
                    if (ESP_Vehicles_ShowFuel)
                        text += "Fuel: " + v.fuel + "\n";
                    if (ESP_Vehicles_ShowLocked)
                        text += "Locked: " + (v.isLocked ? "Yes" : "No") + "\n";
                    if (ESP_Box && collider != null)
                        box = Tools.BoundsToScreenRect(collider.bounds);

                    draw.Add(new ESPDraw(text, v.gameObject, EESPItem.VEHICLE, screenPosition, box, ESP_Vehicles_Color.selectedColor));
                }

                yield return wfs;
            }
        }

        private IEnumerator updateStorageESP()
        {
            while (true)
            {
                clearDraw(EESPItem.STORAGE);
                if ((!ESP_Enabled || !ESP_Storages_Enabled) || (Variables.storages == null || Variables.storages.Length < 1) || !Variables.isInGame)
                {
                    yield return wfs;
                    continue;
                }

                for(int i = 0; i < Variables.storages.Length; i++)
                {
                    InteractableStorage s = Variables.storages[i];
                    if (s == null || s.gameObject == null)
                        continue;

                    float distance = (float)Math.Round(Tools.getDistance(s.transform.position));
                    Vector3 screenPosition = MainCamera.instance.WorldToScreenPoint(s.transform.position);
                    Rect box = new Rect(0f, 0f, 0f, 0f);
                    string text = "";
                    Collider collider = s.gameObject.GetComponent<Collider>();

                    if (screenPosition.z <= 0)
                        continue;
                    screenPosition.x -= 64f;
                    screenPosition.y = (Screen.height - (screenPosition.y + 1f)) - 12f;

                    if (distance > ESP_Distance && !ESP_IgnoreDistance)
                        continue;
                    if (ESP_Storages_IgnoreLocked && !s.checkUseable())
                        continue;

                    if (ESP_ShowNames)
                        text += "Storage\n";
                    if (ESP_ShowDistances)
                        text += "Distance: " + distance + "\n";
                    if (ESP_Storages_ShowLocked)
                        text += "Locked: " + (s.checkUseable() ? "No" : "Yes") + "\n";
                    if (ESP_Box && collider != null)
                        box = Tools.BoundsToScreenRect(collider.bounds);

                    draw.Add(new ESPDraw(text, s.gameObject, EESPItem.STORAGE, screenPosition, box, ESP_Storages_Color.selectedColor));
                }

                yield return wfs;
            }
        }

        private IEnumerator updateSentryESP()
        {
            while (true)
            {
                clearDraw(EESPItem.SENTRY);
                if ((!ESP_Enabled || !ESP_Sentrys_Enabled) || (Variables.sentrys == null || Variables.sentrys.Length < 1) || !Variables.isInGame)
                {
                    yield return wfs;
                    continue;
                }

                for(int i = 0; i < Variables.sentrys.Length; i++)
                {
                    InteractableSentry s = Variables.sentrys[i];
                    if (s == null || s.gameObject == null)
                        continue;

                    float distance = (float)Math.Round(Tools.getDistance(s.transform.position));
                    Vector3 screenPosition = MainCamera.instance.WorldToScreenPoint(s.transform.position);
                    Rect box = new Rect(0f, 0f, 0f, 0f);
                    string text = "";
                    Collider collider = s.gameObject.GetComponent<Collider>();

                    if (screenPosition.z <= 0)
                        continue;
                    screenPosition.x -= 64f;
                    screenPosition.y = (Screen.height - (screenPosition.y + 1f)) - 12f;

                    if (distance > ESP_Distance && !ESP_IgnoreDistance)
                        continue;

                    if (ESP_ShowNames)
                        text += "Sentry\n";
                    if (ESP_ShowDistances)
                        text += "Distance: " + distance + "\n";
                    if (ESP_Box && collider != null)
                        box = Tools.BoundsToScreenRect(collider.bounds);

                    draw.Add(new ESPDraw(text, s.gameObject, EESPItem.SENTRY, screenPosition, box, ESP_Sentrys_Color.selectedColor));
                }

                yield return wfs;
            }
        }
        #endregion
    }
}