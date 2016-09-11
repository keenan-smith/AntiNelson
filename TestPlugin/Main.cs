using System;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using PointBlank.API;
using PointBlank.PB_Library;
using PointBlank;

namespace TestPlugin
{
    [Plugin("Test Plugin", "Kunii", false, true)]
    public class Main : PBPlugin
    {
        
        public void load()
        {
            Console.WriteLine("Hurrdurr, called from plugin!");
            //Instances.commandManager.loadCommand(typeof(TestCommand));
        }

    }
}
