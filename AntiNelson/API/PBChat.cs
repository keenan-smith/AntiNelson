using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

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

        #region Handlers
        public static delegate void ChatMessageHandler(CSteamID speaker, Chat args);
        #endregion

        #region Events
        public static event ChatMessageHandler OnMessageReceived;
        #endregion

        #region Event Functions
        public static void ProcessCommands(CSteamID speaker, Chat args) // NOT DONE!
        {
            string prefix = args.text.Substring(0, 1);
            if (prefix == "@" || prefix == "/")
            {
                // NEED FRAMEWORK TO FINISH!
            }
        }
        #endregion

        #region Override Functions
        public static void askChat(CSteamID steamID, byte mode, string text)
        {
            if (OnMessageReceived == null)
                return;

            SteamPlayer player = PlayerTool.getSteamPlayer(steamID);
            if (player == null)
		    {
			    return;
		    }
		    if (text.Length < 2)
		    {
			    return;
		    }
		    if (text.Length > ChatManager.LENGTH)
		    {
			    text = text.Substring(0, ChatManager.LENGTH);
		    }
		    text = text.Trim();
		    if (mode == 0)
		    {
			    if (CommandWindow.shouldLogChat)
			    {
				    CommandWindow.Log(Provider.localization.format("Global", new object[]
				    {
					    player.playerID.characterName,
					    player.playerID.playerName,
					    text
				    }));
			    }
		    }
		    else if (mode == 1)
		    {
			    if (CommandWindow.shouldLogChat)
			    {
				    CommandWindow.Log(Provider.localization.format("Local", new object[]
				    {
					    player.playerID.characterName,
					    player.playerID.playerName,
					    text
				    }));
			    }
		    }
		    else
		    {
			    if (mode != 2)
			    {
				    return;
			    }
			    if (CommandWindow.shouldLogChat)
			    {
				    CommandWindow.Log(Provider.localization.format("Group", new object[]
				    {
					    player.playerID.characterName,
					    player.playerID.playerName,
					    text
				    }));
			    }
		    }
		    Color color = Color.white;
            if (player.isAdmin && !Provider.hideAdmins)
		    {
			    color = Palette.ADMIN;
		    }
            else if (player.isPro)
		    {
			    color = Palette.PRO;
		    }
		    bool flag = true;
		    if (ChatManager.onChatted != null)
		    {
                ChatManager.onChatted(player, (EChatMode)mode, ref color, text, ref flag);
		    }
            
            Chat args = new Chat((EChatMode)mode, color, player.playerID.playerName, text);
            OnMessageReceived(steamID, args);

            if (args == null)
                return;
            ChatManager manager = (ChatManager)typeof(ChatManager).GetField("manager", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            if (ChatManager.process(player, args.text) && flag)
            {
                if (Time.realtimeSinceStartup - player.lastChat < ChatManager.chatrate)
                {
                    return;
                }
                player.lastChat = Time.realtimeSinceStartup;
                if ((byte)args.mode == 0)
                {
                    manager.channel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				    {
					    steamID,
					    (byte)args.mode,
					    args.color,
					    args.text
				    });
                }
                else if ((byte)args.mode == 1)
                {
                    manager.channel.send("tellChat", ESteamCall.OTHERS, player.player.transform.position, EffectManager.MEDIUM, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				    {
					    steamID,
					    (byte)args.mode,
					    args.color,
					    args.text
				    });
                }
                else if ((byte)args.mode == 2 && player.playerID.group != CSteamID.Nil)
                {
                    for (int i = 0; i < Provider.clients.Count; i++)
                    {
                        SteamPlayer steamPlayer2 = Provider.clients[i];
                        if (steamPlayer2.playerID.group == player.playerID.group)
                        {
                            manager.channel.send("tellChat", steamPlayer2.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
						    {
							    steamID,
							    (byte)args.mode,
							    args.color,
							    args.text
						    });
                        }
                    }
                }
            }
        }
        #endregion
    }
}
