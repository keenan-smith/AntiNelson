using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Types;

namespace PointBlank.PB_GameObjects
{
    internal class Sync : MonoBehaviour
    {
        #region Variables
        private DateTime lastSync;
        private bool loaded = false;
        #endregion

        public void Start()
        {
            lastSync = DateTime.Now;
        }

        #region Functions
        public void Update()
        {
            if ((DateTime.Now - lastSync).TotalMilliseconds >= Instances.sync.syncTime && loaded)
            {
                PBLogging.log("Syncing....");
                sync();
                lastSync = DateTime.Now;
                PBLogging.log("Synced!");
            }
        }

        public void LevelLoadedEvent(int level)
        {
            loaded = true;
        }

        private void sync()
        {
            foreach (PBPlayer player in PBServer.players)
            {
                bool exists = PBSync.sql_exists("PlayerSaves", new string[] { "steamId='" + player.steam64.ToString() + "'" }, Instances.sync.connection);
                if (!exists)
                {
                    string[] columns = new string[]{
                        "steamId",
                        "color"
                    };
                    string[] values = new string[]{
                        player.steam64.ToString(),
                        player.playerColor.ToString()
                    };
                    PBSync.sql_insertCommand("PlayerSaves", columns, values, Instances.sync.connection);
                }
                foreach (KeyValuePair<string, CustomVariable> customVar in player.customVariables)
                {
                    
                }
            }
        }
        #endregion
    }
}
