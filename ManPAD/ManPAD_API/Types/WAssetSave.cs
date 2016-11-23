using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_API.Types
{
    public class WAssetSave
    {
        public ItemWeaponAsset asset;

        public float recoilMax_x;
        public float recoilMax_y;
        public float recoilMin_x;
        public float recoilMin_y;

        public float spreadAim;
        public float spreadHip;

        public float shakeMax_x;
        public float shakeMax_y;
        public float shakeMax_z;
        public float shakeMin_x;
        public float shakeMin_y;
        public float shakeMin_z;

        public float range;

        public WAssetSave(ItemGunAsset asset)
        {
            this.asset = asset;

            recoilMax_x = asset.recoilMax_x;
            recoilMax_y = asset.recoilMax_y;
            recoilMin_x = asset.recoilMin_x;
            recoilMin_y = asset.recoilMin_y;

            spreadAim = asset.spreadAim;
            spreadHip = asset.spreadHip;

            shakeMax_x = asset.shakeMax_x;
            shakeMax_y = asset.shakeMax_y;
            shakeMax_z = asset.shakeMax_z;
            shakeMin_x = asset.shakeMin_x;
            shakeMin_y = asset.shakeMin_y;
            shakeMin_z = asset.shakeMin_z;

            range = asset.range;
        }

        public WAssetSave(ItemMeleeAsset asset)
        {
            range = asset.range;
        }
    }
}
