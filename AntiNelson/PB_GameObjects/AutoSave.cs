using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.PB_GameObjects
{
    public class AutoSave : MonoBehaviour
    {
        #region Variables
        private DateTime lastSave;
        #endregion

        public void Start()
        {
            lastSave = DateTime.Now;
        }

        #region Functions
        public void Update()
        {
            if ((DateTime.Now - lastSave).TotalMilliseconds >= Instances.autoSave.interval)
            {
                SaveManager.save();
                lastSave = DateTime.Now;
            }
        }
        #endregion
    }
}
