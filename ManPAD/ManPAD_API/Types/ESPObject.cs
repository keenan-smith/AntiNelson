using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_Hacks.MainMenu;

namespace ManPAD.ManPAD_API.Types
{
    public class ESPObject : MonoBehaviour
    {
        #region Variables
        private WaitForEndOfFrame _wfeof = new WaitForEndOfFrame();

        public EESPItem espType;
        public Animal animal;
        public InteractableItem item;
        public SteamPlayer player;
        public InteractableSentry sentry;
        public InteractableStorage storage;
        public InteractableVehicle vehicle;
        public Zombie zombie;

        public Vector3 textPosition = new Vector3(0f, 0f, 0f);
        public Rect boxPosition = new Rect(0f, 0f, 0f, 0f);
        public Color color = Color.white;
        public string text = "";
        #endregion

        #region Mono Functions
        public void Start()
        {
            if (espType == EESPItem.ANIMAL)
                color = MP_ESP.ESP_Animals_Color.selectedColor;
            else if (espType == EESPItem.ITEM)
                color = MP_ESP.ESP_Items_Color.selectedColor;
            else if (espType == EESPItem.PLAYER)
                color = MP_ESP.ESP_Players_Color.selectedColor;
            else if (espType == EESPItem.SENTRY)
                color = MP_ESP.ESP_Sentrys_Color.selectedColor;
            else if (espType == EESPItem.STORAGE)
                color = MP_ESP.ESP_Storages_Color.selectedColor;
            else if (espType == EESPItem.VEHICLE)
                color = MP_ESP.ESP_Vehicles_Color.selectedColor;
            else if (espType == EESPItem.ZOMBIE)
                color = MP_ESP.ESP_Zombies_Color.selectedColor;
            else
                GameObject.Destroy(this);
            StartCoroutine(updateSystem());
        }

        public void FixedUpdate()
        {
            if (Tools.getDistance(transform.position) > MP_ESP.ESP_Distance && !MP_ESP.ESP_IgnoreDistance)
                GameObject.Destroy(this);
            if (espType == EESPItem.ANIMAL)
            {
                if (!MP_ESP.ESP_Animals_Enabled || !MP_ESP.ESP_Enabled)
                    GameObject.Destroy(this);
                if (animal.isDead)
                    GameObject.Destroy(this);
            }
            else if (espType == EESPItem.ITEM)
            {
                bool on;

                if (!MP_ESP.ESP_Items_Enabled || !MP_ESP.ESP_Enabled)
                    GameObject.Destroy(this);
                if (!MP_ESP.ESP_Items_Types.filter.TryGetValue(item.asset.type, out on))
                    on = false;
                if (!on)
                    GameObject.Destroy(this);
            }
            else if (espType == EESPItem.PLAYER)
            {
                bool isFriend = (MP_Config.instance.getFriends() != null ? MP_Config.instance.getFriends().Contains(player.playerID.steamID.m_SteamID) : false);

                if (!MP_ESP.ESP_Players_Enabled || !MP_ESP.ESP_Enabled)
                    GameObject.Destroy(this);
                if (MP_ESP.ESP_Players_FilterFriends && isFriend)
                    GameObject.Destroy(this);
            }
            else if (espType == EESPItem.STORAGE)
            {
                if (!MP_ESP.ESP_Storages_Enabled || !MP_ESP.ESP_Enabled)
                    GameObject.Destroy(this);
                if (MP_ESP.ESP_Storages_IgnoreLocked && !storage.checkUseable())
                    GameObject.Destroy(this);
            }
            else if (espType == EESPItem.VEHICLE)
            {
                if (!MP_ESP.ESP_Vehicles_Enabled || !MP_ESP.ESP_Enabled)
                    GameObject.Destroy(this);
                if (MP_ESP.ESP_Vehicles_IgnoreDestroyed && (vehicle.isDead || vehicle.isDrowned))
                    GameObject.Destroy(this);
                if (MP_ESP.ESP_Vehicles_IgnoreEmpty && vehicle.fuel < 1)
                    GameObject.Destroy(this);
                if (MP_ESP.ESP_Vehicles_IgnoreLocked && vehicle.isLocked)
                    GameObject.Destroy(this);
            }
            else if (espType == EESPItem.ZOMBIE)
            {
                if (!MP_ESP.ESP_Zombies_Enabled || !MP_ESP.ESP_Enabled)
                    GameObject.Destroy(this);
                if (zombie.isDead)
                    GameObject.Destroy(this);
            }
            else if (espType == EESPItem.SENTRY)
            {
                if (!MP_ESP.ESP_Sentrys_Enabled || !MP_ESP.ESP_Enabled)
                    GameObject.Destroy(this);
            }
            else
            {
                GameObject.Destroy(this);
            }
        }
        #endregion

        #region Coroutines
        public IEnumerator updateSystem()
        {
            while (true)
            {
                float distance = Tools.getDistance(transform.position);
                textPosition = MainCamera.instance.WorldToScreenPoint(transform.position);
                text = "";

                if (espType == EESPItem.PLAYER)
                    boxPosition = Tools.BoundsToScreenRect(new Bounds(transform.position + new Vector3(0, 1.1f, 0), transform.localScale + new Vector3(0, .95f, 0)));
                else
                    boxPosition = Tools.BoundsToScreenRect(gameObject.GetComponent<Collider>().bounds);

                if (textPosition.z <= 0)
                    yield return _wfeof;
                textPosition.x -= 64f;
                textPosition.y = (Screen.height - (textPosition.y + 1f)) - 12f;

                if (MP_ESP.ESP_ShowNames)
                    if (espType == EESPItem.ANIMAL)
                        text += animal.asset.animalName + "\n";
                    else if (espType == EESPItem.ITEM)
                        text += item.asset.itemName + "\n";
                    else if (espType == EESPItem.PLAYER)
                        text += player.playerID.characterName + "\n";
                    else if (espType == EESPItem.SENTRY)
                        text += "Sentry\n";
                    else if (espType == EESPItem.STORAGE)
                        text += "Storage\n";
                    else if (espType == EESPItem.VEHICLE)
                        text += vehicle.asset.vehicleName + "\n";
                    else if (espType == EESPItem.ZOMBIE)
                        text += Tools.getZombieName(zombie) + "\n";
                    else
                        GameObject.Destroy(this);
                if (MP_ESP.ESP_ShowDistances)
                    text += "Distance: " + distance + "\n";
                if (espType == EESPItem.PLAYER)
                {
                    if (MP_ESP.ESP_Players_ShowWeapons)
                        text += "Weapon: " + (player.player.equipment.asset == null ? "None" : player.player.equipment.asset.itemName) + "\n";
                    if (MP_ESP.ESP_Players_ShowIsAdmin)
                        text += "Is Admin: " + (player.isAdmin ? "Yes" : "No") + "\n";
                }
                else if (espType == EESPItem.VEHICLE)
                {
                    if (MP_ESP.ESP_Vehicles_ShowFuel)
                        text += "Fuel: " + vehicle.fuel + "\n";
                    if (MP_ESP.ESP_Vehicles_ShowLocked)
                        text += "Locked: " + (vehicle.isLocked ? "Yes" : "No") + "\n";
                }
                else if (espType == EESPItem.STORAGE)
                {
                    if (MP_ESP.ESP_Storages_ShowLocked)
                        text += "Locked: " + (storage.checkUseable() ? "No" : "Yes") + "\n";
                }

                yield return _wfeof;
            }
        }
        #endregion
    }
}
