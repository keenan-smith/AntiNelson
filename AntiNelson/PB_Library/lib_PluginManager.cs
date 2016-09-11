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

        private static List<Type> commandList = new List<Type>();
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
            if (!PB.isServer())
                return;

            createPluginDomain();
        }

        private static void createPluginDomain()
        {
            domainSetup.ApplicationBase = Variables.pathManaged;
            domainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            domainSetup.DisallowBindingRedirects = false;
            domainSetup.DisallowCodeDownload = true;
            _pluginDomain = AppDomain.CreateDomain("PB Domain", null, domainSetup);
            pluginLoader = _pluginDomain.CreateInstanceAndUnwrap(typeof(PluginLoaderProxy).Assembly.FullName, typeof(PluginLoaderProxy).FullName) as PluginLoaderProxy;
            pluginLoader.init();

        }

        public void unloadAllPlugins()
        {
            loadedPlugins.Clear();
            AppDomain.Unload(_pluginDomain);
            PBLogging.log("Unloaded plugin domain!");
            createPluginDomain();

            printLoadedAssemblies(AppDomain.CurrentDomain);
            printLoadedAssemblies(_pluginDomain);
        }

        public void loadPlugins()
        {

            foreach (String path in Directory.GetFiles(Variables.pluginsPathServer, "*.dll"))
                loadPlugin(path);

        }

        public PBPlugin loadPlugin(String fullPath)
        {
            try
            {
                if (loadedPlugins.ContainsKey(fullPath))
                {
                    PBLogging.logWarning("Already loaded: " + fullPath);
                    return loadedPlugins[fullPath];
                }

                pluginLoader.loadPlugin(AppDomain.CurrentDomain, fullPath);

            }
            catch (Exception e)
            {
                PBLogging.logError("ERROR: Failed to load plugin! - ", e);
            }
            return null;
        }

        public static void registerPlugin(String fullPath, PBPlugin plugin)
        {
            loadedPlugins.Add(fullPath, plugin);
        }

        public static void registerCommands()
        {

            Instances.commandManager.loadCommands((Assembly)AppDomain.CurrentDomain.GetData("asm"));

        }

        public static byte[] readFile(String fullPath)
        {
            byte[] buffer = new byte[4096];

            using (FileStream fs = new FileStream(fullPath, FileMode.Open))
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
                PBLogging.log(String.Format("{0} ({1}): {2}", domain.FriendlyName, domain.Id, a.ManifestModule.Name));
        }
    }
}
