﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API.Types;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using SDG.Unturned;
using UnityEngine;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(5, "Weapons", 300f)]
    public class MP_Weapons : MenuOption
    {
        #region Variables
        private Dictionary<byte[], WAssetSave> _backup = new Dictionary<byte[], WAssetSave>();

        public static bool norecoil = true;
        public static bool nospread = true;
        public static bool noshake = true;
        public static bool nosway = true;
        public static bool attackThroughWalls = false;
        public static bool farReach = false;
        public static bool alwaysHitHead = false;
        #endregion

        #region Mono Functions
        public void Update()
        {
            if (!Variables.isInGame)
                return;

            #region No Recoil, No Spread, No Shake, No Sway, Far Reach
            if (Player.player.equipment.isSelected && Player.player.equipment.asset != null && Player.player.equipment.useable != null && Player.player.equipment.asset is ItemWeaponAsset)
            {
                byte[] hash = Player.player.equipment.asset.hash;
                if ((norecoil || nospread || noshake || farReach) && !_backup.ContainsKey(hash))
                {
                    if (Player.player.equipment.asset is ItemGunAsset)
                        _backup.Add(hash, new WAssetSave((ItemGunAsset)Player.player.equipment.asset));
                    else
                        _backup.Add(hash, new WAssetSave((ItemMeleeAsset)Player.player.equipment.asset));
                }

                if (Player.player.equipment.asset is ItemGunAsset)
                {
                    if (norecoil)
                        setRecoil((ItemGunAsset)Player.player.equipment.asset, 0f, 0f, 0f, 0f);
                    else
                        setRecoil((ItemGunAsset)Player.player.equipment.asset, _backup[hash].recoilMax_x, _backup[hash].recoilMax_y, _backup[hash].recoilMin_x, _backup[hash].recoilMin_y);

                    if (nospread && !Variables.isSpying)
                        setSpread((ItemGunAsset)Player.player.equipment.asset, 0f, 0f);
                    else
                        setSpread((ItemGunAsset)Player.player.equipment.asset, _backup[hash].spreadAim, _backup[hash].spreadHip);

                    if (noshake)
                        setShake((ItemGunAsset)Player.player.equipment.asset, 0f, 0f, 0f, 0f, 0f, 0f);
                    else
                        setShake((ItemGunAsset)Player.player.equipment.asset, _backup[hash].shakeMax_x, _backup[hash].shakeMax_y, _backup[hash].shakeMax_z, _backup[hash].shakeMin_x, _backup[hash].shakeMin_y, _backup[hash].shakeMin_z);
                }

                if (Player.player.equipment.useable is UseableGun)
                {
                    if (nosway)
                        Player.player.animator.viewSway = Vector3.zero;

                    PlayerUI.updateCrosshair(((ItemGunAsset)Player.player.equipment.asset).spreadHip);
                }

                if (Player.player.equipment.asset is ItemMeleeAsset)
                {
                    if (farReach)
                        setRange((ItemWeaponAsset)Player.player.equipment.asset, 26f);
                    else
                        setRange((ItemWeaponAsset)Player.player.equipment.asset, _backup[hash].range);
                }
            }
            #endregion
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            norecoil = GUILayout.Toggle(norecoil, "No Recoil");
            noshake = GUILayout.Toggle(noshake, "No Shake");
            nospread = GUILayout.Toggle(nospread, "No Spread");
            nosway = GUILayout.Toggle(nosway, "No Sway");
            farReach = GUILayout.Toggle(farReach, "Far Reach");
#if !FREE
            attackThroughWalls = GUILayout.Toggle(attackThroughWalls, "Attack Through Walls");
            alwaysHitHead = GUILayout.Toggle(alwaysHitHead, "Always Hit Head");
#endif
        }

        private void setRecoil(ItemGunAsset asset, float maxX, float maxY, float minX, float minY)
        {
            asset.recoilMax_x = maxX;
            asset.recoilMax_y = maxY;
            asset.recoilMin_x = minX;
            asset.recoilMin_y = minY;
        }

        private void setSpread(ItemGunAsset asset, float aim, float hip)
        {
            asset.spreadAim = aim;
            asset.spreadHip = hip;
        }

        private void setShake(ItemGunAsset asset, float maxX, float maxY, float maxZ, float minX, float minY, float minZ)
        {
            asset.shakeMax_x = maxX;
            asset.shakeMax_y = maxY;
            asset.shakeMax_z = maxZ;
            asset.shakeMin_x = minX;
            asset.shakeMin_y = minY;
            asset.shakeMin_z = minZ;
        }

        private void setRange(ItemWeaponAsset asset, float range)
        {
            asset.range = range;
        }
        #endregion
    }
}
