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
    [Command("Default", "Group")]
    public class CMD_Group : PBCommand // Not done
    {
        public CMD_Group()
        {
            permission = "pointblank.group";
            command = "group";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            if (args.Length < 2)
            {
                player.sendChatMessage(localization.format("NoArguments"), Color.red);
                return;
            }
        }
        #endregion
    }
}
