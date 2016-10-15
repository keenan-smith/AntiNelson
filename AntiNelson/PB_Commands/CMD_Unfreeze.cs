using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.PB_Commands
{
    [Command("Default", "UnfreezeCommand")]
    internal class CMD_Unfreeze : PBCommand
    {
        public CMD_Unfreeze()
        {
            permission = "freeze.unfreeze";
            command = "unfreeze";
            alias = new string[0];
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

            object frozen = player.getCustomVariable("Frozen");
            if (frozen != null && (bool)frozen)
            {
                player.removeCustomVariable("Frozen");
                player.removeCustomVariable("Frozen_position_x");
                player.removeCustomVariable("Frozen_position_y");
                player.removeCustomVariable("Frozen_position_z");
                orgPlayer.sendChatMessage(localization.format("UnfreezeSuccess"), Color.magenta);
                player.sendChatMessage(localization.format("Unfrozen"), Color.magenta);
            }
            else
            {
                orgPlayer.sendChatMessage(localization.format("UnfreezeFail"), Color.magenta);
            }
        }
        #endregion
    }
}
