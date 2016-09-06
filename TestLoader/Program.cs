using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank;
using PointBlank.API;
using PointBlank.API.Extensions;
using PointBlank.PB_Library;

namespace TestLoader
{
    class Program
    {
        static void Main(string[] args)
        {

            lib_PluginManager pm = new lib_PluginManager();

            pm.loadPlugin(@"C:\Users\Kunii\Dropbox\Github\AntiNelson\TestPlugin\bin\Debug\TestPlugin.dll");

            Console.ReadKey();
            pm.unloadAllPlugins();
            lib_PluginManager.printLoadedAssemblies(AppDomain.CurrentDomain);
            lib_PluginManager.printLoadedAssemblies(lib_PluginManager.pluginDomain);

            Console.ReadKey();
            pm.loadPlugin(@"C:\Users\Kunii\Dropbox\Github\AntiNelson\TestPlugin\bin\Debug\TestPlugin.dll");
            Console.ReadKey();
        }
    }
}
