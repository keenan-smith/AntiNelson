using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.PB_Commands
{
    [Command("Default", "MaxSkills")]
    public class CMD_MaxSkills : PBCommand
    {
        public CMD_MaxSkills()
        {
            permission = "maxskills";
            command = "maxskills";
            alias = new string[]
            {
                "skills",
            };
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

            foreach (PBSkills skill in player.skills)
            {
                skill.setMaxLevel(player);
            }
            orgPlayer.sendChatMessage(localization.format("MaxSkillSuccess"), Color.magenta);
        }
        #endregion
    }
}
