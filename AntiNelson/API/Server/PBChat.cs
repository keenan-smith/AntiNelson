using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PointBlank.API.Server.Extensions;

namespace PointBlank.API.Server
{
    public class PBChat
    {
        #region Functions
        /// <summary>
        /// Sends a chat to a specific player.
        /// </summary>
        /// <param name="player">The player instance.</param>
        /// <param name="message">The message you want to send.</param>
        /// <param name="color">The color of the message.</param>
        public static void sendChatToPlayer(PBPlayer player, string message, Color color)
        {
            ChatManager.say(player.steamID, message, color, EChatMode.SAY);
        }

        private static bool checkPermissions(PBPlayer player, string permission, string[] args)
        {
            string prm = permission;
            foreach (string arg in args)
            {
                prm = prm + "." + arg;
            }
            return (string.IsNullOrEmpty(permission) || player.hasPermission(prm));
        }
        #endregion

        #region Handlers
        public delegate void ChatMessageHandler(CSteamID speaker, Chat args);
        #endregion

        #region Events
        /// <summary>
        /// The event is called when a message is received.
        /// </summary>
        public static event ChatMessageHandler OnMessageReceived;
        #endregion

        #region Event Functions
        internal static void ProcessCommands(CSteamID speaker, Chat args)
        {
            if (args == null || args.text == null)
                return;

            string prefix = args.text.Substring(0, 1);
            if (prefix == "@" || prefix == "/")
            {
                string command = args.text.Substring(1, args.text.Length - 1);
                string[] info = command.Split(' ');
                PBCommand cmd = PBServer.findCommand(info[0]);
                if (cmd != null)
                {
                    PBLogging.log("Calling: " + info[0], false);
                    string cArgs = (info.Length > 1 ? info[1] : "");
                    try
                    {
                        cmd.execute(PBServer.findPlayer(speaker), cArgs);
                    }
                    catch (Exception ex)
                    {
                        PBLogging.logError("ERROR, while running command!", ex);
                    }
                    args.text = null;
                }
                else
                {
                    Command uCmd = Array.Find(Commander.commands.ToArray(), a => a.command.ToLower() == info[0].ToLower());
                    if (uCmd != null)
                    {
                        PBLogging.log("Calling: " + info[0], false);
                        string cArgs = (info.Length > 1 ? info[1] : "");
                        try
                        {
                            PBPlayer ply = PBServer.findPlayer(speaker);
                            if (checkPermissions(ply, "unturned." + info[0], cArgs.Split('/')))
                            {
                                uCmd.check(speaker, info[0].ToLower(), cArgs);
                            }
                        }
                        catch (Exception ex)
                        {
                            PBLogging.logError("ERROR, while running command!", ex);
                        }
                        args.text = null;
                    }
                }
            }
        }
        #endregion

        #region Override Functions
        internal static void askChat(CSteamID steamID, byte mode, string text)
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
            Chat args = new Chat(player, (EChatMode)mode, color, player.playerID.playerName, text);

            ProcessCommands(steamID, args);
            if (args == null || args.text == null)
                return;

            OnMessageReceived(steamID, args);
            if (args == null || args.text == null)
                return;

            ChatManager manager = (ChatManager)typeof(ChatManager).GetField("manager", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            if (flag)
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
