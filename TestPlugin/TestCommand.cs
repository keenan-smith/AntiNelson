using UnityEngine;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;
using PointBlank.API;

namespace TestPlugin
{
    [Command("Test Plugin", "testcommand")]
    public class TestCommand : PBCommand
    {
        public TestCommand()
        {
            permission = "";
            command = "testcommand";
            alias = new string[0];
        }

        public override void onCall(PBPlayer player, string[] args)
        {
            if (player.getCustomVariable("test") == null)
                player.setCustomVariable("test", 0, true);
            else player.setCustomVariable("test", ((int)player.getCustomVariable("test"))+1, true);


            player.sendChatMessage("Set local var: " + ((int)player.getCustomVariable("test")), Color.red);
        }
    }
}
