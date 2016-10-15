using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server;
using PointBlank.API;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.PB_Commands
{
    [Command("Default", "Permission")]
    internal class CMD_Permission : PBCommand
    {
        public CMD_Permission()
        {
            permission = "pointblank.permission";
            command = "permission";
            alias = new string[]
            {
                "p",
            };
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            PBPlayer orgPlayer = player;

            if (args.Length < 2)
            {
                orgPlayer.sendChatMessage(localization.format("NoArguments"), Color.red);
                return;
            }

            string action = args[0];
            string permission = args[1];
            if (args.Length >= 3)
                player = PBServer.findPlayer(args[2]);

            if (action.ToLower() == "set")
            {
                if (!player.hasPermission(permission))
                {
                    player.permissions.Add(permission);
                    orgPlayer.sendChatMessage(localization.format("PermissionSet"), Color.magenta);
                }
                else
                {
                    orgPlayer.sendChatMessage(localization.format("PermissionNoSet"), Color.magenta);
                }
            }
            else if (action.ToLower() == "del")
            {
                if (Array.Exists(player.permissions.ToArray(), a => a == permission))
                {
                    player.permissions.Remove(permission);
                    orgPlayer.sendChatMessage(localization.format("PermissionRem"), Color.magenta);
                }
                else
                {
                    orgPlayer.sendChatMessage(localization.format("PermissionNoRem"), Color.magenta);
                }
            }
            else if (action.ToLower() == "get")
            {
                if (player.hasPermission(permission))
                    orgPlayer.sendChatMessage(localization.format("TruePermission"), Color.magenta);
                else
                    orgPlayer.sendChatMessage(localization.format("FalsePermission"), Color.magenta);
            }
            else
            {
                orgPlayer.sendChatMessage(localization.format("InvalidAction"), Color.red);
                return;
            }
        }
        #endregion
    }
}
