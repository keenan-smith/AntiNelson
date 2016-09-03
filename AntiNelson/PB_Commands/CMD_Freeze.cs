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
        #region Properties
        public override string command
        {
            get
            {
                return "freeze";
            }
        }

        public override string[] alias
        {
            get
            {
                return new string[0];
            }
        }

        public override string permission
        {
            get
            {
                return "freeze";
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
