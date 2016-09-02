using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Extensions;
using PointBlank.API.Attributes;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.PB_Library
{
    public class lib_CommandManager
    {
        #region Variables
        private List<PBCommand> _commands = new List<PBCommand>();
        #endregion

        #region Propertys
        public PBCommand[] commands
        {
            get
            {
                return _commands.ToArray();
            }
        }
        #endregion

        public lib_CommandManager()
        {
            loadCommands(AppDomain.CurrentDomain); // Don't forget to load our commands!
        }

        #region Functions
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
                            CommandAttribute att = (CommandAttribute)Attribute.GetCustomAttribute(t, typeof(CommandAttribute));
                            if (att != null)
                            {
                                PBCommand cmd = (PBCommand)Activator.CreateInstance(t);
                                if (Array.Exists(commands, a => a.command == cmd.command))
                                    continue;
                                cmd.localization = Localizator.read("Locals\\" + att.pluginName + "\\" + att.commandName + ".dat");
                                _commands.Add(cmd);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                CommandWindow.LogError("ERROR: Exception while attempting to load commands from appdomain!");
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
                        CommandAttribute att = (CommandAttribute)Attribute.GetCustomAttribute(t, typeof(CommandAttribute));
                        if (att != null)
                        {
                            PBCommand cmd = (PBCommand)Activator.CreateInstance(t);
                            if (Array.Exists(commands, a => a.command == cmd.command))
                                continue;
                            cmd.localization = Localizator.read("Locals\\" + att.pluginName + "\\" + att.commandName + ".dat");
                            _commands.Add(cmd);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                CommandWindow.LogError("ERROR: Exception while attempting to load commands from assembly!");
                return false;
            }
        }
        #endregion
    }
}
