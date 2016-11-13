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
    [MenuOption(2, "ESP", 200f, 500f)]
    public class MP_ESP : MenuOption
    {
        #region Variables
        private List<ESPDraw> _draw = new List<ESPDraw>();
        private Texture2D _ESPTexture = new Texture2D(1, 1);

        public bool ESP_Enabled = false;
        public bool ESP_Chams = true;
        public bool ESP_Box = false;
        public bool ESP_ShowNames = true;
        public bool ESP_ShowDistances = true;
        public float ESP_Distance = 1000f;

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
        public bool ESP_Vehicles_IgnoreDestroyed = false;
        public bool ESP_Vehicles_IgnoreEmpty = false;
        public bool ESP_Vehicles_IgnoreLocked = false;
        public bool ESP_Vehicles_ShowFuel = true;
        public bool ESP_Vehicles_ShowLocked = true;
        public MP_ColorSelector ESP_Vehicles_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Vehicles"));

        public bool ESP_Storages_Enabled = true;
        public bool ESP_Storages_IgnoreLocked = false;
        public bool ESP_Storages_ShowLocked = true;
        public MP_ColorSelector ESP_Storages_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Storages"));

        public bool ESP_Sentrys_Enabled = false;
        public MP_ColorSelector ESP_Sentrys_Color = new MP_ColorSelector(MP_Config.instance.getESPColor("Sentrys"));
        #endregion

        #region Mono Functions
        public void Start()
        {
            StartCoroutine(updateESP());
        }

        public void OnGUI()
        {
            if (!Variables.isInGame || !ESP_Enabled)
                return;

            if (Event.current.type != EventType.Repaint)
                return;
            lock (_draw)
            {
                foreach (ESPDraw drawing in _draw)
                {
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
        private IEnumerator updateESP()
        {
            while (true)
            {
                if (!ESP_Enabled || !Variables.isInGame)
                {
                    yield return null;
                    continue;
                }

                _draw.Clear();

                #region ESP: Players
                if (ESP_Players_Enabled && (Variables.players != null && Variables.players.Length > 0))
                {
                    foreach (SteamPlayer p in Variables.players)
                    {
                        if (p == null || p.player == null || p.player.gameObject == null || p.player == Player.player || p.player.life.isDead)
                            continue;

                        float distance = (float)Math.Round(Tools.getDistance(p.player.transform.position));
                        Vector3 screenPosition = MainCamera.instance.WorldToScreenPoint(p.player.transform.position);
                        Rect box = new Rect(0f, 0f, 0f, 0f);
                        string text = "";
                        Collider collider = p.player.gameObject.GetComponent<Collider>();
                        bool isFriend = MP_Config.instance.getFriends().Contains(p.playerID.steamID.m_SteamID);

                        if (screenPosition.z <= 0)
                            continue;
                        screenPosition.x -= 64f;
                        screenPosition.y = (Screen.height - (screenPosition.y + 1f)) - 12f;

                        if (distance > ESP_Distance)
                            continue;
                        if (ESP_Players_FilterFriends && isFriend)
                            continue;

                        if (ESP_ShowNames)
                            text += p.playerID.characterName + "\n";
                        if (ESP_ShowDistances)
                            text += "Distance: " + distance + "\n";
                        if (ESP_Players_ShowWeapons)
                            text += "Weapon: " + (p.player.equipment.asset == null ? "None" : p.player.equipment.asset.itemName) + "\n";
                        if (ESP_Players_ShowIsAdmin)
                            text += "Is Admin: " + (p.isAdmin ? "Yes" : "No") + "\n";
                        if (ESP_Box && collider != null)
                            box = Tools.BoundsToScreenRect(collider.bounds);

                        _draw.Add(new ESPDraw(text, p.player.gameObject, EESPItem.PLAYER, screenPosition, box, (isFriend ? ESP_Friends_Color.selectedColor : ESP_Players_Color.selectedColor)));
                    }
                }
                #endregion

                #region ESP: Zombies
                if (ESP_Zombies_Enabled && (Variables.zombies != null && Variables.zombies.Length > 0))
                {
                    foreach (Zombie z in Variables.zombies)
                    {
                        if (z == null || z.gameObject == null || z.isDead)
                            continue;

                        float distance = (float)Math.Round(Tools.getDistance(z.transform.position));
                        Vector3 screenPosition = MainCamera.instance.WorldToScreenPoint(z.transform.position);
                        Rect box = new Rect(0f, 0f, 0f, 0f);
                        string text = "";
                        Collider collider = z.gameObject.GetComponent<Collider>();

                        if (screenPosition.z <= 0)
                            continue;
                        screenPosition.x -= 64f;
                        screenPosition.y = (Screen.height - (screenPosition.y + 1f)) - 12f;

                        if (distance > ESP_Distance)
                            continue;

                        if (ESP_ShowNames)
                            text += getZombieName(z) + "\n";
                        if (ESP_ShowDistances)
                            text += "Distance: " + distance + "\n";
                        if (ESP_Box && collider != null)
                            box = Tools.BoundsToScreenRect(collider.bounds);

                        _draw.Add(new ESPDraw(text, z.gameObject, EESPItem.ZOMBIE, screenPosition, box, ESP_Zombies_Color.selectedColor));
                    }
                }
                #endregion

                #region ESP: Animals
                if (ESP_Animals_Enabled && (Variables.animals != null && Variables.animals.Length > 0))
                {
                    foreach (Animal a in Variables.animals)
                    {
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

                        if (distance > ESP_Distance)
                            continue;

                        if (ESP_ShowNames)
                            text += a.name + "\n";
                        if (ESP_ShowDistances)
                            text += "Distance: " + distance + "\n";
                        if (ESP_Box && collider != null)
                            box = Tools.BoundsToScreenRect(collider.bounds);

                        _draw.Add(new ESPDraw(text, a.gameObject, EESPItem.ANIMAL, screenPosition, box, ESP_Animals_Color.selectedColor));
                    }
                }
                #endregion

                #region ESP: Items
                if (ESP_Items_Enabled && (Variables.items != null && Variables.items.Length > 0))
                {
                    foreach (InteractableItem i in Variables.items)
                    {
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
                        if (distance > ESP_Distance)
                            continue;

                        if (ESP_ShowNames)
                            text += i.asset.itemName + "\n";
                        if (ESP_ShowDistances)
                            text += "Distance: " + distance + "\n";
                        if (ESP_Box && collider != null)
                            box = Tools.BoundsToScreenRect(collider.bounds);

                        _draw.Add(new ESPDraw(text, i.gameObject, EESPItem.ITEM, screenPosition, box, ESP_Items_Color.selectedColor));
                    }
                }
                #endregion

                #region ESP: Vehicles
                if (ESP_Vehicles_Enabled && (Variables.vehicles != null && Variables.vehicles.Length > 0))
                {
                    foreach (InteractableVehicle v in Variables.vehicles)
                    {
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

                        if (distance > ESP_Distance)
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

                        _draw.Add(new ESPDraw(text, v.gameObject, EESPItem.VEHICLE, screenPosition, box, ESP_Vehicles_Color.selectedColor));
                    }
                }
                #endregion

                #region ESP: Storages
                if (ESP_Storages_Enabled && (Variables.storages != null && Variables.storages.Length > 0))
                {
                    foreach (InteractableStorage s in Variables.storages)
                    {
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

                        if (distance > ESP_Distance)
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

                        _draw.Add(new ESPDraw(text, s.gameObject, EESPItem.STORAGE, screenPosition, box, ESP_Storages_Color.selectedColor));
                    }
                }
                #endregion

                #region ESP: Sentrys
                if (ESP_Sentrys_Enabled && (Variables.sentrys != null && Variables.sentrys.Length > 0))
                {
                    foreach (InteractableSentry s in Variables.sentrys)
                    {
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

                        if (distance > ESP_Distance)
                            continue;

                        if (ESP_ShowNames)
                            text += "Sentry\n";
                        if (ESP_ShowDistances)
                            text += "Distance: " + distance + "\n";
                        if (ESP_Box && collider != null)
                            box = Tools.BoundsToScreenRect(collider.bounds);

                        _draw.Add(new ESPDraw(text, s.gameObject, EESPItem.SENTRY, screenPosition, box, ESP_Sentrys_Color.selectedColor));
                    }
                }
                #endregion

                yield return null;
            }
        }
        #endregion
    }
}