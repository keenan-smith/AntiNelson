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
    public class lib_InfoUpdater : MonoBehaviour
    {
        #region Variables
        public List<GOUpdate> collections = new List<GOUpdate>();

        private WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        private WaitForSeconds wfs = new WaitForSeconds(0.015f);
        #endregion

        #region Mono Functions
        public void Start()
        {
            InvokeRepeating("reloadInfoInvoke", 5f, 5f);
            StartCoroutine(updateStuff());
        }

        public void Updatex1()
        {
            float aim_distanceAway = float.MaxValue;
            object aim_nextTarget = null;
            MP_ESP.draw.Clear();

            lock (collections)
            {
                for(int i = 0; i < collections.Count; i++)
                {
                    if (collections[i] == null || collections[i].gameObject == null || collections[i].instance == null)
                        continue;

                    GOUpdate updateObject = collections[i];
                    float distance = (float)Math.Round(Tools.getDistance(updateObject.gameObject.transform.position));

                    /*#region Players
                    if (updateObject.type == EGOUpdate.PLAYER && !((Player)updateObject.instance).life.isDead)
                    {
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
                        if (MP_ESP.ESP_Enabled && MP_ESP.ESP_Players_Enabled && (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance) && ((SteamPlayer)updateObject.instance).player == Player.player)
                        {
                            bool isFriend = (MP_Config.instance.getFriends() != null ? MP_Config.instance.getFriends().Contains(((SteamPlayer)updateObject.instance).playerID.steamID.m_SteamID) : false);

                            updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                            updateObject.text = "";
                            updateObject.color = (isFriend ? MP_ESP.ESP_Friends_Color.selectedColor : MP_ESP.ESP_Players_Color.selectedColor);

                            if (updateObject.screenPosition.z > 0 && (MP_ESP.ESP_Players_FilterFriends && !isFriend))
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
                        #endregion
                    }
                    #endregion*/

                    #region Zombies
                    if (updateObject.type == EGOUpdate.ZOMBIE && !((Zombie)updateObject.instance).isDead)
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

                for (int i = 0; i < ZombieManager.regions.Length; i++)
                    for (int a = 0; a < ZombieManager.regions[i].zombies.Count; a++)
                        collections.Add(new GOUpdate(ZombieManager.regions[i].zombies[a]));

                foreach (InteractableItem item in UnityEngine.Object.FindObjectsOfType(typeof(InteractableItem)) as InteractableItem[])
                    collections.Add(new GOUpdate(item));

                for (int i = 0; i < VehicleManager.vehicles.Count; i++)
                    collections.Add(new GOUpdate(VehicleManager.vehicles[i]));

                foreach (InteractableStorage storage in UnityEngine.Object.FindObjectsOfType(typeof(InteractableStorage)) as InteractableStorage[])
                    collections.Add(new GOUpdate(storage));

                foreach (InteractableSentry sentry in UnityEngine.Object.FindObjectsOfType(typeof(InteractableSentry)) as InteractableSentry[])
                    collections.Add(new GOUpdate(sentry));

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
                        float distance = (float)Math.Round(Tools.getDistance(updateObject.gameObject.transform.position));

                        /*#region Players
                        if (updateObject.type == EGOUpdate.PLAYER && !((Player)updateObject.instance).life.isDead)
                        {
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
                            if (MP_ESP.ESP_Enabled && MP_ESP.ESP_Players_Enabled && (distance <= MP_ESP.ESP_Distance || MP_ESP.ESP_IgnoreDistance) && ((SteamPlayer)updateObject.instance).player == Player.player)
                            {
                                bool isFriend = (MP_Config.instance.getFriends() != null ? MP_Config.instance.getFriends().Contains(((SteamPlayer)updateObject.instance).playerID.steamID.m_SteamID) : false);

                                updateObject.screenPosition = MainCamera.instance.WorldToScreenPoint(updateObject.gameObject.transform.position);
                                updateObject.text = "";
                                updateObject.color = (isFriend ? MP_ESP.ESP_Friends_Color.selectedColor : MP_ESP.ESP_Players_Color.selectedColor);

                                if (updateObject.screenPosition.z > 0 && (MP_ESP.ESP_Players_FilterFriends && !isFriend))
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
                            #endregion
                        }
                        #endregion*/

                        #region Zombies
                        if (updateObject.type == EGOUpdate.ZOMBIE && !((Zombie)updateObject.instance).isDead)
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

                yield return wfs;
            }
        }
        #endregion
    }
}
