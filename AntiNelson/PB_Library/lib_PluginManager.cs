using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace PointBlank.PB_Library
{
    public class lib_PluginManager
    {

        private static AppDomainSetup domainSetup = new AppDomainSetup();

        static lib_PluginManager()
        {

            domainSetup.ApplicationBase = Variables.pluginsPathServer;
            domainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            domainSetup.DisallowBindingRedirects = !(domainSetup.DisallowCodeDownload = true);

        }


        public Dictionary<String, PointBlank.API.PBPlugin> loadedPlugins = new Dictionary<String, API.PBPlugin>();
        private Dictionary<String, AppDomain> loadedPluginAppdomains = new Dictionary<String, AppDomain>();

        public PointBlank.API.PBPlugin loadPlugin(String name)
        {

            if (loadedPlugins.ContainsKey(name))
            {

                Console.WriteLine("Already loaded: " + name);
                return loadedPlugins[name];

            }

            String pluginPath = Variables.pluginsPathServer + name;

            Console.WriteLine(pluginPath);

            try
            {

                AppDomain ad = AppDomain.CreateDomain(name, null, domainSetup);

                Assembly asm = ad.Load(readFile(pluginPath));
                Type pluginType = typeof(PointBlank.API.PBPlugin);
                Type pluginBase = null;

                foreach(Type t in asm.GetExportedTypes())
                    if (t.IsAssignableFrom(pluginType))
                    {
                        pluginBase = t;
                        break;
                    }

                PointBlank.API.PBPlugin plugin = (PointBlank.API.PBPlugin)Activator.CreateInstance(pluginBase);

                plugin.load();

                loadedPlugins.Add(name, plugin);
                loadedPluginAppdomains.Add(name, ad);

            } catch (Exception e)
            {

                Console.WriteLine(e);

            }

            return null;

        }

        private static byte[] readFile(String path)
        {

            byte[] buffer = new byte[4096];

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {

                using (MemoryStream ms = new MemoryStream())
                {

                    int i;

                    while ((i = fs.Read(buffer, 0, buffer.Length)) > 0)
                        ms.Write(buffer, 0, i);

                    fs.Close();

                    return ms.ToArray();

                }

            }

        }

    }
}
