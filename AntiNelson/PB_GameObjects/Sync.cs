using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Types;

namespace PointBlank.PB_GameObjects
{
    public class Sync : MonoBehaviour
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
                foreach (KeyValuePair<string, CustomVariable> customVar in player.customVariables)
                {
                    PBSync.sql_insertCommand()
                }
            }
        }
        #endregion
    }
}
