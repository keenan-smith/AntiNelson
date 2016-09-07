using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API;
using PointBlank.PB_Threads;

namespace PointBlank.PB_Library
{
    public class lib_RCON : MonoBehaviour
    {
        #region Variables
        private RCON sys_RCON;
        private ushort _port;
        private string _password;
        private bool _canSendCommands;
        private bool _canReadLogs;
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

        public bool canSendCommands
        {
            get
            {
                return _canSendCommands;
            }
        }

        public bool canReadLogs
        {
            get
            {
                return _canReadLogs;
            }
        }
        #endregion

        public lib_RCON()
        {
            if (!PB.isServer())
                return;
            string path = Variables.currentPath + "\\Settings\\RCON.dat";
            if (ReadWrite.fileExists(path, false, false))
            {
                PBConfig rConfig = new PBConfig(path);
                if (rConfig.getText("enabled") != "true")
                    return;
                _port = ushort.Parse(rConfig.getText("port"));
                _password = rConfig.getText("password");
                _canReadLogs = (rConfig.getText("CanReadLogs") == "true");
                _canSendCommands = (rConfig.getText("CanSendCommands") == "true");
            }
            else
            {
                PBConfig rConfig = new PBConfig();
                rConfig.addTextElement("enabled", "false");
                rConfig.addTextElement("port", "27115");
                rConfig.addTextElement("password", Tool.GetRandomString());
                rConfig.addTextElement("CanReadLogs", "true");
                rConfig.addTextElement("CanSendCommands", "true");
                rConfig.save(path);
                return;
            }
            createThread();
        }

        #region Functions
        public void createThread()
        {
        }
        #endregion
    }
}
