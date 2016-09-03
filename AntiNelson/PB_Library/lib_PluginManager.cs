using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Reflection;
using PointBlank.API.Extensions;
using PointBlank.API;

namespace PointBlank.PB_Library
{
    public class lib_PluginManager
    {

        public Dictionary<String, PBPlugin> loadedPlugins = new Dictionary<String, PBPlugin>();
        private static AppDomainSetup domainSetup = new AppDomainSetup();
        private static AppDomain _pluginDomain = null;
        private static PluginLoadingProxy pluginLoader = null;

        public static AppDomain pluginDomain
        {
            get
            {
                return _pluginDomain;
            }
        }

        static lib_PluginManager()
        {

            //domainSetup.ApplicationBase = Variables.pluginsPathServer;
            domainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            domainSetup.DisallowBindingRedirects = !(domainSetup.DisallowCodeDownload = true);
            _pluginDomain = AppDomain.CreateDomain("PB Domain", null, domainSetup);

        }

        public void unloadAllPlugins()
        {

            AppDomain.Unload(_pluginDomain);
            PBLogging.log("Unloaded plugin domain!");

        }

        public PBPlugin loadPlugin(String name)
        {
            try {
                if (loadedPlugins.ContainsKey(name))
                {

                    PBLogging.logWarning("Already loaded: " + name);
                    return loadedPlugins[name];

                }

                String pluginPath = Variables.pluginsPathServer + name;

                //Console.WriteLine(pluginPath);

                if (pluginLoader == null)
                {

                    _pluginDomain.Load(typeof(PluginLoadingProxy).Assembly.FullName);
                    pluginLoader = (PluginLoadingProxy)Activator.CreateInstance(_pluginDomain, typeof(PluginLoadingProxy).Assembly.FullName, typeof(PluginLoadingProxy).FullName).Unwrap();
                    PBLogging.log("Initialized plugin loader");

                }

                if (pluginLoader != null)
                {

                    //PBPlugin plugin = pluginLoader.fetchPlugin(name);
                    //plugin.load();
                    //loadedPlugins.Add(name, plugin);

                    //return plugin;

                }
                else PBLogging.logWarning("Plugin loader is not functioning!");
            } catch (Exception e)
            {

                PBLogging.logError("ERROR: Failed to load plugin loader!", e);

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

                    try
                    {
                        while ((i = fs.Read(buffer, 0, buffer.Length)) > 0)
                            ms.Write(buffer, 0, i);
                    }
                    catch (InternalBufferOverflowException ex)
                    {
                        PBLogging.logError("ERROR: Buffer overflow upon reading file!", ex);
                    }

                    fs.Close();

                    return ms.ToArray();

                }

            }

        }

    }
}
