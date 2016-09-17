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
            if (args.Length < 1)
            {
                foreach (PBCommand command in PBServer.commands)
                {
                    player.sendChatMessage(command.command + " - " + command.help, Color.magenta);
                }
                foreach (Command uCommand in Commander.commands)
                {
                    player.sendChatMessage(uCommand.command + " - " + uCommand.help, Color.magenta);
                }
                return;
            }

            PBCommand cmd = PBServer.findCommand(args[0]);
            if (cmd != null && cmd.usage != null)
            {
                player.sendChatMessage(cmd.help, Color.magenta);
            }
            else
            {
                Command uCmd = Array.Find(Commander.commands.ToArray(), a => a.command.ToLower() == args[0].ToLower());
                if (uCmd != null)
                {
                    player.sendChatMessage(uCmd.help, Color.magenta);
                }
            }
        }
        #endregion
    }
}
