using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.API
{
    public class PBChat
    {
        #region Functions
        public static void sendChatToPlayer(PBPlayer player, string message, Color color)
        {
            ChatManager.say(player.steamID, message, color, EChatMode.SAY);
        }
        #endregion
    }
}
