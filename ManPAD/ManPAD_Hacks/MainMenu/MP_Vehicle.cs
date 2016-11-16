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
    [MenuOption(4, "Vehicle", 200f, 200f)]
    public class MP_Vehicle : MenuOption
    {
        #region Variables
        private InteractableVehicle vehicle;
        private VehicleAsset asset;
        private Rigidbody body;
        public bool fly = false;
        public bool disablesGravity = false;
        public float flySpeed = 15f;
        #endregion

        #region Mono Functions
        public void Update()
        {
            if (!Variables.isInGame)
                return;

            if (fly && vehicle != null && body != null)
            {
                if (Input.GetKeyDown(ControlsSettings.interact))
                    fly = false;
                // ↑↓→←
                if (Input.GetKey(KeyCode.UpArrow))
                    body.AddForce(Camera.main.transform.forward * flySpeed);
                if (Input.GetKey(KeyCode.DownArrow))
                    body.AddForce(-Camera.main.transform.forward * flySpeed);
                if (Input.GetKey(KeyCode.RightArrow))
                    body.AddForce(Camera.main.transform.right * flySpeed);
                if (Input.GetKey(KeyCode.LeftArrow))
                    body.AddForce(-Camera.main.transform.right * flySpeed);
            }
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            if (GUILayout.Button("Vehicle Flight: " + (fly ? "On" : "Off")))
            {
                vehicle = Player.player.movement.getVehicle();
                if (vehicle != null)
                {
                    fly = !fly;
                    asset = vehicle.asset;
                    body = vehicle.GetComponent<Rigidbody>();
                    if (disablesGravity)
                    {
                        if (fly)
                        {
                            body.useGravity = false;
                        }
                        else
                        {
                            body.useGravity = true;
                        }
                    }
                }
            }
            disablesGravity = GUILayout.Toggle(disablesGravity, "Disables Gravity");
            GUILayout.Label("Flight speed: " + flySpeed);
            flySpeed = GUILayout.HorizontalSlider(flySpeed, 10f, 100f);
        }
        #endregion
    }
}
