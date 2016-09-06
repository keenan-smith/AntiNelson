using PointBlank.API.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.PB_Library
{
    [Serializable]
    public class PluginLoaderProxy : MarshalByRefObject
    {

        public void loadPlugin(String dll)
        {
            foreach (Type t in AppDomain.CurrentDomain.Load(lib_PluginManager.readFile(dll)).GetTypes())
            {

                if (t.IsInterface || t.IsAbstract)
                    continue;

                if (t.GetInterface(typeof(PBPlugin).FullName) != null)
                {

                    PBPlugin plugin = (AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap(dll, t.FullName) as PBPlugin);
                    plugin.load();
                    lib_PluginManager.registerPlugin(dll, plugin);
                    break;

                }
            }

        }

    }
}
