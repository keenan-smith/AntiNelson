using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using ManPAD.ManPAD_Overridables;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(20, "Build Anywhere", 300f)]
    public class MP_BuildAnywhere : MenuOption
    {
        public static Vector3 pos = new Vector3(0,0,0);
        public static Vector3 offset = new Vector3(0, 0, 0);
        public Vector3 offs = new Vector3(0,0,0);
        public float num = 1;
        public bool ba_enable = false;
        public RedirectCallsState[] rcs = new RedirectCallsState[2];
        public RedirectCallsState[] iprcs = new RedirectCallsState[6];
        public MethodInfo[] mi = new MethodInfo[2];
        public MethodInfo[] tordr = new MethodInfo[2];
        public MethodInfo[] ipmi = new MethodInfo[6];
        public MethodInfo[] iptordr = new MethodInfo[6];
        public Material mat;
        public bool ip_enable = false;

        void Start()
        {
            mi[0] = typeof(OV_UseableBarricade).GetMethod("checkSpace", BindingFlags.Instance | BindingFlags.Public);
            mi[1] = typeof(OV_UseableStructure).GetMethod("checkSpace", BindingFlags.Instance | BindingFlags.Public);
            tordr[0] = typeof(UseableBarricade).GetMethod("checkSpace", BindingFlags.Instance | BindingFlags.NonPublic);
            tordr[1] = typeof(UseableStructure).GetMethod("checkSpace", BindingFlags.Instance | BindingFlags.NonPublic);

            ipmi[0] = typeof(OV_UseableBarricade).GetMethod("get_isBuildable", BindingFlags.Instance | BindingFlags.Public);
            ipmi[1] = typeof(OV_UseableBarricade).GetMethod("get_isUseable", BindingFlags.Instance | BindingFlags.Public);
            ipmi[2] = typeof(OV_UseableBarricade).GetMethod("build", BindingFlags.Instance | BindingFlags.Public);
            ipmi[3] = typeof(OV_UseableStructure).GetMethod("get_isConstructable", BindingFlags.Instance | BindingFlags.Public);
            ipmi[4] = typeof(OV_UseableStructure).GetMethod("get_isUseable", BindingFlags.Instance | BindingFlags.Public);
            ipmi[5] = typeof(OV_UseableStructure).GetMethod("construct", BindingFlags.Instance | BindingFlags.Public);

            iptordr[0] = typeof(UseableBarricade).GetMethod("get_isBuildable", BindingFlags.Instance | BindingFlags.NonPublic);
            iptordr[1] = typeof(UseableBarricade).GetMethod("get_isUseable", BindingFlags.Instance | BindingFlags.NonPublic);
            iptordr[2] = typeof(UseableBarricade).GetMethod("build", BindingFlags.Instance | BindingFlags.NonPublic);
            iptordr[3] = typeof(UseableStructure).GetMethod("get_isConstructable", BindingFlags.Instance | BindingFlags.NonPublic);
            iptordr[4] = typeof(UseableStructure).GetMethod("get_isUseable", BindingFlags.Instance | BindingFlags.NonPublic);
            iptordr[5] = typeof(UseableStructure).GetMethod("construct", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        void Update()
        {
            if (Variables.isInGame && ba_enable && Player.player.look != null)
            {
                Transform trans = Player.player.look.aim.transform;
                pos = trans.position;
                if (Input.GetKey(KeyCode.Keypad7))
                {
                    offset += transform.up * (-num * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.Keypad9))
                {
                    offset += transform.up * (num * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.Keypad8))
                {
                    offset += transform.forward * (num * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.Keypad2))
                {
                    offset += transform.forward * (-num * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.Keypad4))
                {
                    offset += transform.right * (-num * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.Keypad6))
                {
                    offset += transform.right * (num * Time.deltaTime);
                }

                if (Player.player.equipment.useable is UseableBarricade)
                {
                    try
                    {
                        typeof(UseableBarricade).GetField("point", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableBarricade)Player.player.equipment.useable, MP_BuildAnywhere.pos + new Vector3(0, -2, 0) + MP_BuildAnywhere.offset);
                        typeof(UseableBarricade).GetField("angle_y", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableBarricade)Player.player.equipment.useable, Player.player.look.yaw);
                    }
                    catch (Exception ex) { Debug.LogException(ex); }
                }
                if (Player.player.equipment.useable is UseableStructure)
                {
                    try
                    {
                        typeof(UseableStructure).GetField("point", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableStructure)Player.player.equipment.useable, MP_BuildAnywhere.pos + new Vector3(0, -2, 0) + MP_BuildAnywhere.offset);
                        typeof(UseableStructure).GetField("angle", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableStructure)Player.player.equipment.useable, Player.player.look.yaw);
                    }
                    catch (Exception ex) { Debug.LogException(ex); }
                }
            }

        }

        public override void runGUI()
        {
            if (GUILayout.Button("Build Anywhere " + (ba_enable ? "Enabled" : "Disabled")))
            {
                ba_enable = !ba_enable;
                if (ba_enable)
                {
                    rcs[0] = MP_Redirector.RedirectCalls(tordr[0], mi[0]);
                    rcs[1] = MP_Redirector.RedirectCalls(tordr[1], mi[1]);
                }
                if (!ba_enable)
                {
                    MP_Redirector.RevertRedirect(tordr[0], rcs[0]);
                    MP_Redirector.RevertRedirect(tordr[1], rcs[1]);
                }
            }
            GUILayout.Label("Offset Modify Speed: " + num);
            num = GUILayout.HorizontalSlider(num, 0f, 10f);
            GUILayout.Label("Offset | X: " + Math.Round(offset.x, 3) + ", Y: " + Math.Round(offset.y, 3) + ", Z: " + Math.Round(offset.z, 3));
            if (GUILayout.Button("Reset Offset"))
            {
                offset = new Vector3(0, 0, 0);
            }

            GUILayout.Space(10f);

            /*if (GUILayout.Button("Instant Place" + (ip_enable ? "Enabled" : "Disabled")))
            {
                ip_enable = !ip_enable;
                if (ip_enable)
                {
                    iprcs[0] = MP_Redirector.RedirectCalls(iptordr[0], ipmi[0]);
                    iprcs[1] = MP_Redirector.RedirectCalls(iptordr[1], ipmi[1]);
                    iprcs[2] = MP_Redirector.RedirectCalls(iptordr[2], ipmi[2]);
                    iprcs[3] = MP_Redirector.RedirectCalls(iptordr[3], ipmi[3]);
                    iprcs[4] = MP_Redirector.RedirectCalls(iptordr[4], ipmi[4]);
                    iprcs[5] = MP_Redirector.RedirectCalls(iptordr[5], ipmi[5]);
                }
                if (!ip_enable)
                {
                    MP_Redirector.RevertRedirect(iptordr[0], iprcs[0]);
                    MP_Redirector.RevertRedirect(iptordr[1], iprcs[1]);
                    MP_Redirector.RevertRedirect(iptordr[2], iprcs[2]);
                    MP_Redirector.RevertRedirect(iptordr[3], iprcs[3]);
                    MP_Redirector.RevertRedirect(iptordr[4], iprcs[4]);
                    MP_Redirector.RevertRedirect(iptordr[5], iprcs[5]);
                }
            }*/

        }
    }
}
