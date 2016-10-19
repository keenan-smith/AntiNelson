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
    internal class CMD_Help : PBCommand
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
            int page = 1;
            if (args.Length < 1 || int.TryParse(args[0], out page))
            {
                string[] commands = new string[PBServer.commands.Count + Commander.commands.Count];
                int index = 0;
                foreach (PBCommand command in PBServer.commands)
                {
                    commands[index] = command.command + " - " + command.help;
                    index++;
                }
                foreach (Command uCommand in Commander.commands)
                {
                    commands[index] = uCommand.command + " - " + uCommand.help;
                    index++;
                }
                for (int i = (page * 5) - 5; i < page * 5; i++)
                {
                    if (i >= commands.Length)
                        break;

                    player.sendChatMessage(commands[i], Color.magenta);
                }
                player.sendChatMessage("Page: " + page + "/" + Math.Floor((double)(commands.Length / 5)), Color.magenta);
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
