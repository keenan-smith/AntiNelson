using System;
using PointBlank.API.Extensions;
using PointBlank.API.Attributes;

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
