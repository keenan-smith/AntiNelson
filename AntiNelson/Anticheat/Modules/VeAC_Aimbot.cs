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
    internal class VeAC_Aimbot : VeAC_Module // NOTE: MAKE THIS SHIT BETTER IT IS TEMPORARY.... KUNII WHEN YOU HAVE THE TIME MAKE THIS BETTER
    {
        #region Variables
        private Dictionary<PBPlayer, VeAC_PositionSave> _savePositions = new Dictionary<PBPlayer, VeAC_PositionSave>();
        #endregion

        #region Mono Functions
        public void Start()
        {
            if (!check())
                return;
            PBPlayer.OnPlayerMove += new PBPlayer.PlayerMoveHandler(Event_PlayerMove);
        }
        #endregion

        #region Functions
        public override bool check()
        {
            return VeAC_Settings.anti_Aimbot;
        }
        #endregion

        #region Event Functions
        public void Event_PlayerMove(PBPlayer player, Vector3 position, Quaternion rotation)
        {
            if (_savePositions.ContainsKey(player))
            {
                if ((Math.Abs(rotation.x - _savePositions[player].rotation.x) + Math.Abs(rotation.y - _savePositions[player].rotation.y)) > 50)
                    VeAC.instance.moduleManager.addDetection(player, "Aimbot");

                _savePositions[player].position = position;
                _savePositions[player].rotation = rotation;
            }
            else
            {
                _savePositions.Add(player, new VeAC_PositionSave(position, rotation));
            }
        }
        #endregion
    }
}
