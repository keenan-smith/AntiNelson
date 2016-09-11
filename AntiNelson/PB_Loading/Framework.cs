using PointBlank.PB_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API;
using SDG.Unturned;

namespace PointBlank.PB_Loading
{
    public class Framework : MonoBehaviour
    {

        public void _Start()
        {
            Variables.currentPath = PB.getWorkingDirectory();

            #region Folder Stuff
            if (PB.isServer())
            {
                foreach (string fName in Variables.serverDirectories)
                {
                    string path = Variables.currentPath + "\\" + fName;
                    if (!ReadWrite.folderExists(path, false))
                        ReadWrite.createFolder(path, false);
                }
            }
            else
            {
                foreach (string fName in Variables.clientDirectories)
                {
                    string path = Variables.currentPath + "\\" + fName;
                    if (!ReadWrite.folderExists(path, false))
                        ReadWrite.createFolder(path, false);
                }
            }
            #endregion

            Variables.obj_Commands = new GameObject();
            DontDestroyOnLoad(Variables.obj_Commands);

            Instances.eventInitalizer = new lib_EventInitalizer();

            PB.preInit();

            Instances.autoSave = new lib_AutoSave();
            Instances.codeReplacer = new lib_CodeReplacer();
            Instances.RCON = new lib_RCON();
            Instances.commandManager = new lib_CommandManager();
            Instances.pluginManager = new lib_PluginManager();
            Instances.pluginManager.loadPlugins();

            PB.postInit();
        }

        public void _Update()
        {
        }

        public void _OnGUI()
        {
        }
    }
}
