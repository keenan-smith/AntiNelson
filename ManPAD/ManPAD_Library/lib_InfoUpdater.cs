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

namespace ManPAD.ManPAD_Library
{
    public class lib_InfoUpdater : MonoBehaviour
    {
        #region Variables
        public List<GOUpdate> collections = new List<GOUpdate>();
        #endregion

        #region Mono Functions
        public void Start()
        {
            InvokeRepeating("reloadInfoInvoke", 5f, 5f);
        }

        public void Update()
        {
            float aim_distanceAway = float.MaxValue;
            object aim_nextTarget = null;

            lock (collections)
            {
                Debug.Log(collections.Count);
                for(int i = 0; i < collections.Count; i++)
                {
                    GOUpdate updateObject = collections[i];
                    if (updateObject == null || updateObject.gameObject == null || updateObject.instance == null)
                        continue;

                    float distance = (float)Math.Round(Tools.getDistance(updateObject.gameObject.transform.position));
                    
                    #region Zombies
                    if (updateObject.type == EGOUpdate.ZOMBIE)
                    {
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
                        MP_ESP.clearDraw(EESPItem.ZOMBIE);
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
                                    updateObject.text += distance + "\n";
                                if (MP_ESP.ESP_Box && collider != null)
                                    updateObject.box = Tools.BoundsToScreenRect(collider.bounds);

                                MP_ESP.draw.Add(new ESPDraw(updateObject.text, updateObject.gameObject, EESPItem.ZOMBIE, updateObject.screenPosition, updateObject.box, updateObject.color));
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
            }

            if (MP_Aimbot.aimbot || MP_Aimbot.silentAim)
                MP_Aimbot.attackNext = aim_nextTarget;
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

                collections.Clear();

                for (int i = 0; i < Provider.clients.Count; i++)
                    collections.Add(new GOUpdate(Provider.clients[i]));
                //Variables.players = Provider.clients.ToArray();

                //List<Zombie> temp = new List<Zombie>();
                lock (ZombieManager.regions)
                    for (int i = 0; i < ZombieManager.regions.Length; i++)
                        for (int a = 0; a < ZombieManager.regions[i].zombies.Count; a++)
                            collections.Add(new GOUpdate(ZombieManager.regions[i].zombies[a]));
                //temp.AddRange(ZombieManager.regions[i].zombies);
                //Variables.zombies = temp.ToArray();

                foreach (InteractableItem item in UnityEngine.Object.FindObjectsOfType(typeof(InteractableItem)) as InteractableItem[])
                    collections.Add(new GOUpdate(item));
                //Variables.items = UnityEngine.Object.FindObjectsOfType(typeof(InteractableItem)) as InteractableItem[];

                for (int i = 0; i < VehicleManager.vehicles.Count; i++)
                    collections.Add(new GOUpdate(VehicleManager.vehicles[i]));
                //Variables.vehicles = VehicleManager.vehicles.ToArray();

                foreach (InteractableStorage storage in UnityEngine.Object.FindObjectsOfType(typeof(InteractableStorage)) as InteractableStorage[])
                    collections.Add(new GOUpdate(storage));
                //Variables.storages = UnityEngine.Object.FindObjectsOfType(typeof(InteractableStorage)) as InteractableStorage[];

                foreach (InteractableSentry sentry in UnityEngine.Object.FindObjectsOfType(typeof(InteractableSentry)) as InteractableSentry[])
                    collections.Add(new GOUpdate(sentry));
                //Variables.sentrys = UnityEngine.Object.FindObjectsOfType(typeof(InteractableSentry)) as InteractableSentry[];

                for (int i = 0; i < AnimalManager.animals.Count; i++)
                    collections.Add(new GOUpdate(AnimalManager.animals[i]));
                //Variables.animals = AnimalManager.animals.ToArray();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        #endregion
    }
}
