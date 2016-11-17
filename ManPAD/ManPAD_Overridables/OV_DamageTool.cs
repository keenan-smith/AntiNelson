using System;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;

namespace ManPAD.ManPAD_Overridables
{
    internal class OV_DamageTool : MonoBehaviour // dont worry bout my old trash code
    {
        public static Player getNPlayer()
        {
            Player p = null;
            SteamPlayer[] plrs = Provider.clients.ToArray();
            for (int i = 0; i < plrs.Length; i++)
            {
                if (plrs[i].playerID.steamID != Provider.client && plrs[i].player.life != null && !plrs[i].player.life.isDead)
                {
                    if (Variables.fovBased)
                    {
                        Vector3 v3pos = Camera.main.WorldToScreenPoint(plrs[i].player.transform.position);
                        if (v3pos.z > 0f)
                        {
                            Vector2 pos = new Vector2(v3pos.x, v3pos.y);
                            float dist = Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2), pos);
                            if (dist <= Variables.aimFov* 2)
                            {
                                p = plrs[i].player;
                            }
                        }
                    }
                    else
                    {
                        if (p == null)
                        {
                            p = plrs[i].player;
                        }
                        else
                        {
                            if (Tools.getDistance(p.transform.position) > Tools.getDistance(plrs[i].player.transform.position))
                            {
                                p = plrs[i].player;
                            }
                        }
                    }
                }
            }
            return p;
        }

        [CodeReplace("raycast", typeof(DamageTool), BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)]
        public static RaycastInfo raycast(Ray ray, float range, int mask)
        {
            int num = mask;
            RaycastHit hit;
            Physics.Raycast(ray, out hit, range, num);
            RaycastInfo raycastInfo = new RaycastInfo(hit);
            raycastInfo.direction = ray.direction;
            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);
                }
                if (hit.transform.CompareTag("Zombie"))
                {
                    raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);
                }
                if (hit.transform.CompareTag("Animal"))
                {
                    raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);
                }
                raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);
                if (hit.transform.CompareTag("Vehicle"))
                {
                    raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);
                }
                if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
                {
                    raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
                }
                else
                {
                    raycastInfo.material = DamageTool.getMaterial(hit.point, hit.transform, hit.collider);
                }
                if (Variables.silentAim)
                {
                    raycastInfo.player = getNPlayer();
                    raycastInfo.limb = ELimb.SKULL;
                }
            }
            return raycastInfo;
        }
    }
}
