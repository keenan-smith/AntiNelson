using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(1, "Aimbot", 200f)]
    public class MP_Aimbot : MenuOption
    {
        #region Variables
        private WaitForSeconds wfs = new WaitForSeconds(0.5f);
        private WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        private ELimb[] limbs = { ELimb.SKULL, ELimb.SPINE };
        private EAttackPriority[] prioritys = { EAttackPriority.DISTANCE, EAttackPriority.THREAT };
        public static object attackNext = null;

        public static bool ignoreFOV = false;
        public static float FOV = 90f;
        public static bool ignoreDistance = false;
        public static float distance = 1000f;
        public static ELimb aimLocation = ELimb.SKULL;
        public static EAttackPriority attackPriority = EAttackPriority.DISTANCE;

        public static bool aimbot = false;
        public static bool autoTrigger = true;

        public static bool silentAim = false;
        public static bool FovCrosshair = false;
        public static bool aim_players = true;
        public static bool aim_friends = false;
        public static bool aim_zombies = false;
        public static bool aim_animals = false;
        #endregion

        #region Mono Functions
        public void Start()
        {
            //StartCoroutine(getAttacking());
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            FovCrosshair = GUILayout.Toggle(FovCrosshair, "Fov Crosshair");       
            ignoreFOV = GUILayout.Toggle(ignoreFOV, "Ignore FOV");
            GUILayout.Label("Aim FOV: " + FOV);
            FOV = (float)Math.Round(GUILayout.HorizontalSlider(FOV, 1f, 360f));
         
            ignoreDistance = GUILayout.Toggle(ignoreDistance, "Ignore Distance");
            GUILayout.Label("Distance: " + distance);
            distance = (float)Math.Round(GUILayout.HorizontalSlider(distance, 0f, 50000f));
            if (GUILayout.Button("Target: " + aimLocation.ToString()))
            {
                if (Array.IndexOf(limbs, aimLocation) == limbs.Length - 1)
                    aimLocation = limbs[0];
                else
                    aimLocation = limbs[Array.IndexOf(limbs, aimLocation) + 1];
            }
            /*if (GUILayout.Button("Priority: " + attackPriority.ToString()))
            {
                if (Array.IndexOf(prioritys, attackPriority) == prioritys.Length - 1)
                    attackPriority = prioritys[0];
                else
                    attackPriority = prioritys[Array.IndexOf(prioritys, attackPriority) + 1];
            }*/

            GUILayout.Space(10f);
            aimbot = GUILayout.Toggle(aimbot, "Aimbot");
            autoTrigger = GUILayout.Toggle(autoTrigger, "Auto Trigger");

            GUILayout.Space(10f);
            silentAim = GUILayout.Toggle(silentAim, "Silent Aim");

            GUILayout.Space(10f);
            aim_players = GUILayout.Toggle(aim_players, "Attack Players");
            aim_friends = GUILayout.Toggle(aim_friends, "Attack Friends");
        }
        #endregion

        #region Coroutines
        private IEnumerator getAttacking()
        {
            while (true)
            {
                if (!(aimbot || silentAim) || !Variables.isInGame)
                {
                    yield return wfs;
                    continue;
                }

                #region Player
                if (aim_players && (Variables.players != null && Variables.players.Length > 0))
                {
                    float cDistance = -1f;
                    Player nextTarget = null;
                    for(int i = 0; i < Variables.players.Length; i++)
                    {
                        SteamPlayer p = Variables.players[i];

                        if (p == null || p.player == null || p.player.gameObject == null || p.player.life.isDead || p.player == Player.player)
                            continue;

                        float pDistance = Tools.getDistance(p.player.transform.position);

                        if (pDistance > distance && !ignoreDistance)
                            continue;
                        if (aim_friends || (MP_Config.instance.getFriends() == null || !MP_Config.instance.getFriends().Contains(p.playerID.steamID.m_SteamID)))
                            continue;

                        if (attackPriority == EAttackPriority.DISTANCE)
                        {
                            if (cDistance == -1f || pDistance < cDistance)
                            {
                                cDistance = pDistance;
                                nextTarget = p.player;
                            }
                        }
                        else if (attackPriority == EAttackPriority.THREAT)
                        {

                        }
                    }
                    yield return wfeof;
                }
                #endregion

                yield return wfs;
            }
        }
        #endregion
    }
}
