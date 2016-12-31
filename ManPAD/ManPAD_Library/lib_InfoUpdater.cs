using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.Types;
using ManPAD.ManPAD_Hacks.MainMenu;
using ManPAD.ManPAD_API;

namespace ManPAD.ManPAD_Library
{
#if DEBUG
    // Zoomy's amazing debug console (dont touch u queers)
    public class lib_Console : MonoBehaviour
    {
        public Rect menu = new Rect(10, 220, 450, 200);
        Vector2 Scrollposition;
        bool handleClicked = false;
        Vector3 clickedPosition = new Vector3(0, 0, 0);
        int minWindowWidth = 200;
        int maxWindowWidth = 1920;
        int minWindowHeight = 200;
        int maxWindowHeight = 1080;
        Rect originalWindow;
        public static List<string> logtext = new List<string>();

        void Start()
        {
            originalWindow = menu;
        }

        void OnGUI()
        {
            if (false)
            {
                menu = GUILayout.Window(999, menu, DoMenu, "Console");
                var mousePos = Input.mousePosition;
                mousePos.y = Screen.height - mousePos.y;
                Rect windowHandle = new Rect(menu.x + menu.width - 10, menu.y + menu.height - 10, 10, 10);
                GUI.Box(windowHandle, "");
                if (Input.GetMouseButtonDown(0) && windowHandle.Contains(mousePos))
                {
                    handleClicked = true;
                    clickedPosition = mousePos;
                    originalWindow = menu;
                }

                if (handleClicked)
                {
                    if (Input.GetMouseButton(0))
                    {
                        menu.width = Mathf.Clamp(originalWindow.width + (mousePos.x - clickedPosition.x), minWindowWidth, maxWindowWidth);
                        menu.height = Mathf.Clamp(originalWindow.height + (mousePos.y - clickedPosition.y), minWindowHeight, maxWindowHeight);
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        handleClicked = false;
                    }
                }
            }

        }

        void DoMenu(int windowID)
        {

            Scrollposition = GUILayout.BeginScrollView(Scrollposition);

            foreach (string text in logtext)
            {
                GUILayout.Label(text);
            }

            GUILayout.EndScrollView();
            if (GUILayout.Button("Clear"))
            {
                logtext.Clear();
            }

            if (GUILayout.Button("Close"))
            {
            }
            if (!handleClicked)
            {
                GUI.DragWindow();
            }
        }
        void Update()
        {
            {
                while (logtext.Count > 200)
                {
                    logtext.RemoveAt(0);
                }
            }
        }

        public static void log(string text)
        {
            logtext.Add(DateTime.Now.ToString("h:mm:ss tt") + ": " + text);
        }
    }
#endif

    public class lib_InfoUpdater : MonoBehaviour
    {
        #region Variables
        public List<GOUpdate> collections = new List<GOUpdate>();

        private WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        private WaitForSeconds wfs = new WaitForSeconds(0.015f);
        private Shader normal;
        private Shader esp = null;
        #endregion

        #region Mono Functions
        public void Start()
        {
            InvokeRepeating("reloadInfoInvoke", 5f, 5f);
            StartCoroutine(updateStuff());
            normal = Shader.Find("Standard");
        }

        public void Update()
        {
            if (Variables.bundle_chams == null)
                return;

            if (esp == null)
            {
                Variables.bundle_chams.LoadAllAssets<Material>();
                esp = Variables.bundle_chams.LoadAllAssets<Shader>()[0];
            }
        }
        #endregion

