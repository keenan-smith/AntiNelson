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
using Steamworks;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
#if DEBUG
    [MenuOption(100, "Item Filters(ic3)", 250f)]
    public class MP_iItemFilters : MenuOption
    {
        public static bool ItemFilter_Gun = false;
        public static bool ItemFilter_Ammo = false;
        public static bool ItemFilter_Medical = false;
        public static bool ItemFilter_FoodAndWater = false;
        public static bool ItemFilter_Backpack = false;
        public static bool ItemFilter_Charge = false;
        public static bool ItemFilter_Fuel = false;
        public static bool ItemFilter_Clothing = false;

        public static bool isItemWhitelisted(ItemAsset item)
        {
            if (ItemFilter_Gun && item is ItemGunAsset)
                return true;
            else if (ItemFilter_Ammo && item is ItemMagazineAsset)
                return true;
            else if (ItemFilter_Medical && item is ItemMedicalAsset)
                return true;
            else if (ItemFilter_FoodAndWater && (item is ItemFoodAsset || item is ItemWaterAsset))
                return true;
            else if (ItemFilter_Backpack && item is ItemBackpackAsset)
                return true;
            else if (ItemFilter_Charge && item is ItemChargeAsset)
                return true;
            else if (ItemFilter_Fuel && item is ItemFuelAsset)
                return true;
            else if (ItemFilter_Clothing && item is ItemClothingAsset)
                return true;
            return false;
        }

        public override void runGUI()
        {
            ItemFilter_Gun = GUILayout.Toggle(ItemFilter_Gun, "Guns");
            ItemFilter_Ammo = GUILayout.Toggle(ItemFilter_Ammo, "Ammo");
            ItemFilter_Medical = GUILayout.Toggle(ItemFilter_Medical, "Medical");
            ItemFilter_Backpack = GUILayout.Toggle(ItemFilter_Backpack, "Backpack");
            ItemFilter_Charge = GUILayout.Toggle(ItemFilter_Charge, "Charges");
            ItemFilter_Fuel = GUILayout.Toggle(ItemFilter_Fuel, "Fuel");
            ItemFilter_Clothing = GUILayout.Toggle(ItemFilter_Clothing, "Clothing");
            ItemFilter_FoodAndWater = GUILayout.Toggle(ItemFilter_FoodAndWater, "Food and Water");
        }
    }
#endif
}
