using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Attributes;
using PointBlank.API.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "GodMode")]
    public class CMD_GodMode : PBCommand // NOT DONE FINISH LATER
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

        }
        #endregion
    }
}
