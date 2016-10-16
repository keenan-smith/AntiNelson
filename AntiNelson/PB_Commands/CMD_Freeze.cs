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
    [Command("Default", "FreezeCommand")]
    internal class CMD_Freeze : PBCommand
    {
        public CMD_Freeze()
        {
            permission = "pointblank.freeze.freeze";
            command = "freeze";
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
            if (frozen == null || !(bool)frozen)
            {
                player.setCustomVariable("Frozen", true);
                player.setCustomVariable("Frozen_position_x", player.player.transform.position.x);
                player.setCustomVariable("Frozen_position_y", player.player.transform.position.y);
                player.setCustomVariable("Frozen_position_z", player.player.transform.position.z);
                orgPlayer.sendChatMessage(localization.format("FreezeSuccess"), Color.magenta);
                player.sendChatMessage(localization.format("Frozen"), Color.magenta);
            }
            else
            {
                orgPlayer.sendChatMessage(localization.format("FreezeFail"), Color.magenta);
            }
        }

        public void Update()
        {
            foreach (PBPlayer player in PBServer.players)
            {
                object frozen = player.getCustomVariable("Frozen");
                if (frozen != null && (bool)frozen)
                {
                    player.player.transform.position = new Vector3((float)player.getCustomVariable("Frozen_position_x"), (float)player.getCustomVariable("Frozen_position_y"), (float)player.getCustomVariable("Frozen_position_z"));
                }
            }
        }
        #endregion
    }
}
