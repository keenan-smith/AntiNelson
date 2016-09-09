using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "UnmuteCommand")]
    public class CMD_Unmute : PBCommand
    {
        public CMD_Unmute()
        {
            permission = "mute";
            command = "unmute";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            if (args.Length > 0)
            {
                player = PBServer.findPlayer(args[0]);
            }

            if (player.getCustomVariable("Muted") != null)
                player.customVariables.Remove("Muted");
        }
        #endregion
    }
}
