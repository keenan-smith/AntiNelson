using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PointBlank.API.Extensions;

namespace PointBlank.PB_Library
{
    public class PluginLoadingProxy : MarshalByRefObject
    {

        public PBPlugin fetchPlugin(String path)
        {
            try
            {
                Assembly asm = Assembly.LoadFile(path);

                PBPlugin plugin = null;

                foreach (Type t in asm.GetTypes())
                {

                    if (t.IsSubclassOf(typeof(PBPlugin)))
                    {

                        plugin = (PBPlugin)asm.CreateInstance(t.FullName);

                    }

                }

                if (plugin != null)
                    Console.WriteLine("Loaded plugin: " + path);
                else
                    Console.WriteLine("Failed to load plugin: " + path);

                return plugin;

            }
            catch (Exception e)
            {

                Console.WriteLine(e);

            }

            return null;

        }

    }
}
