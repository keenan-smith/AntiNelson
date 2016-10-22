using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.Anticheat.Extensions;
using PointBlank.API.Server;

namespace PointBlank.Anticheat.Modules
{
    internal class VeAC_AntiScreenshot : VeAC_Module
    {
        #region Variables
        private Dictionary<PBPlayer, DateTime> _queue = new Dictionary<PBPlayer, DateTime>();
        #endregion

        #region Mono Functions
        public void Start()
        {

        }
        #endregion
    }
}
