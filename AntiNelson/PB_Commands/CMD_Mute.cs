using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PointBlank.PB_Commands
{
    [Command("Default", "MuteCommand")]
    public class CMD_Mute : PBCommand
    {
        public CMD_Mute()
        {
            permission = "mute";
            command = "mute";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            if (args.Length > 0)
            {
                player = PBServer.findPlayer(args[0]);
            }

            object bl = player.getCustomVariable("Muted");
            if (bl == null)
                player.customVariables.Add("Muted", true);
            else if (!(bool)bl)
                player.setCustomVariable("Muted", true);
        }

        public void Start()
        {
            PBChat.OnMessageReceived += new PBChat.ChatMessageHandler(ChatMute);
        }

        public void ChatMute(CSteamID speaker, Chat args)
        {
            if (args == null || args.text == null)
                return;

            PBPlayer player = PBServer.findPlayer(speaker);
            if (player != null)
            {
                object muted = player.getCustomVariable("Muted");
                if (muted != null && (bool)muted)
                {
                    args.text = null;
                }
            }
        }
        #endregion
    }
}
