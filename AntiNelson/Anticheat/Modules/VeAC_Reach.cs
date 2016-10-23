using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.Anticheat.Extensions;

namespace PointBlank.Anticheat.Modules
{
    internal class VeAC_Reach : VeAC_Module
    {
        #region Mono Functions
        public void Start()
        {

        }
        #endregion

        #region Functions
        public override bool check()
        {
            return VeAC_Settings.anti_Reach;
        }
        #endregion
    }
}
