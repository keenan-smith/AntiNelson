using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Extensions;
using PointBlank.API.Attributes;
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
            foreach (PBSKills skill in player.skills)
            {
                skill.setMaxLevel(player);
            }
        }
        #endregion
    }
}
