using System;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.PB_Library;
using UnityEngine;

namespace TestPlugin
{
    [Plugin("Test Plugin", "Kunii", false, true)]
    public class Main : PBPlugin
    {
        public override void onLoad()
        {
            PBLogging.log("Log test.");
            Console.WriteLine("Console test.");
            PBServer.OnPlayerJoin += new PBServer.ClientJoinHandler(onPlayerJoin);
            PBServer.OnPlayerLeave += new PBServer.ClientLeaveHandler(onPlayerLeave);
            PBServer.addPrefix(76561198073993164, "Admin");
            PBServer.addSuffix(76561198073993164, "Fag");
        }

        public void onPlayerLeave(PBPlayer player)
        {
            PBServer.broadcastChat(player.playerID.characterName + " has left the server.", Color.red);
            PBLogging.log(player.playerID.characterName + " has left the server.");
        }

        public void onPlayerJoin(PBPlayer player)
        {
            PBServer.broadcastChat(player.playerID.characterName + " has joined the server.", Color.red);
            PBLogging.log(player.playerID.characterName + " has joined the server.");
        }
    }
}