        #region Functions
        private void reloadInfoInvoke()
        {
            try
            {
                Variables.isInGame = (!LoadingUI.isBlocked && !Provider.isLoading && Provider.isConnected && Provider.clients != null && Provider.clients.Count > 0);
                if (!Variables.isInGame)
                    return;

                if (Variables.LocalPlayer == null || Variables.LocalSteamPlayer == null)
                {
                    Variables.LocalPlayer = Player.player;
                    Variables.LocalSteamPlayer = Player.player.channel.owner;
                }

                collections.Clear();

                for (int i = 0; i < Provider.clients.Count; i++)
                    collections.Add(new GOUpdate(Provider.clients[i]));

                for (int i = 0; i < ZombieManager.regions.Length; i++)
                    for (int a = 0; a < ZombieManager.regions[i].zombies.Count; a++)
                        collections.Add(new GOUpdate(ZombieManager.regions[i].zombies[a]));

                foreach (InteractableItem item in UnityEngine.Object.FindObjectsOfType(typeof(InteractableItem)) as InteractableItem[])
                    collections.Add(new GOUpdate(item));

                for (int i = 0; i < VehicleManager.vehicles.Count; i++)
                    collections.Add(new GOUpdate(VehicleManager.vehicles[i]));

                InteractableStorage[] storages = UnityEngine.Object.FindObjectsOfType(typeof(InteractableStorage)) as InteractableStorage[];
                for (int i = 0; i < storages.Count(); i++)
                    collections.Add(new GOUpdate(storages[i]));

                InteractableSentry[] sentrys = UnityEngine.Object.FindObjectsOfType(typeof(InteractableSentry)) as InteractableSentry[];
                for (int i = 0; i < sentrys.Count(); i++)
                    collections.Add(new GOUpdate(sentrys[i]));

                for (int i = 0; i < AnimalManager.animals.Count; i++)
                    collections.Add(new GOUpdate(AnimalManager.animals[i]));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        #endregion

        #region Coroutines
        public IEnumerator updateStuff()
        {
            while (true)
            {
                float aim_distanceAway = float.MaxValue;
                object aim_nextTarget = null;
                MP_ESP.draw.Clear();

                lock (collections)
                {
                    for (int i = 0; i < collections.Count; i++)
                    {
                        if (collections[i] == null || collections[i].gameObject == null || collections[i].instance == null)
                            continue;

                        GOUpdate updateObject = collections[i];
                        
                        #region Players
                        if (updateObject.type == EGOUpdate.PLAYER)
                        {
                            if (!((SteamPlayer)updateObject.instance).player.life.isDead && ((SteamPlayer)updateObject.instance).player != Player.player)
                            {
                                int distance = (int)Tools.getDistance(updateObject.gameObject.transform.position);

                                #region Aimbot & SilentAim
                                if ((MP_Aimbot.aimbot || MP_Aimbot.silentAim) && MP_Aimbot.aim_players)
                                {
                                    if (aim_distanceAway > distance && (distance <= MP_Aimbot.distance || MP_Aimbot.ignoreDistance))
                                    {
                                        aim_distanceAway = distance;
                                        aim_nextTarget = updateObject.instance;
                                    }
                                }
                                #endregion

                                #region ESP
                                if (MP_ESP.ESP_Enabled && MP_ESP.ESP_Players_Enabled && (((SteamPlayer)updateObject.instance).player != Player.player))
                                {
                                    if (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance)
                                    {
                                        bool isFriend = MP_Players.isFriend((SteamPlayer)updateObject.instance);

                                        updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                        updateObject.text = "";
                                        updateObject.color = (isFriend ? MP_ESP.ESP_Friends_Color.selectedColor : MP_ESP.ESP_Players_Color.selectedColor);

                                        if (updateObject.screenPosition.z > 0)
                                        {
                                            updateObject.screenPosition.y = (Screen.height - (updateObject.screenPosition.y + 1f)) - 12f;
                                            SteamPlayer p = (SteamPlayer)updateObject.instance;

                                            if (MP_ESP.ESP_ShowNames)
                                                updateObject.text += p.playerID.characterName + "\n";
                                            if (MP_ESP.ESP_ShowDistances)
                                                updateObject.text += "Distance: " + distance + "\n";
                                            if (MP_ESP.ESP_Players_ShowWeapons)
                                                updateObject.text += "Weapon: " + (p.player.equipment.asset == null ? "None" : p.player.equipment.asset.itemName) + "\n";
                                            if (MP_ESP.ESP_Players_ShowIsAdmin)
                                                updateObject.text += "Is Admin: " + (p.isAdmin ? "Yes" : "No") + "\n";
                                            if (MP_ESP.ESP_Box)
                                                updateObject.box = Tools.BoundsToScreenRect(new Bounds(p.player.transform.position + new Vector3(0, 1.1f, 0), p.player.transform.localScale + new Vector3(0, .95f, 0)));

                                            MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.PLAYER, updateObject.screenPosition, updateObject.box, updateObject.color));
                                        }
                                    }
                                }
                                #endregion

                                #region Chams
                                if (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance)
                                {
                                    Renderer[] renderers = ((SteamPlayer)updateObject.instance).player.gameObject.GetComponentsInChildren<Renderer>();

                                    for (int a = 0; a < renderers.Length; a++)
                                    {
                                        for (int b = 0; b < renderers[a].materials.Length; b++)
                                        {
                                            if (Variables.isSpying || !MP_ESP.ESP_Enabled || !MP_ESP.ESP_Chams || !MP_ESP.ESP_Players_Enabled)
                                            {
                                                if (renderers[a].materials[b].shader == esp)
                                                    renderers[a].materials[b].shader = normal;
                                            }
                                            else
                                            {
                                                if (renderers[a].materials[b].shader != esp)
                                                    renderers[a].materials[b].shader = esp;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region Zombies
                        if (updateObject.type == EGOUpdate.ZOMBIE && !((Zombie)updateObject.instance).isDead)
                        {
                            int distance = (int)Tools.getDistance(updateObject.gameObject.transform.position);

                            #region Aimbot & SilentAim
                            if ((MP_Aimbot.aimbot || MP_Aimbot.silentAim) && MP_Aimbot.aim_zombies)
                            {
                                if (aim_distanceAway > distance && (distance <= MP_Aimbot.distance || MP_Aimbot.ignoreDistance))
                                {
                                    aim_distanceAway = distance;
                                    aim_nextTarget = updateObject.instance;
                                }
                            }
                            #endregion

                            #region ESP
                            if (MP_ESP.ESP_Enabled && MP_ESP.ESP_Zombies_Enabled && (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance))
                            {
                                updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                updateObject.text = "";
                                updateObject.color = MP_ESP.ESP_Zombies_Color.selectedColor;

                                if (updateObject.screenPosition.z > 0)
                                {
                                    Collider collider = null;

                                    if (MP_ESP.ESP_Box)
                                        collider = updateObject.gameObject.GetComponent<Collider>();
                                    updateObject.screenPosition.x -= 64f;
                                    updateObject.screenPosition.y = (Screen.height - (updateObject.screenPosition.y + 1f)) - 12f;

                                    if (MP_ESP.ESP_ShowNames)
                                        updateObject.text += Tools.getZombieName((Zombie)updateObject.instance) + "\n";
                                    if (MP_ESP.ESP_ShowDistances)
                                        updateObject.text += "Distance: " + distance + "\n";
                                    if (MP_ESP.ESP_Box && collider != null)
                                        updateObject.box = Tools.BoundsToScreenRect(collider.bounds);

                                    MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.ZOMBIE, updateObject.screenPosition, updateObject.box, updateObject.color));
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Animals
                        if (updateObject.type == EGOUpdate.ANIMAL && !((Animal)updateObject.instance).isDead && MP_ESP.ESP_Enabled && MP_ESP.ESP_Animals_Enabled)
                        {
                            Animal animal = (Animal)updateObject.instance;
                            int distance = (int)Tools.getDistance(updateObject.gameObject.transform.position);

                            #region ESP
                            if (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance)
                            {
                                updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                updateObject.text = "";
                                updateObject.color = MP_ESP.ESP_Animals_Color.selectedColor;

                                if (updateObject.screenPosition.z > 0)
                                {
                                    Collider collider = null;

                                    if (MP_ESP.ESP_Box)
                                        collider = updateObject.gameObject.GetComponent<Collider>();
                                    updateObject.screenPosition.x -= 64f;
                                    updateObject.screenPosition.y = (Screen.height - (updateObject.screenPosition.y + 1f)) - 12f;

                                    if (MP_ESP.ESP_ShowNames)
                                        updateObject.text += animal.asset.animalName + "\n";
                                    if (MP_ESP.ESP_ShowDistances)
                                        updateObject.text += "Distance: " + distance + "\n";
                                    if (MP_ESP.ESP_Box && collider != null)
                                        updateObject.box = Tools.BoundsToScreenRect(collider.bounds);

                                    MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.ANIMAL, updateObject.screenPosition, updateObject.box, updateObject.color));
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Items
                        if (updateObject.type == EGOUpdate.ITEM)
                        {
                            InteractableItem item = (InteractableItem)updateObject.instance;
                            bool on = false;
                            if (MP_ESP.ESP_Items_Types.filter.TryGetValue(item.asset.type, out on))
                            {
                                if (on)
                                {
                                    int distance = (int)Tools.getDistance(updateObject.gameObject.transform.position);

                                    #region ESP
                                    if (MP_ESP.ESP_Enabled && MP_ESP.ESP_Items_Enabled && (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance))
                                    {
                                        updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                        updateObject.text = "";
                                        updateObject.color = MP_ESP.ESP_Items_Color.selectedColor;

                                        if (updateObject.screenPosition.z > 0)
                                        {
                                            Collider collider = null;

                                            if (MP_ESP.ESP_Box)
                                                collider = updateObject.gameObject.GetComponent<Collider>();
                                            updateObject.screenPosition.x -= 64f;
                                            updateObject.screenPosition.y = (Screen.height - (updateObject.screenPosition.y + 1f)) - 12f;

                                            if (MP_ESP.ESP_ShowNames)
                                                updateObject.text += item.asset.itemName + "\n";
                                            if (MP_ESP.ESP_ShowDistances)
                                                updateObject.text += "Distance: " + distance + "\n";
                                            if (MP_ESP.ESP_Box && collider != null)
                                                updateObject.box = Tools.BoundsToScreenRect(collider.bounds);

                                            MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.ITEM, updateObject.screenPosition, updateObject.box, updateObject.color));
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion

                        #region Vehicles
                        if (updateObject.type == EGOUpdate.VEHICLE && MP_ESP.ESP_Enabled && MP_ESP.ESP_Vehicles_Enabled)
                        {
                            InteractableVehicle vehicle = (InteractableVehicle)updateObject.instance;
                            int distance = (int)Tools.getDistance(updateObject.gameObject.transform.position);

                            #region ESP
                            if (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance)
                            {
                                updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                updateObject.text = "";
                                updateObject.color = MP_ESP.ESP_Vehicles_Color.selectedColor;

                                if (updateObject.screenPosition.z > 0)
                                {
                                    Collider collider = null;

                                    if (MP_ESP.ESP_Box)
                                        collider = updateObject.gameObject.GetComponent<Collider>();
                                    updateObject.screenPosition.x -= 64f;
                                    updateObject.screenPosition.y = (Screen.height - (updateObject.screenPosition.y + 1f)) - 12f;

                                    if (MP_ESP.ESP_ShowNames)
                                        updateObject.text += vehicle.asset.vehicleName + "\n";
                                    if (MP_ESP.ESP_Vehicles_ShowFuel)
                                        updateObject.text += "Fuel: " + vehicle.fuel + "\n";
                                    if (MP_ESP.ESP_Vehicles_ShowLocked)
                                        updateObject.text += "Locked: " + (vehicle.isLocked ? "Yes" : "No") + "\n";
                                    if (MP_ESP.ESP_ShowDistances)
                                        updateObject.text += "Distance: " + distance + "\n";
                                    if (MP_ESP.ESP_Box && collider != null)
                                        updateObject.box = Tools.BoundsToScreenRect(collider.bounds);

                                    MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.VEHICLE, updateObject.screenPosition, updateObject.box, updateObject.color));
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Storages
                        if (updateObject.type == EGOUpdate.STORAGE && MP_ESP.ESP_Enabled && MP_ESP.ESP_Storages_Enabled)
                        {
                            InteractableStorage storage = (InteractableStorage)updateObject.instance;
                            int distance = (int)Tools.getDistance(updateObject.gameObject.transform.position);

                            #region ESP
                            if (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance)
                            {
                                updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                updateObject.text = "";
                                updateObject.color = MP_ESP.ESP_Storages_Color.selectedColor;

                                if (updateObject.screenPosition.z > 0)
                                {
                                    Collider collider = null;

                                    if (MP_ESP.ESP_Box)
                                        collider = updateObject.gameObject.GetComponent<Collider>();
                                    updateObject.screenPosition.x -= 64f;
                                    updateObject.screenPosition.y = (Screen.height - (updateObject.screenPosition.y + 1f)) - 12f;

                                    if (MP_ESP.ESP_ShowNames)
                                        updateObject.text += "Storage\n";
                                    if (MP_ESP.ESP_Storages_ShowLocked)
                                        updateObject.text += "Locked: " + (storage.checkUseable() ? "Yes" : "No") + "\n";
                                    if (MP_ESP.ESP_ShowDistances)
                                        updateObject.text += "Distance: " + distance + "\n";
                                    if (MP_ESP.ESP_Box && collider != null)
                                        updateObject.box = Tools.BoundsToScreenRect(collider.bounds);

                                    MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.STORAGE, updateObject.screenPosition, updateObject.box, updateObject.color));
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Sentries
                        if (updateObject.type == EGOUpdate.SENTRY && MP_ESP.ESP_Enabled && MP_ESP.ESP_Sentrys_Enabled)
                        {
                            InteractableSentry sentry = (InteractableSentry)updateObject.instance;
                            int distance = (int)Tools.getDistance(updateObject.gameObject.transform.position);

                            #region ESP
                            if (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance)
                            {
                                updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                updateObject.text = "";
                                updateObject.color = MP_ESP.ESP_Sentrys_Color.selectedColor;

                                if (updateObject.screenPosition.z > 0)
                                {
                                    Collider collider = null;

                                    if (MP_ESP.ESP_Box)
                                        collider = updateObject.gameObject.GetComponent<Collider>();
                                    updateObject.screenPosition.x -= 64f;
                                    updateObject.screenPosition.y = (Screen.height - (updateObject.screenPosition.y + 1f)) - 12f;

                                    if (MP_ESP.ESP_ShowNames)
                                        updateObject.text += "Storage\n";
                                    if (MP_ESP.ESP_ShowDistances)
                                        updateObject.text += "Distance: " + distance + "\n";
                                    if (MP_ESP.ESP_Box && collider != null)
                                        updateObject.box = Tools.BoundsToScreenRect(collider.bounds);

                                    MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.SENTRY, updateObject.screenPosition, updateObject.box, updateObject.color));
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }

                if (MP_Aimbot.aimbot || MP_Aimbot.silentAim)
                {
                    MP_Aimbot.attackNext = aim_nextTarget;
                }
                yield return wfs;
            }
        }
        #endregion
    }
}