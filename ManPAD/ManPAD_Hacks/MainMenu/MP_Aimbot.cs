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
    [MenuOption(1, "Aimbot", 200f, 310f)]
    public class MP_Aimbot : MenuOption
    {
        #region Variables
        private WaitForSeconds wfs = new WaitForSeconds(0.5f);
        private WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        private ELimb[] limbs = { ELimb.SKULL, ELimb.SPINE };
        private EAttackPriority[] prioritys = { EAttackPriority.DISTANCE, EAttackPriority.THREAT };
        public static float cDistance = -1f;
        public static object nextTarget = null;

        public static bool ignoreFOV = false;
        public static float FOV = 90f;
        public static bool ignoreDistance = false;
        public static float distance = 1000f;
        public static ELimb aimLocation = ELimb.SKULL;
        public static EAttackPriority attackPriority = EAttackPriority.DISTANCE;

        public static bool aimbot = false;
        public static bool autoTrigger = true;

        public static bool silentAim = false;

        public static bool aim_players = true;
        public static bool aim_friends = false;
        public static bool aim_zombies = false;
        public static bool aim_animals = false;
        #endregion

        #region Mono Functions
        public void Start()
        {
            StartCoroutine(getAttackingPlayer());
            StartCoroutine(getAttackingZombie());
            StartCoroutine(getAttackingAnimal());
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            ignoreFOV = GUILayout.Toggle(ignoreFOV, "Ignore FOV");
            GUILayout.Label("Aim FOV: " + FOV);
            FOV = (float)Math.Round(GUILayout.HorizontalSlider(FOV, 1f, 45f));
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
            aim_zombies = GUILayout.Toggle(aim_zombies, "Attack Zombies");
            aim_animals = GUILayout.Toggle(aim_animals, "Attack Animals");
        }
        #endregion

        #region Coroutines
        private IEnumerator getAttackingPlayer()
        {
            while (true)
            {
                if (!(aimbot || silentAim) || !aim_players || !Variables.isInGame || (Variables.players == null || Variables.players.Length < 1))
                {
                    yield return wfs;
                    continue;
                }

                foreach (SteamPlayer p in Variables.players)
                {
                    if (p == null || p.player == null || p.player.gameObject == null || p.player.life.isDead)
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

                    yield return wfeof;
                }

                yield return wfs;
            }
        }

        private IEnumerator getAttackingZombie()
        {
            while (true)
            {
                if (!(aimbot || silentAim) || !aim_zombies || !Variables.isInGame || (Variables.zombies == null || Variables.zombies.Length < 1))
                {
                    yield return wfs;
                    continue;
                }

                foreach (Zombie z in Variables.zombies)
                {
                    if (z == null || z.gameObject == null || z.isDead)
                        continue;

                    float pDistance = Tools.getDistance(z.transform.position);

                    if (pDistance > distance && !ignoreDistance)
                        continue;

                    if (cDistance == -1f || pDistance < cDistance)
                    {
                        cDistance = pDistance;
                        nextTarget = z;
                    }

                    yield return wfeof;
                }

                yield return wfs;
            }
        }

        private IEnumerator getAttackingAnimal()
        {
            while (true)
            {
                if (!(aimbot || silentAim) || !aim_animals || !Variables.isInGame || (Variables.animals == null || Variables.animals.Length < 1))
                {
                    yield return wfs;
                    continue;
                }

                foreach (Animal a in Variables.animals)
                {
                    if (a == null || a.gameObject == null || a.isDead)
                        continue;

                    float pDistance = Tools.getDistance(a.transform.position);

                    if (pDistance > distance && !ignoreDistance)
                        continue;

                    if (cDistance == -1f || pDistance < cDistance)
                    {
                        cDistance = pDistance;
                        nextTarget = a;
                    }

                    yield return wfeof;
                }

                yield return wfs;
            }
        }
        #endregion
    }
}
