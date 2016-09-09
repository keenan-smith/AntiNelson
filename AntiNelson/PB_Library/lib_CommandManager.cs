using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using PointBlank.API;
using PointBlank.PB_Library;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.PB_Library
{
    public class lib_CommandManager : MonoBehaviour
    {
        #region Variables
        private GameObject obj_Commands;
        #endregion

        public lib_CommandManager()
        {
            if (!PB.isServer())
                return;
            PBLogging.log("Loading CommandManager...");
            obj_Commands = new GameObject();
            DontDestroyOnLoad(obj_Commands);
            //loadCommands(AppDomain.CurrentDomain);
            loadCommands(lib_PluginManager.pluginDomain);
        }

        #region Functions
        public void addLocals(Type t)
        {
            CommandAttribute att = (CommandAttribute)Attribute.GetCustomAttribute(t, typeof(CommandAttribute));
            if (att != null)
            {
                string path = Variables.currentPath + "\\Locals\\" + att.pluginName + "\\" + att.commandName + ".dat";
                if (!ReadWrite.fileExists(path, false, false))
                {
                    if (!ReadWrite.folderExists(Variables.currentPath + "\\Locals\\" + att.pluginName, false))
                        ReadWrite.createFolder(Variables.currentPath + "\\Locals\\" + att.pluginName, false);
                    byte[] s = Tool.getResource(att.commandName);
                    if (s != null && s.Length > 0)
                    {
                        File.WriteAllBytes(path, s);
                    }
                }
            }
        }

        public void loadCommand(Type t)
        {
            CommandAttribute att = (CommandAttribute)Attribute.GetCustomAttribute(t, typeof(CommandAttribute));
            if (att != null)
            {
                PBCommand cmd = obj_Commands.AddComponent(t) as PBCommand;
                if (Array.Exists(PBServer.commands.ToArray(), a => a.command == cmd.command))
                    return;
                cmd.localization = Localizator.read("Locals\\" + att.pluginName + "\\" + att.commandName + ".dat");
                string path = Variables.currentPath + "\\Settings\\" + att.pluginName + "\\" + att.commandName + ".dat";
                if (ReadWrite.fileExists(path, false, false))
                {
                    PBConfig cConfig = new PBConfig(path);
                    if (cConfig.getText("enabled") != "true")
                        return;
                    cmd.alias = cConfig.getChildNodesText("aliases");
                    cmd.command = cConfig.getText("command");
                    cmd.cooldown = int.Parse(cConfig.getText("cooldown"));
                    cmd.maxUsage = int.Parse(cConfig.getText("maxusage"));
                    cmd.permission = cConfig.getText("permission");
                }
                else
                {
                    if (!ReadWrite.folderExists(Variables.currentPath + "\\Settings\\" + att.pluginName, false))
                        ReadWrite.createFolder(Variables.currentPath + "\\Settings\\" + att.pluginName, false);
                    PBConfig cConfig = new PBConfig();
                    cConfig.addTextElement("enabled", "true");
                    cConfig.addTextElements("aliases", "alias", cmd.alias);
                    cConfig.addTextElement("command", cmd.command);
                    cConfig.addTextElement("cooldown", cmd.cooldown.ToString());
                    cConfig.addTextElement("maxusage", cmd.maxUsage.ToString());
                    cConfig.addTextElement("permission", cmd.permission);
                    cConfig.save(path);
                }
                PBServer.commands.Add(cmd);
            }
        }

        public bool loadCommands(AppDomain appd)
        {
            try
            {
                foreach (Assembly asm in appd.GetAssemblies())
                {
                    foreach (Type t in asm.GetTypes())
                    {
                        if (t.IsClass && typeof(PBCommand).IsAssignableFrom(t))
                        {
                            addLocals(t);
                            loadCommand(t);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to load commands from appdomain!", ex);
                return false;
            }
        }

        public bool loadCommands(Assembly asm)
        {
            try
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsClass && typeof(PBCommand).IsAssignableFrom(t))
                    {
                        addLocals(t);
                        loadCommand(t);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while attempting to load commands from assembly!", ex);
                return false;
            }
        }
        #endregion
    }
}
