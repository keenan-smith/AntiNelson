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
        #region Properties
        public override string command
        {
            get
            {
                return "godmode";
            }
        }

        public override string[] alias
        {
            get
            {
                return new string[]
                {
                    "god",
                };
            }
        }

        public override string permission
        {
            get
            {
                return "godmode";
            }
        }
        #endregion

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {

        }
        #endregion
    }
}
