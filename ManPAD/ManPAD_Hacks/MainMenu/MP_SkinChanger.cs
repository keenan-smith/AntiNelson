using System;
using System.Collections;
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
        //private Vector2 scroll;

        public List<UnturnedEconInfo> Skinz;
        #endregion

        #region Mono Functions
        public void Start()
        {
            Skinz = new List<UnturnedEconInfo>();
            foreach(UnturnedEconInfo skin in TempSteamworksEconomy.econInfo)
            {
                if (!skin.type.Contains("Hat") & !skin.type.Contains("Glass") & !skin.type.Contains("Backpack") & !skin.type.Contains("Vest") & !skin.type.Contains("Mask") & !skin.type.Contains("Pants") & !skin.type.Contains("Shirt"))
                    Skinz.Add(skin);
            }
        }

        public void Update()
        {
            if (!Variables.isInGame)
                return;
        }

        public void OnGUI()
        {
            if (!Variables.isInGame)
                return;
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            searchText = GUILayout.TextField(searchText);
            for (int i = 0; i < Skinz.Count; i++)
            {
                UnturnedEconInfo skin = Skinz[i];
                bool isShown = false;
                if (string.IsNullOrEmpty(searchText))
                    isShown = true;
                else if (!string.IsNullOrEmpty(searchText) & skin.name.ToLower().Contains(searchText.ToLower()))
                {
                    isShown = true;
                }
                if (isShown)
                {
                    if (GUILayout.Button(skin.name))
                    {
                        ushort itemId = Provider.provider.economyService.getInventoryItemID(skin.itemdefid);
                        //if (Provider.provider.economyService.getInventoryType(skin.item_id))
                            //Player.player.channel.owner.skins.Remove(itemId);
                        Player.player.channel.owner.skins.Add(itemId, skin.itemdefid);
                    }
                }
            }
        }
        #endregion

        #region Coroutines
        #endregion
    }
}