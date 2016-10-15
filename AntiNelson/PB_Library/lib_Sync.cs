using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.PB_GameObjects;
using UnityEngine;

namespace PointBlank.PB_Library
{
    public class lib_Sync : MonoBehaviour
    {
        #region Variables
        private bool _enabled = false;
        private string _username;
        private string _password;
        private string _ip;
        private ushort _port;
        private string _database;
        private int _timeout;
        private int _syncTime;

        private SqlConnection _connection;
        private Sync _sync;
        private GameObject _syncObj;
        #endregion
        
        #region Properties
        public SqlConnection connection
        {
            get
            {
                return _connection;
            }
        }

        public int syncTime
        {
            get
            {
                return _syncTime;
            }
        }
        #endregion

        public lib_Sync()
        {
            if (!PB.isServer())
                return;
            PBLogging.log("Loading sync...");
            string path = Variables.currentPath + "\\Settings\\Sync.dat";
            if (ReadWrite.fileExists(path, false, false))
            {
                PBConfig config = new PBConfig(path);
                if (config.getText("enabled") != "true")
                    return;
                _username = config.getText("username");
                _password = config.getText("password");
                _ip = config.getText("ip");
                _port = ushort.Parse(config.getText("port"));
                _database = config.getText("database");
                _timeout = int.Parse(config.getText("timeout"));
                _syncTime = int.Parse(config.getText("syncTime"));
            }
            else
            {
                PBConfig config = new PBConfig();
                config.addTextElement("enabled", "false");
                config.addTextElement("ip", "127.0.0.1");
                config.addTextElement("port", "1433");
                config.addTextElement("username", "database username here");
                config.addTextElement("password", "database password here");
                config.addTextElement("database", "database name");
                config.addTextElement("timeout", "30");
                config.addTextElement("syncTime", "120");
                config.save(path);
                return;
            }
            _enabled = true;
            doSync();
        }

        #region Functions
        private void doSync()
        {
            _connection = PBSync.addSQL(_ip, _port.ToString(), _username, _password, _database, _timeout);
            if (_connection == null)
            {
                _enabled = false;
                return;
            }
            _syncObj = new GameObject();
            _sync = _syncObj.AddComponent<Sync>();
            DontDestroyOnLoad(_sync);
        }
        #endregion
    }
}
