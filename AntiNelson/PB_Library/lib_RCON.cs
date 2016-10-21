using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.PB_Threads;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.PB_Extensions;

namespace PointBlank.PB_Library
{
    internal class lib_RCON : MonoBehaviour
    {
        #region Variables
        private RCON sys_RCON;
        private ushort _port;
        private string _password;
        private bool _canSendCommands;
        private bool _canReadLogs;
        private bool _enabled = false;
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

        public bool enabled
        {
            get
            {
                return _enabled;
            }
        }
        #endregion

        public lib_RCON()
        {
            if (!PB.isServer())
                return;
            PBLogging.logImportant("Loading RCON...");
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
            _enabled = true;
            createThread();
        }

        #region Functions
        public void createThread()
        {
            sys_RCON = new RCON(port, password, canReadLogs, canSendCommands);
            sys_RCON.startHooking();
        }

        public void RCONOutputUpdate(string text)
        {
            if (!enabled || !canReadLogs)
                return;
            if (text == "\n" || text == "\t" || string.IsNullOrEmpty(text))
                return;
            lock (sys_RCON.clients)
            {
                foreach (RCONClient client in sys_RCON.clients)
                {
                    if(client.inConsole)
                        client.writeLog(text);
                }
            }
        }

        public void RCONInputUpdate()
        {
            if (!enabled || !canSendCommands)
                return;
            lock (sys_RCON.clients)
            {
                foreach (RCONClient client in sys_RCON.clients)
                {
                    lock (client.execute)
                    {
                        while (client.execute.Count > 0)
                        {
                            string command = client.execute.Dequeue();
                            PBServer.ParseInputCommand(command);
                            Console.WriteLine("[" + client.IP + "] " + command);
                        }
                    }
                }
            }
        }

        public void RCONDestroy()
        {
            if (!enabled)
                return;
            sys_RCON.stopHooking();
        }
        #endregion
    }
}
