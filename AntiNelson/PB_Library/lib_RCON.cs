using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.PB_GameObjects;
using PointBlank.API;

namespace PointBlank.PB_Library
{
    public class lib_RCON : MonoBehaviour
    {
        #region Variables
        private GameObject obj_RCON;
        private RCON sys_RCON;
        private ushort _port;
        private string _password;
        #endregion

        #region Properties
        public ushort port
        {
            get
            {
                return _port;
            }
        }

        public string password
        {
            get
            {
                return _password;
            }
        }
        #endregion

        public lib_RCON()
        {
            string path = Variables.currentPath + "\\Settings\\RCON.dat";
            if (ReadWrite.fileExists(path, false, false))
            {
                PBConfig rConfig = new PBConfig(path);
                if (rConfig.getText("enabled") != "true")
                    return;
                _port = ushort.Parse(rConfig.getText("port"));
                _password = rConfig.getText("password");
            }
            else
            {
                PBConfig rConfig = new PBConfig();
                rConfig.addTextElement("enabled", "false");
                rConfig.addTextElement("port", "27115");
                rConfig.addTextElement("password", Tool.GetRandomString());
                rConfig.save(path);
            }
            createGameObject();
        }

        #region Functions
        public void createGameObject()
        {
            obj_RCON = new GameObject();

            sys_RCON = obj_RCON.AddComponent<RCON>();

            DontDestroyOnLoad(obj_RCON);
        }
        #endregion
    }
}
