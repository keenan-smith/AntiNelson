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
    [Command("Default", "HelpCommand")]
    public class CMD_Help : PBCommand
    {
        public CMD_Help()
        {
            permission = "";
            command = "help";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                string msg = "";
                foreach (PBCommand command in PBServer.commands)
                {
                    msg = msg + command.command + " - " + command.help + "\n";
                }
                player.sendChatMessage(msg, Color.magenta);
                return;
            }

            PBCommand cmd = PBServer.findCommand(args[0]);
            if (cmd != null && cmd.usage != null)
                player.sendChatMessage(cmd.help, Color.magenta);
        }
        #endregion
    }
}
