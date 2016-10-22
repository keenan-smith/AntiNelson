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
    internal class VeAC_AntiAim : VeAC_Module
    {
        #region Mono Functions
        public void Start()
        {
            PBPlayer.OnPlayerHurt += new PBPlayer.PlayerHurtHandler(Event_PlayerDamage);
        }
        #endregion

        #region Functions
        public override bool check()
        {
            return VeAC_Settings.anti_AntiAim;
        }
        #endregion

        #region Event Functions
        public void Event_PlayerDamage(PBPlayer victim, PBPlayer attacker, byte damage, ELimb limb, Vector3 force)
        {
            if (!Provider.modeConfigData.Gameplay.Ballistics || VeAC_Tools.getDistance(victim.player.transform.position, attacker.player.transform.position) <= 50f)
            {
                RaycastHit hit;
                if (Physics.Raycast(attacker.player.transform.position, attacker.player.transform.forward, out hit, ((ItemGunAsset)attacker.player.equipment.asset).range, RayMasks.DAMAGE_SERVER))
                {
                    if (hit.collider.gameObject != victim.player.gameObject)
                        VeAC.instance.moduleManager.addDetection(attacker, "AntiAim");
                }
                else
                {
                    VeAC.instance.moduleManager.addDetection(attacker, "AntiAim");
                }
            }
        }
        #endregion
    }
}
