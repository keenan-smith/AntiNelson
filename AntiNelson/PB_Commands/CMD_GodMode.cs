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
    [Command("Default", "GodMode")]
    internal class CMD_GodMode : PBCommand
    {
        public CMD_GodMode()
        {
            permission = "godmode.god";
            command = "godmode";
            alias = new string[]
            {
                "god",
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
            if (godmode == null || !(bool)godmode)
            {
                player.setCustomVariable("GodMode", true);
                orgPlayer.sendChatMessage(localization.format("GodmodeSuccess"), Color.magenta);
                player.sendChatMessage(localization.format("Godmoded"), Color.magenta);
            }
            else
            {
                orgPlayer.sendChatMessage(localization.format("GodmodeFail"), Color.magenta);
            }
        }

        public void Update()
        {
            foreach (PBPlayer player in PBServer.players)
            {
                object god = player.getCustomVariable("GodMode");
                if (god != null && (bool)god)
                {
                    player.player.life.askBreath(100);
                    player.player.life.askDrink(100);
                    player.player.life.askEat(100);
                    player.player.life.askHeal(100, true, true);
                    player.player.life.askDisinfect(100);
                    player.player.life.askRest(100);
                }
            }
        }
        #endregion
    }
}
