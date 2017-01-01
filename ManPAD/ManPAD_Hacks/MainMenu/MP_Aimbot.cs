using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using ManPAD.ManPAD_Library;
using UnityEngine;
using SDG.Unturned;
using System.Reflection;

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
        public static Type attackNextType = null;

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

        public static bool drawTracer = false;
        Material mat;
        #endregion

        #region Mono Functions
        public void Update()
        {
            //StartCoroutine(getAttacking());
            aim();
        }
        public void OnGUI()
        {
            if ((MP_Aimbot.aimbot || MP_Aimbot.silentAim) && MP_Aimbot.attackNext != null && MP_Aimbot.drawTracer)
            {
                SteamPlayer sp = (SteamPlayer)MP_Aimbot.attackNext;
                Player p = sp.player;
                Vector3 player_pos = MainCamera.instance.WorldToScreenPoint(p.gameObject.transform.position);
                player_pos.y = (Screen.height - player_pos.y);

                if (mat == null)
                {
                    mat = new Material(Shader.Find("Hidden/Internal-Colored"));
                    mat.hideFlags = HideFlags.HideAndDontSave;
                    mat.SetInt("_SrcBlend", 5);
                    mat.SetInt("_DstBlend", 10);
                    mat.SetInt("_Cull", 0);
                    mat.SetInt("_ZWrite", 0);
                }

                if (player_pos.z > 0)
                {
                    GL.PushMatrix();
                    GL.Begin(1);
                    mat.SetPass(0);
                    GL.Color(Color.black);

                    GL.Vertex3(Screen.width / 2, Screen.height / 2, 0);
                    GL.Vertex3(player_pos.x, player_pos.y, 0);

                    GL.End();
                    GL.PopMatrix();
                }
            }
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
            
#if DEBUG
            if (GUILayout.Button("Priority: " + attackPriority.ToString()))
            {
                if (Array.IndexOf(prioritys, attackPriority) == prioritys.Length - 1)
                    attackPriority = prioritys[0];
                else
                    attackPriority = prioritys[Array.IndexOf(prioritys, attackPriority) + 1];
            }
#endif

            GUILayout.Space(10f);
            aimbot = GUILayout.Toggle(aimbot, "Aimbot");

            autoTrigger = GUILayout.Toggle(autoTrigger, "Auto Trigger");

#if !FREE
            GUILayout.Space(10f);
            silentAim = GUILayout.Toggle(silentAim, "Silent Aim");
#endif

            GUILayout.Space(10f);
            aim_players = GUILayout.Toggle(aim_players, "Attack Players");
            aim_friends = GUILayout.Toggle(aim_friends, "Attack Friends");

            GUILayout.Space(10f);
            drawTracer = GUILayout.Toggle(drawTracer, "Draw Tracer to Current Target");
        }

        public void aim()
        {
            if (aimbot && Variables.isInGame)
            {
                if (attackNext != null)
                {
                    Player localplayer = Player.player;
                    Vector3 skullPosition = getAimPosition(((SteamPlayer)attackNext).player.gameObject.transform);
                    localplayer.transform.LookAt(skullPosition);
                    localplayer.transform.eulerAngles = new Vector3(0f, localplayer.transform.rotation.eulerAngles.y, 0f);
                    Camera.main.transform.LookAt(skullPosition);
                    float num4 = Camera.main.transform.localRotation.eulerAngles.x;
                    if (num4 <= 90f && num4 <= 270f)
                    {
                        num4 = Camera.main.transform.localRotation.eulerAngles.x + 90f;
                    }
                    else if (num4 >= 270f && num4 <= 360f)
                    {
                        num4 = Camera.main.transform.localRotation.eulerAngles.x - 270f;
                    }
                    localplayer.look.GetType().GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(localplayer.look, num4);
                    localplayer.look.GetType().GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(localplayer.look, localplayer.transform.rotation.eulerAngles.y);
                 
                }
            }
        }

        public Vector3 getAimPosition(Transform parent)
        {
            Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>();
            if (componentsInChildren != null)
            {
                Transform[] array = componentsInChildren;
                for (int i = 0; i < array.Length; i++)
                {
                    Transform tr = array[i];
                    if (tr.name.Trim() == EBones.GetBone(aimLocation))
                    {
                        return tr.position + new Vector3(0f, 0.4f, 0f);
                    }
                }
            }
            return Vector3.zero;
        }
        #endregion
    }
}
