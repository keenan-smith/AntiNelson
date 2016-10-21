using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;
using PointBlank.API;

namespace PointBlank.PB_Library
{
    internal class lib_PluginManager : MonoBehaviour
    {
        #region Variables
        private List<string> _loadedPaths = new List<string>();
        private Dictionary<PluginAttribute, PBPlugin> _plugins = new Dictionary<PluginAttribute, PBPlugin>();
        private Dictionary<string, string> _libraries = new Dictionary<string, string>();
        #endregion

        #region Properties
        public PluginAttribute[] pluginAttributes
        {
            get
            {
                return _plugins.Keys.ToArray();
            }
        }

        public PBPlugin[] plugins
        {
            get
            {
                return _plugins.Values.ToArray();
            }
        }
        #endregion

        public lib_PluginManager()
        {
            if (!PB.isServer())
                return;
            PBLogging.logImportant("Loading plugin manager...");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(onResolve);
        }

        #region Functions
        public PBPlugin getPlugin(PluginAttribute attribute)
        {
            return _plugins[attribute];
        }

        public PluginAttribute getAttribute(PBPlugin plugin)
        {
            foreach (KeyValuePair<PluginAttribute, PBPlugin> pair in _plugins)
            {
                if (pair.Value == plugin)
                    return pair.Key;
            }
            return null;
        }

        public void unloadPlugin(PBPlugin plugin)
        {
            GameObject.Destroy(plugin.pluginObject);
        }

        public void unloadAllPlugins()
        {
            foreach (KeyValuePair<PluginAttribute, PBPlugin> pair in _plugins)
            {
                GameObject.Destroy(pair.Value.pluginObject);
            }
        }

        public void loadLibraries()
        {
            foreach (string path in Directory.GetFiles(Variables.librariesPathServer, "*.dll"))
                _libraries.Add(AssemblyName.GetAssemblyName(path).FullName, path);
        }

        public void loadPlugins()
        {
            foreach (string path in Directory.GetFiles(Variables.pluginsPathServer, "*.dll"))
                loadPlugin(path);
        }

        public bool loadPlugin(string fullpath)
        {
            try
            {
                if (Array.Exists(_loadedPaths.ToArray(), a => a == fullpath))
                    return true;

                Assembly asm = Assembly.Load(File.ReadAllBytes(fullpath));

                foreach (Type a in asm.GetTypes())
                {
                    if (a.IsClass && typeof(PBPlugin).IsAssignableFrom(a))
                    {
                        PluginAttribute pa = (PluginAttribute)Attribute.GetCustomAttribute(a, typeof(PluginAttribute));
                        if (pa != null)
                        {
                            GameObject obj = new GameObject(pa.pluginName);
                            PBPlugin plugin = obj.AddComponent(a) as PBPlugin;

                            DontDestroyOnLoad(obj);
                            plugin.pluginObject = obj;
                            plugin.onLoad();

                            _plugins.Add(pa, plugin);
                            _loadedPaths.Add(fullpath);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("Failed to load " + Path.GetFileName(fullpath), ex);
                return false;
            }
        }
        #endregion

        #region Event Functions
        private Assembly onResolve(object sender, ResolveEventArgs args)
        {
            if (_libraries.ContainsKey(args.Name))
                return Assembly.Load(File.ReadAllBytes(_libraries[args.Name]));
            else
                PBLogging.log("Unable to find dependency: " + args.Name);
            return null;
        }
        #endregion
    }
}
