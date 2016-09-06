using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API;

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
                PluginAttribute pa = null;

                foreach (Type t in asm.GetTypes())
                {

                    if (t.IsSubclassOf(typeof(PBPlugin)))
                    {
                        pa = (PluginAttribute)Attribute.GetCustomAttribute(t, typeof(PluginAttribute));
                        if(pa != null)
                        {
                            plugin = (PBPlugin)asm.CreateInstance(t.FullName);
                        }
                        else
                        {
                            PBLogging.logWarning("Failed to load plugin: " + path);
                        }

                    }

                }

                if (plugin != null && pa != null)
                    PBLogging.log("Loaded plugin: " + pa.pluginName);
                else
                    PBLogging.logWarning("Failed to load plugin: " + path);

                return plugin;

            }
            catch (Exception e)
            {

                PBLogging.logError("ERROR: Failed to load plugin!", e);

            }

            return null;

        }

    }
}
