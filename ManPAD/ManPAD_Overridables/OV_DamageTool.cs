using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using ManPAD.ManPAD_Hacks.MainMenu;

namespace ManPAD.ManPAD_Overridables
{
    public class OV_DamageTool : MonoBehaviour
    {
        [CodeReplace("raycast", typeof(DamageTool), BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)]
        public static RaycastInfo raycast(Ray ray, float range, int mask)
        {
            int num;
            if (MP_Weapons.attackThroughWalls)
                num = RayMasks.VEHICLE | RayMasks.ENEMY | RayMasks.ENTITY;
            else
                num = mask;

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

                #region SilentAim
                if (MP_Aimbot.silentAim)
                {
                    Player p = Tools.getNearestPlayer();

                    if (p == null)
                        return raycastInfo;

                    raycastInfo.player = p;
                    raycastInfo.limb = MP_Aimbot.aimLocation;
                }
                #endregion
            }
            return raycastInfo;
        }
    }
}
