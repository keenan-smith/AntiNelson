using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PrefixAndSuffix
{
    [Plugin("Prefix And Suffix", "AtiLion", false, false)]
    public class Main : PBPlugin
    {
        public override void onLoad()
        {
            
        }

        public void Event_ClientPreConnect(SteamPlayerID playerID, Vector3 point, byte angle, bool isPro, bool isAdmin, int channel, byte face, byte hair, byte beard, Color skin, Color color, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, EPlayerSkillset skillset)
        {
            
        }
    }
}
