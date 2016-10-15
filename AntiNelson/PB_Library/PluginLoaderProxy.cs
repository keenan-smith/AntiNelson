using PointBlank.API.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PointBlank.API;

namespace PointBlank.PB_Library
{
    [Serializable]
    internal class PluginLoaderProxy : MarshalByRefObject
    {
        private AppDomain domain = AppDomain.CurrentDomain;

        public void init()
        {
            //Console.WriteLine("Adding event...");
            //domain.AssemblyResolve += new ResolveEventHandler(resolveEvent);
            //domain.AssemblyLoad += new AssemblyLoadEventHandler(loadedEvent);
        }

        public void loadPlugin(AppDomain parent, String dll)
        {

            Assembly asm = AppDomain.CurrentDomain.Load(lib_PluginManager.readFile(dll));

            foreach (Type t in asm.GetTypes())
            {
                if (t.IsInterface || t.IsAbstract)
                    continue;

                if (t.GetInterface(typeof(PBPlugin).FullName) != null)
                {
                    PBPlugin plugin = (AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap(dll, t.FullName) as PBPlugin);
                    plugin.load();
                    lib_PluginManager.registerPlugin(dll, plugin);
                }

                if (typeof(PBCommand).IsAssignableFrom(t))
                {

                    parent.SetData("asm", parent.Load(lib_PluginManager.readFile(dll)));
                    parent.DoCallBack(new CrossAppDomainDelegate(lib_PluginManager.registerCommands));

                }
            }
        }

        private Assembly resolveEvent(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("LoadASM: " + args.Name);
            return Assembly.Load(args.Name);
        }

        private void loadedEvent(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine("Loaded: " + args.LoadedAssembly.FullName);
        }
    }
}
