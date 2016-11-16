using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using System.Reflection;
using Steamworks;

namespace ManPAD.ManPAD_Overridables
{
    internal class OV_Player : MonoBehaviour
    {
        [CodeReplace("askScreenshot", typeof(Player), BindingFlags.Instance | BindingFlags.Public)]
        public void askScreenshot(CSteamID id)
        {
            if (Player.player.channel.checkServer(id))
            {
                Player.player.StartCoroutine("takeScreenshot");
            }
        }
    }
}
