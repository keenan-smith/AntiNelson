using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "UnmuteCommand")]
    public class CMD_Unmute : PBCommand
    {
        public CMD_Unmute()
        {
            permission = "mute.unmute";
            command = "unmute";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            PBPlayer orgPlayer = player;
            if (!string.IsNullOrEmpty(args[0]))
                player = PBServer.findPlayer(args[0]);
            if (player == null)
            {
                orgPlayer.sendChatMessage(localization.format("InvalidPlayer"), Color.red);
                return;
            }

            if (player.getCustomVariable("Muted") != null)
                player.removeCustomVariable("Muted");
        }
        #endregion
    }
}
