using System;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;

namespace TestPlugin
{
    [Plugin("Test Plugin", "Kunii", false, true)]
    public class Main : PBPlugin
    {
        
        public override void load()
        {
            Console.WriteLine("Hurrdurr");
        }

    }
}
