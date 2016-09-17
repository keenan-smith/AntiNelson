using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "UnGodMode")]
    public class CMD_UnGodMode : PBCommand
    {
        public CMD_UnGodMode()
        {
            permission = "godmode.ungod";
            command = "ungodmode";
            alias = new string[]
            {
                "ungod",
            };
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            PBPlayer orgPlayer = player;
            if (args.Length > 0)
                player = PBServer.findPlayer(args[0]);
            if (player == null)
            {
                orgPlayer.sendChatMessage(localization.format("InvalidPlayer"), Color.red);
                return;
            }

            object godmode = player.getCustomVariable("GodMode");
            if (godmode != null && (bool)godmode)
            {
                player.removeCustomVariable("GodMode");
                orgPlayer.sendChatMessage(localization.format("UnGodmodeSuccess"), Color.magenta);
                player.sendChatMessage(localization.format("UnGodmoded"), Color.magenta);
            }
            else
            {
                orgPlayer.sendChatMessage(localization.format("UnGodmodeFail"), Color.magenta);
            }
        }
        #endregion
    }
}
