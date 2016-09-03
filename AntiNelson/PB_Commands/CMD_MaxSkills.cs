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
        #region Properties
        public override string command
        {
            get
            {
                return "maxskills";
            }
        }

        public override string[] alias
        {
            get
            {
                return new string[]
                {
                    "skills",
                };
            }
        }

        public override string permission
        {
            get
            {
                return "maxskills";
            }
        }
        #endregion

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            PBSKills.Agriculture.setMaxLevel(player);
            PBSKills.Cardio.setMaxLevel(player);
            PBSKills.Cooking.setMaxLevel(player);
            PBSKills.Crafting.setMaxLevel(player);
            PBSKills.Dexterity.setMaxLevel(player);
            PBSKills.Diving.setMaxLevel(player);
            PBSKills.Engineer.setMaxLevel(player);
            PBSKills.Exercise.setMaxLevel(player);
            PBSKills.Fishing.setMaxLevel(player);
            PBSKills.Healing.setMaxLevel(player);
            PBSKills.Immunity.setMaxLevel(player);
            PBSKills.Mechanic.setMaxLevel(player);
            PBSKills.Outdoors.setMaxLevel(player);
            PBSKills.Overkill.setMaxLevel(player);
            PBSKills.Parkour.setMaxLevel(player);
            PBSKills.SharpShooter.setMaxLevel(player);
            PBSKills.SneakyBeaky.setMaxLevel(player);
            PBSKills.Strength.setMaxLevel(player);
            PBSKills.Survival.setMaxLevel(player);
            PBSKills.Toughness.setMaxLevel(player);
            PBSKills.Vitality.setMaxLevel(player);
            PBSKills.WarmBlooded.setMaxLevel(player);
        }
        #endregion
    }
}
