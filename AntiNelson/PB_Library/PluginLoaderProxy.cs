using PointBlank.API.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PointBlank.PB_Library
{
    [Serializable]
    public class PluginLoaderProxy : MarshalByRefObject
    {

        public void loadPlugin(String dll)
        {

            Assembly asm = AppDomain.CurrentDomain.Load(lib_PluginManager.readFile(dll));

            foreach (Type t in asm.GetTypes())
            {

                if (t.IsInterface || t.IsAbstract)
                    continue;

                if (t.GetInterface(typeof(PBPlugin).FullName) != null)
                {

                    PBPlugin plugin = (AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap(dll, t.FullName) as PBPlugin);
                    Console.WriteLine(t.GetInterface(typeof(PBPlugin).FullName) + "##################");
                    //Instances.commandManager.loadCommands(asm);
                    //lib_CommandManager.loadCommands(asm);
                    plugin.load();
                    lib_PluginManager.registerPlugin(dll, plugin);
                    //break;

                }

                if (typeof(PBCommand).IsAssignableFrom(t))
                {

                    //lib_CommandManager.addLocals(t);
                    lib_CommandManager.loadCommand(t);

                }
            }

        }
    }
}
