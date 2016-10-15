using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "IPCommand")]
    internal class CMD_IP : PBCommand
    {
        public CMD_IP()
        {
            permission = "ip";
            command = "ip";
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

            orgPlayer.sendChatMessage(Parser.getIPFromUInt32(player.IP), Color.magenta);
        }
        #endregion
    }
}
