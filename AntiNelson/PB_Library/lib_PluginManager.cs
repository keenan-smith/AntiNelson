using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Reflection;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server;
using PointBlank.API;

namespace PointBlank.PB_Library
{
    public class lib_PluginManager
    {

        private static Dictionary<String, PBPlugin> loadedPlugins = new Dictionary<String, PBPlugin>();
        private static AppDomainSetup domainSetup = new AppDomainSetup();
        private static AppDomain _pluginDomain = null;
        private static PluginLoaderProxy pluginLoader = null;

        public static AppDomain pluginDomain
        {
            get
            {
                return _pluginDomain;
            }
        }

        static lib_PluginManager()
        {
            /*
            if (!PB.isServer())
                return;*/

            createPluginDomain();

        }

        private static void createPluginDomain()
        {

            domainSetup.ApplicationBase = Variables.currentPath;
            domainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            domainSetup.DisallowBindingRedirects = false;
            domainSetup.DisallowCodeDownload = true;
            _pluginDomain = AppDomain.CreateDomain("PB Domain", null, domainSetup);
            //_pluginDomain.Load(typeof(PluginLoaderProxy).Assembly.FullName);
            pluginLoader = _pluginDomain.CreateInstanceAndUnwrap(typeof(PluginLoaderProxy).Assembly.FullName, typeof(PluginLoaderProxy).FullName) as PluginLoaderProxy;

        }

        public void unloadAllPlugins()
        {

            loadedPlugins.Clear();
            AppDomain.Unload(_pluginDomain);
            //PBLogging.log("Unloaded plugin domain!");
            Console.WriteLine("Unloaded plugin domain!");
            createPluginDomain();

        }

        public PBPlugin loadPlugin(String name)
        {
            try {
                if (loadedPlugins.ContainsKey(name))
                {

                    PBLogging.logWarning("Already loaded: " + name);
                    return loadedPlugins[name];

                }

                pluginLoader.loadPlugin(name);

            } catch (Exception e)
            {

                PBLogging.logError("ERROR: Failed to load plugin loader!", e);

            }

            return null;

        }

        public static void registerPlugin(String name, PBPlugin plugin)
        {

            loadedPlugins.Add(name, plugin);

        }

        public static byte[] readFile(String path)
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

        public static void printLoadedAssemblies(AppDomain domain)
        {

            foreach (Assembly a in domain.GetAssemblies())
                Console.WriteLine("{0} ({1}): {2}", domain.FriendlyName, domain.Id, a.ManifestModule.Name);

        }

    }
}
