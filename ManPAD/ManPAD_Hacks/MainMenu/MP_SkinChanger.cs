using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.GUIUtilitys;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using ManPAD.ManPAD_API.Types;
using SDG.Unturned;
using UnityEngine;
using SDG.Provider;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(3, "Skin Changer", 300f, 500f)]
    public class MP_SkinChanger : MenuOption
    {
        #region Variables
        private string searchText = "";

        public List<UnturnedEconInfo> skins = new List<UnturnedEconInfo>();
        #endregion

        #region Mono Functions
        public void Start()
        {
            foreach(UnturnedEconInfo skin in TempSteamworksEconomy.econInfo)
                if (!skin.type.Contains("Hat") & !skin.type.Contains("Glass") & !skin.type.Contains("Backpack") & !skin.type.Contains("Vest") & !skin.type.Contains("Mask") & !skin.type.Contains("Pants") & !skin.type.Contains("Shirt"))
                    skins.Add(skin);
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            searchText = GUILayout.TextField(searchText);
            foreach (UnturnedEconInfo skin in skins)
            {
                if (!string.IsNullOrEmpty(searchText) && !skin.name.ToLower().Contains(searchText.ToLower()))
                    continue;

                Color saveColor = GUI.color;

                if (Player.player.channel.owner.skins.ContainsValue(skin.itemdefid))
                    GUI.color = Color.green;
                else
                    GUI.color = Color.red;
                if (GUILayout.Button(skin.name))
                {
                    if (Player.player.channel.owner.skins.ContainsValue(skin.itemdefid))
                        Player.player.channel.owner.skins.Remove((ushort)skin.item_id);
                    else
                        Player.player.channel.owner.skins.Add((ushort)skin.item_id, skin.itemdefid);
                    if (Player.player.equipment.asset.id == (ushort)skin.item_id)
                        Player.player.equipment.dequip();
                }
                GUI.color = saveColor;
            }
        }
        #endregion
    }
}