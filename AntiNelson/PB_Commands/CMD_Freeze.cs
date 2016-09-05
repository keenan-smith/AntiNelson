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
    [Command("Default", "FreezeCommand")]
    public class CMD_Freeze : PBCommand // NOT DONE FINISH LATER
    {
        public CMD_Freeze()
        {
            permission = "freeze";
            command = "freeze";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
        }
        #endregion
    }
}
