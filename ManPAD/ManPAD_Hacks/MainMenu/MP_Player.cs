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
using System.Collections;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(0, "Player", 200f)]
    public class MP_Player : MenuOption
    {
        #region Variables
        private float fly_Y = 0f;
        private Interactable interactable;
        private bool wasDown = false;
        private WaitForSeconds wfs = new WaitForSeconds(.025f);

        public static bool fly = false;
        public static bool godMode = false;
        public static bool noclip = false;
        public static bool freecam = false;
        public static bool invisibility = false;
        public static bool jumper = false;
        public static bool walkOnWater = false;
        public static bool antiDrowning = false;
        public static bool infiniteStamina = false;
        public static bool antiAim = false;
        public static bool antiAim_Movement = false;
        public static bool noFall = false;
        public static float autoDC_Health = 10f;
        public static float speed_walk = 4.5f;
        public static float speed_crouch = 2.5f;
        public static float speed_prone = 1.5f;
        public static float speed_sprint = 7f;
        public static float speed_climb = 4.5f;
        public static float speed_swim = 3f;
        public static float jump_height = 7f;
        public static bool autoDC = false;
        public static bool interactThroughWalls = false;
        public static bool farPunch = false;
        #endregion

        #region Mono Functions
        public void Start()
        {
            StartCoroutine(UpdateInteract());
        }
        public void Update()
        {
            if (!Variables.isInGame)
                return;

            if (Player.player != null && Player.player.transform != null)
            {
                #region Fly
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
                    if (Player.player.movement.gravity == 0f)
                        Player.player.movement.gravity = 1f;
                }
                #endregion

                #region Interact Through Walls
                if (interactThroughWalls && interactable != null && interactable.checkInteractable() && PlayerInteract.interactable == null)
                {
                    if (Input.GetKeyDown(ControlsSettings.interact))
                        wasDown = true;
                    if (Input.GetKeyUp(ControlsSettings.interact) && wasDown)
                    {
                        wasDown = false;
                        if (interactable.checkUseable())
                            interactable.use();
                    }
                }
                #endregion
            }
        }

        public void OnGUI()
        {
            if (Variables.isSpying)
                return;

            #region Interact Through Walls
            if (interactThroughWalls && interactable != null)
            {
                string text = "Interact";
                if (interactable is InteractableItem)
                    text = "Pickup " + interactable.GetComponent<InteractableItem>().asset.itemName;
                else if (interactable is InteractableBed)
                    text = (interactable.GetComponent<InteractableBed>().isClaimable ? "Unclaim" : "Claim");
                else if (interactable is InteractableStorage)
                    text = "Open storage";
                else if (interactable is InteractableVehicle)
                    text = "Enter vehicle";
                else if (interactable is InteractableGenerator)
                    text = "Turn " + (interactable.GetComponent<InteractableGenerator>().isPowered ? "Off" : "On");
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 20, 300, 80), string.Format("<size=14><color=lime>{0}</color></size>", text));
            }
            #endregion
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
            interactThroughWalls = GUILayout.Toggle(interactThroughWalls, "Interact Through Walls");
            farPunch = GUILayout.Toggle(farPunch, "Increase Punch Range");
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

        #region Coroutines
        private IEnumerator UpdateInteract()
        {
            while (true)
            {
                if (!interactThroughWalls || !Variables.isInGame)
                {
                    interactable = null;
                    yield return wfs;
                    continue;
                }

                RaycastHit hit;
                Physics.Raycast(Player.player.look.aim.position, Player.player.look.aim.forward, out hit, 64, RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.ITEM | RayMasks.RESOURCE);
                if (hit.transform != null)
                {
                    Transform transform = hit.transform;
                    interactable = transform.GetComponent<Interactable>();
                }
                else if (hit.transform == null)
                    interactable = null;

                yield return wfs;
            }
        }
        #endregion
    }
}
