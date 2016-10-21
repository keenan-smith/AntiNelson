using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API;
using PointBlank.API.Server;

namespace PointBlank.PB_Library
{
    internal class lib_EventRunner : MonoBehaviour
    {
        #region Variables
        private List<playerSavePos> savePos = new List<playerSavePos>();
        #endregion

        public lib_EventRunner()
        {
            PBLogging.logImportant("Loading event runner...");
        }

        #region Functions
        public void Update()
        {
            foreach (playerSavePos save in savePos)
            {
                if (save.player.player.transform.position != save.pos || save.player.player.transform.rotation != save.rotation)
                {
                    save.pos = save.player.player.transform.position;
                    save.rotation = save.player.player.transform.rotation;
                    PBPlayer.playerMoveEvent(save.player, save.pos, save.rotation);
                }
            }
        }
        #endregion
    }

    internal class playerSavePos
    {
        #region Variables
        public PBPlayer player;
        public Vector3 pos;
        public Quaternion rotation;
        #endregion

        public playerSavePos(PBPlayer player, Vector3 pos, Quaternion rotation)
        {
            this.player = player;
            this.pos = pos;
            this.rotation = rotation;
        }
    }
}
