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
    [MenuOption(1, "Aimbot", 200f, 300f)]
    public class MP_Aimbot : MenuOption
    {
        #region Variables
        private WaitForSeconds wfs = new WaitForSeconds(0.5f);
        private WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        private ELimb[] limbs = { ELimb.SKULL, ELimb.SPINE };
        private EAttackPriority[] prioritys = { EAttackPriority.DISTANCE, EAttackPriority.THREAT };

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
        public static float LineWidth = 1;
        public static bool aim_players = true;
        public static bool aim_friends = false;
        public Material mat;
        #endregion

        #region Mono Functions
        public void Start()
        {
            StartCoroutine(getAttacking());
            mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            mat.hideFlags = HideFlags.HideAndDontSave;
            mat.SetInt("_SrcBlend", 5);
            mat.SetInt("_DstBlend", 10);
            mat.SetInt("_Cull", 0);
            mat.SetInt("_ZWrite", 0);
        }
        #endregion

        void OnGUI()
        {
            if(FovCrosshair && Variables.isInGame && !Variables.isSpying)
            {
                GL.PushMatrix();
                mat.SetPass(0);
                GL.Begin(GL.LINES);
                GL.Color(Color.red);

                for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.01f)
                {
                    Vector3 ci = (new Vector3(Mathf.Cos(theta) * MP_Aimbot.FOV + Screen.width / 2, Mathf.Sin(theta) * MP_Aimbot.FOV + Screen.height / 2, 0));
                    GL.Vertex3(ci.x , ci.y, 0);
             


                }
                GL.End();
            }
        }


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
                    foreach (SteamPlayer p in Variables.players)
                    {
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
