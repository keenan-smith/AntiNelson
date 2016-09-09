using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "GodMode")]
    public class CMD_GodMode : PBCommand // NOT FINISHED
    {
        public CMD_GodMode()
        {
            permission = "godmode";
            command = "godmode";
            alias = new string[]
            {
                "god",
            };
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            if (args[0] != null)
            {
            }
            player.customVariables.Add("GodMode", true);
        }

        public void Update()
        {
            foreach (PBPlayer player in PBServer.players)
            {
                object god = player.getCustomVariable("GodMode");
                if (god != null && (bool)god)
                {
                    player.player.life.askBreath(100);
                    player.player.life.askDrink(100);
                    player.player.life.askEat(100);
                    player.player.life.askHeal(100, true, true);
                    player.player.life.askDisinfect(100);
                }
            }
        }
        #endregion
    }
}
