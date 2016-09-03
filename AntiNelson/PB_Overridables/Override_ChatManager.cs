using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API;
using PointBlank.API.Attributes;

namespace PointBlank.PB_Overridables
{
    public class Override_ChatManager
    {
        [ReplaceCode(typeof(ChatManager), "askChat", BindingFlags.Public | BindingFlags.Instance)]
        [SteamCall]
        public void askChat(CSteamID steamID, byte mode, string text)
        {
            if (Provider.isServer)
            {
                PBChat.askChat(steamID, mode, text);
            }
        }
    }
}
