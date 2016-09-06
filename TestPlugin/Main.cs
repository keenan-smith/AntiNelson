using System;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;

namespace TestPlugin
{
    //[Plugin("Test Plugin", "Kunii", false, true)]
    public class Main : PBPlugin
    {
        
        public void load()
        {
            Console.WriteLine("adqwdhqudwhi");
            PointBlank.PB_Library.lib_PluginManager.printLoadedAssemblies(AppDomain.CurrentDomain);
        }

    }
}
