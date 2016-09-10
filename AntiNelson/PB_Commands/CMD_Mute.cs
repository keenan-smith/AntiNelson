using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
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
            permission = "mute.mute";
            command = "mute";
            alias = new string[0];
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            PBPlayer orgPlayer = player;
            if (!string.IsNullOrEmpty(args[0]))
                player = PBServer.findPlayer(args[0]);
            if (player == null)
            {
                orgPlayer.sendChatMessage(localization.format("InvalidPlayer"), Color.red);
                return;
            }

            object bl = player.getCustomVariable("Muted");
            if (bl == null)
                player.setCustomVariable("Muted", true);
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
