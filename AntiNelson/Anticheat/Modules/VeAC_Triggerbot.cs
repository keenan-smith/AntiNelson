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
    internal class VeAC_Triggerbot : VeAC_Module
    {
        #region Variables
        private Dictionary<PBPlayer, float> _shootMemory = new Dictionary<PBPlayer, float>();
        #endregion

        #region Mono Functions
        public void Start()
        {
            PBPlayer.OnPlayerMove += new PBPlayer.PlayerMoveHandler(Event_PlayerMove);
        }
        #endregion

        #region Event Functions
        private void Event_PlayerMove(PBPlayer player, Vector3 position, Quaternion rotation)
        {

        }
        #endregion

        #region Functions
        public override bool check()
        {
            return VeAC_Settings.anti_Triggerbot;
        }
        #endregion
    }
}
