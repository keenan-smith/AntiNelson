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


        public lib_CommandManager()
        {
            if (!PB.isServer())
                return;
            PBLogging.log("Loading CommandManager...");
            //loadCommands(AppDomain.CurrentDomain);
            loadCommands(lib_PluginManager.pluginDomain);
        }

        #region Functions
        public static void addLocals(Type t)
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

        public static void loadCommand(Type t)
        {
            try {
                Console.WriteLine("0000000000000000000000000000000000");
                CommandAttribute att = (CommandAttribute)Attribute.GetCustomAttribute(t, typeof(CommandAttribute));
                Console.WriteLine("9999999999999999999999999999999999");
                if (att != null)
                {
                    Console.WriteLine("8888888888888888888888888888888888888888");
                    PBCommand cmd = Variables.obj_Commands.AddComponent(t) as PBCommand;
                    Console.WriteLine("77777777777777777777777777");
                    if (Array.Exists(PBServer.commands.ToArray(), a => a.command == cmd.command))
                        return;
                    Console.WriteLine("666666666666666666666666666666");
                    cmd.localization = Localizator.read("Locals\\" + att.pluginName + "\\" + att.commandName + ".dat");
                    string path = Variables.currentPath + "\\Settings\\" + att.pluginName + "\\" + att.commandName + ".dat";
                    PBLogging.log("Loading command: " + att.commandName, false);
                    Console.WriteLine("555555555555555555555555555");
                    if (ReadWrite.fileExists(path, false, false))
                    {
                        Console.WriteLine("444444444444444444444444444444444");
                        PBConfig cConfig = new PBConfig(path);
                        if (cConfig.getText("enabled") != "true")
                            return;
                        cmd.alias = cConfig.getChildNodesText("aliases");
                        cmd.command = cConfig.getText("command");
                        cmd.cooldown = int.Parse(cConfig.getText("cooldown"));
                        cmd.maxUsage = int.Parse(cConfig.getText("maxusage"));
                        cmd.permission = cConfig.getText("permission");
                        Console.WriteLine("333333333333333333333333333333");
                    }
                    else
                    {
                        Console.WriteLine("22222222222222222222222222222222");
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
                        Console.WriteLine("111111111111111111111111111111111");
                    }
                    PBServer.commands.Add(cmd);
                }
            } catch(Exception e)
            {

                Console.WriteLine(e);

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

        public static bool loadCommands(Assembly asm)
        {

            try
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsClass && typeof(PBCommand).IsAssignableFrom(t))
                    {
                        Console.WriteLine("Found cmd: " + t.Name);
                        addLocals(t);
                        Console.WriteLine("Passed addLocals");
                        loadCommand(t);
                        Console.WriteLine("Passed loadCommand");
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
