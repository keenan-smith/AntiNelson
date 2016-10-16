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
    [Command("Default", "PointBlankCommand")]
    internal class CMD_PointBlank : PBCommand
    {
        public CMD_PointBlank()
        {
            permission = "pointblank.pointblank";
            command = "pointblank";
            alias = new string[]{
                "pb"
            };
        }

        #region Functions
        public override void onCall(PBPlayer player, string[] args)
        {
            if (args.Length <= 0)
            {
                player.sendChatMessage(localization.format("NotEnoughArgs"), Color.magenta);
                return;
            }
            if (args[0].ToLower() == "reload")
            {
                if(PBServer.reloadPlugins())
                    player.sendChatMessage(localization.format("ReloadedPlugins"), Color.magenta);
                else
                    player.sendChatMessage(localization.format("ReloadedPluginsFail"), Color.magenta);
            }
            else if (args[0].ToLower() == "load")
            {
                if (args.Length <= 1)
                {
                    player.sendChatMessage(localization.format("NotEnoughArgs"), Color.magenta);
                    return;
                }
                if(PBServer.loadPlugin(args[1]))
                    player.sendChatMessage(localization.format("LoadedPlugin"), Color.magenta);
                else
                    player.sendChatMessage(localization.format("LoadedPluginFail"), Color.magenta);
            }
            else if (args[0].ToLower() == "unload")
            {
                if(PBServer.unloadPlugins())
                    player.sendChatMessage(localization.format("UnloadedPlugins"), Color.magenta);
                else
                    player.sendChatMessage(localization.format("UnloadedPluginsFail"), Color.magenta);
            }
            else if (args[0].ToLower() == "restart")
            {
                PBServer.restart();
            }
            else
            {
                player.sendChatMessage(localization.format("InvalidArg"), Color.magenta);
            }
        }
        #endregion
    }
}
