using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(0, "Player", 200f, 600f)]
    public class MP_Player : MenuOption
    {
        #region Variables
        private float fly_Y = 0f;

        public bool fly = false;
        public bool godMode = false;
        public bool noclip = false;
        public bool freecam = false;
        public bool invisibility = false;
        public bool jumper = false;
        public bool walkOnWater = false;
        public bool antiDrowning = false;
        public bool infiniteStamina = false;
        public bool antiAim = false;
        public bool antiAim_Movement = false;
        public bool noFall = false;
        public bool autoDC = false;
        public float autoDC_Health = 10f;
        public float speed_walk = 4.5f;
        public float speed_crouch = 2.5f;
        public float speed_prone = 1.5f;
        public float speed_sprint = 7f;
        public float speed_climb = 4.5f;
        public float speed_swim = 3f;
        public float jump_height = 7f;
        #endregion

        #region Mono Functions
        public void Update()
        {
            if (!Variables.isInGame)
                return;

            if (fly)
            {
                if (Input.GetKey(ControlsSettings.jump))
                    fly_Y += 1f;
                if (Input.GetKey(ControlsSettings.crouch) && fly_Y > 0f)
                    fly_Y -= 1f;
                Player.player.transform.position = new Vector3(Player.player.transform.position.x, fly_Y, Player.player.transform.position.z);
                Player.player.movement.gravity = 0f;
            }
            else
            {
                if (fly_Y != Player.player.transform.position.y)
                    fly_Y = Player.player.transform.position.y;
                if(Player.player.movement.gravity == 0f)
                    Player.player.movement.gravity = 1f;
            }
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            fly = GUILayout.Toggle(fly, "Fly(Client)");
            noclip = GUILayout.Toggle(noclip, "Noclip(Client)");
            jumper = GUILayout.Toggle(jumper, "Jumper(Client)");
            walkOnWater = GUILayout.Toggle(walkOnWater, "Walk on water(Client)");
            infiniteStamina = GUILayout.Toggle(infiniteStamina, "Infinite stamina(Client)");
            freecam = GUILayout.Toggle(freecam, "Freecam");

            GUILayout.Space(10f);
            godMode = GUILayout.Toggle(godMode, "GodMode(Client)");
            invisibility = GUILayout.Toggle(invisibility, "Invisibility(Client)");
            antiDrowning = GUILayout.Toggle(antiDrowning, "AntiDrown(Client)");
            noFall = GUILayout.Toggle(noFall, "No fall damage(Client)");
            antiAim = GUILayout.Toggle(antiAim, "AntiAim");
            antiAim_Movement = GUILayout.Toggle(antiAim_Movement, "AntiAim Movement(Client)");
            autoDC = GUILayout.Toggle(autoDC, "Auto Disconnect");
            GUILayout.Label("Health: " + autoDC_Health);
            autoDC_Health = GUILayout.HorizontalSlider(autoDC_Health, 1f, 99f);

            GUILayout.Space(10f);
            GUILayout.Label("Walk Speed(Client): " + speed_walk);
            speed_walk = GUILayout.HorizontalSlider(speed_walk, 0f, 10f);
            GUILayout.Label("Sprint Speed(Client): " + speed_sprint);
            speed_sprint = GUILayout.HorizontalSlider(speed_sprint, 0f, 10f);
            GUILayout.Label("Crouch Speed(Client): " + speed_crouch);
            speed_crouch = GUILayout.HorizontalSlider(speed_crouch, 0f, 10f);
            GUILayout.Label("Prone Speed(Client): " + speed_prone);
            speed_prone = GUILayout.HorizontalSlider(speed_prone, 0f, 10f);
            GUILayout.Label("Swim Speed(Client): " + speed_swim);
            speed_swim = GUILayout.HorizontalSlider(speed_swim, 0f, 10f);
            GUILayout.Label("Climb Speed(Client): " + speed_climb);
            speed_climb = GUILayout.HorizontalSlider(speed_climb, 0f, 10f);
            GUILayout.Label("Jump Height(Client): " + jump_height);
            jump_height = GUILayout.HorizontalSlider(jump_height, 0f, 10f);
        }
        #endregion
    }
}
