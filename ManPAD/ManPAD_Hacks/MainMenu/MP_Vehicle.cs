using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using Steamworks;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(4, "Vehicle", 200f)]
    public class MP_Vehicle : MenuOption
    {
        #region Variables
        public static bool Vehicle_Enabled = false;
        public static bool Vehicle_Fly = false;
        public static bool Vehicle_Noclip = false;
        public static bool Vehicle_DriveOnWater = false;
        public static float Vehicle_Speed = 15f;
        #endregion

        #region Mono Functions
        public void Update()
        {
            if (Player.player == null || Player.player.movement == null || Player.player.movement.getVehicle() == null)
                return;

            Rigidbody body = Player.player.movement.getVehicle().GetComponent<Rigidbody>();

            if ((!Vehicle_Fly || !Vehicle_Enabled) && !body.useGravity)
                body.useGravity = true;
            if (!Variables.isInGame || !Vehicle_Enabled)
                return;

            // ↑↓→←
            if (Input.GetKey(KeyCode.UpArrow))
                body.AddForce(Camera.main.transform.forward * Vehicle_Speed);
            if (Input.GetKey(KeyCode.DownArrow))
                body.AddForce(-Camera.main.transform.forward * Vehicle_Speed);
            if (Input.GetKey(KeyCode.RightArrow))
                body.AddForce(Camera.main.transform.right * Vehicle_Speed);
            if (Input.GetKey(KeyCode.LeftArrow))
                body.AddForce(-Camera.main.transform.right * Vehicle_Speed);

            if (Vehicle_Fly && body.useGravity)
                body.useGravity = false;
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            Vehicle_Enabled = GUILayout.Toggle(Vehicle_Enabled, "Vehicle Hacks");
            Vehicle_Fly = GUILayout.Toggle(Vehicle_Fly, "Vehicle Fly");
            Vehicle_Noclip = GUILayout.Toggle(Vehicle_Noclip, "Vehicle Noclip");
            Vehicle_DriveOnWater = GUILayout.Toggle(Vehicle_DriveOnWater, "Vehicle Drive On Water");
            GUILayout.Label("Vehicle Speed: " + Vehicle_Speed);
            Vehicle_Speed = (float)Math.Round(GUILayout.HorizontalSlider(Vehicle_Speed, 10f, 100f));
        }
        #endregion
    }
}
