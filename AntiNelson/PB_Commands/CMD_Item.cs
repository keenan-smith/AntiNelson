using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.PB_Commands
{
    [Command("Default", "ItemCommand")]
    public class CMD_Item : PBCommand
    {
        public CMD_Item()
        {
            permission = "item";
            command = "item";
            alias = new string[]
            {
                "i",
            };
        }

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

            PBPlayer orgPlayer = player;
            if (args.Length > 0)
                player = PBServer.findPlayer(args[0]);
            if (player == null)
            {
                orgPlayer.sendChatMessage(localization.format("InvalidPlayer"), Color.red);
                return;
            }

            player.giveItem(id, amount, quality);
        }
        #endregion
    }
}
