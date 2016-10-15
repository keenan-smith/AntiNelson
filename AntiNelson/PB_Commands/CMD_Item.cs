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
    [Command("Default", "ItemCommand")]
    internal class CMD_Item : PBCommand
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
            if (args.Length < 1)
            {
                player.sendChatMessage(localization.format("NoArguments"), Color.red);
                return;
            }

            ushort id;
            byte amount;
            byte quality;
            if (!ushort.TryParse(args[0], out id))
            {
                PBChat.sendChatToPlayer(player, localization.format("InvalidItemID"), Color.red);
                return;
            }
            if (args.Length < 2 || args[1] == null || !byte.TryParse(args[1], out amount))
                amount = 1;
            if (args.Length < 3 || args[2] == null || !byte.TryParse(args[2], out quality))
                quality = 100;

            PBPlayer orgPlayer = player;
            if (args.Length > 3)
                player = PBServer.findPlayer(args[3]);
            if (player == null)
            {
                orgPlayer.sendChatMessage(localization.format("InvalidPlayer"), Color.red);
                return;
            }
            try
            {
                player.giveItem(id, amount, quality);
                orgPlayer.sendChatMessage(localization.format("GiveSuccess"), Color.magenta);
            }
            catch (Exception ex)
            {
                orgPlayer.sendChatMessage(localization.format("GiveFail"), Color.magenta);
                PBLogging.logError("ERROR: Item give fail!", ex, false);
            }
        }
        #endregion
    }
}
