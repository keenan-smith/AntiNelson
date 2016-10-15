using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.PB_GameObjects;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.PB_Library
{
    internal class lib_AutoSave : MonoBehaviour
    {
        #region Variables
        private GameObject obj_AutoSave;
        private AutoSave sys_AutoSave;
        private int _interval;
        #endregion

        #region Properties
        public int interval
        {
            get
            {
                return _interval;
            }
        }

        public AutoSave autosave
        {
            get
            {
                return sys_AutoSave;
            }
        }
        #endregion

        public lib_AutoSave()
        {
            if (!PB.isServer())
                return;
            PBLogging.log("Loading Autosave...");
            string path = Variables.currentPath + "\\Settings\\AutoSave.dat";
            if (ReadWrite.fileExists(path, false, false))
            {
                PBConfig aConfig = new PBConfig(path);
                if (aConfig.getText("enabled") != "true")
                    return;
                _interval = int.Parse(aConfig.getText("saveTime"));
            }
            else
            {
                PBConfig rConfig = new PBConfig();
                rConfig.addTextElement("enabled", "true");
                rConfig.addTextElement("saveTime", "600000");
                rConfig.save(path);
            }
            createGameObject();
        }

        #region Functions
        private void createGameObject()
        {
            obj_AutoSave = new GameObject();

            sys_AutoSave = obj_AutoSave.AddComponent<AutoSave>();

            DontDestroyOnLoad(obj_AutoSave);
        }
        #endregion
    }
}
