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
    [Command("Default", "ItemCommand")] // Plugin Name, Command Name
    public class CMD_Item : PBCommand
    {
        #region Propertys
        public override string command
        {
            get
            {
                return "item";
            }
        }

        public override string[] alias
        {
            get
            {
                return new string[]
                {
                    "i",
                };
            }
        }

        public override string permission
        {
            get
            {
                return "i";
            }
        }
        #endregion

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            ushort id;
            byte amount;
            byte quality;
            if (!ushort.TryParse(args[0], out id))
            {
                PBChat.sendChatToPlayer(player, localization.format("InvalidItemID"), Color.red);
                return;
            }
            if (args[1] == null || !byte.TryParse(args[1], out amount))
                amount = 1;
            if (args[2] == null || !byte.TryParse(args[2], out quality))
                quality = 255;
            player.giveItem(id, amount, quality);
        }
        #endregion
    }
}
