using PointBlank.PB_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API;
using SDG.Unturned;
using PointBlank.PB_Extensions;
using PointBlank.API.Server;

namespace PointBlank.PB_Loading
{
    internal class Framework : MonoBehaviour
    {

        public void _Start()
        {
            PBLogging.logImportant("Loading PointBlank...");

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
            Variables.obj_Extras = new GameObject();
            DontDestroyOnLoad(Variables.obj_Commands);
            DontDestroyOnLoad(Variables.obj_Extras);

            Instances.pluginManager = new lib_PluginManager();
            Instances.RCON = new lib_RCON();
            Instances.autoSave = new lib_AutoSave();
            Instances.eventInitalizer = new lib_EventInitalizer();

            PB.preInit();
            Instances.pluginManager.loadLibraries();
            Instances.pluginManager.loadPlugins();
            PB.postInit();

            Instances.codeReplacer = new lib_CodeReplacer();
            Instances.commandManager = new lib_CommandManager();

            Instances.eventRunner = Variables.obj_Extras.AddComponent<lib_EventRunner>();

            PBLogging.logImportant("PointBlank has been loaded!");
        }

        public void _Update()
        {
            if (PB.isServer())
                Instances.RCON.RCONInputUpdate();
        }

        public void _OnGUI()
        {
        }

        public void _OnDestroy()
        {
            PBLogging.log("Shutting down!");
            if (PB.isServer())
            {
                Instances.RCON.RCONDestroy();
                PBSync.shutdown();
            }
        }
    }
}
