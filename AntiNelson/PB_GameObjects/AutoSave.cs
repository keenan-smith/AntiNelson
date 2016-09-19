using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API;

namespace PointBlank.PB_GameObjects
{
    public class AutoSave : MonoBehaviour
    {
        #region Variables
        private DateTime lastSave;
        private bool loaded = false;
        #endregion

        public void Start()
        {
            lastSave = DateTime.Now;
        }

        #region Functions
        public void Update()
        {
            if ((DateTime.Now - lastSave).TotalMilliseconds >= Instances.autoSave.interval && loaded)
            {
                PBLogging.log("Autosaving....");
                SaveManager.save();
                lastSave = DateTime.Now;
                PBLogging.log("Map saved!");
            }
        }

        public void LevelLoadedEvent(int level)
        {
            loaded = true;
        }
        #endregion
    }
}
