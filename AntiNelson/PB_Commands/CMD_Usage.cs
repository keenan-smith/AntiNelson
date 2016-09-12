using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "UsageCommand")]
    public class CMD_Usage : PBCommand
    {
        public CMD_Usage()
        {
            permission = "";
            command = "usage";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            if (args.Length < 1)
            {
                player.sendChatMessage(localization.format("NoArguments"), Color.red);
                return;
            }

            PBCommand cmd = PBServer.findCommand(args[0]);
            if (cmd != null && cmd.usage != null)
                player.sendChatMessage(cmd.usage, Color.magenta);
        }
        #endregion
    }
}
