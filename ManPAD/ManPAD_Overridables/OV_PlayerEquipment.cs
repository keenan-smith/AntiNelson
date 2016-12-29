using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using ManPAD.ManPAD_Hacks.MainMenu;

namespace ManPAD.ManPAD_Overridables
{
#if DEBUG
    public class OV_PlayerEquipment : MonoBehaviour
    {
        private static readonly AnimalDamageMultiplier DAMAGE_ANIMAL_MULTIPLIER = new AnimalDamageMultiplier(15f, 0.3f, 0.6f, 1.1f);
        private static readonly float DAMAGE_BARRICADE = 2f;
        private static readonly float DAMAGE_OBJECT = 5f;
        private static readonly PlayerDamageMultiplier DAMAGE_PLAYER_MULTIPLIER = new PlayerDamageMultiplier(15f, 0.6f, 0.6f, 0.8f, 1.1f);
        private static readonly float DAMAGE_RESOURCE = 0f;
        private static readonly float DAMAGE_STRUCTURE = 2f;
        private static readonly float DAMAGE_VEHICLE = 0f;
        private static readonly ZombieDamageMultiplier DAMAGE_ZOMBIE_MULTIPLIER = new ZombieDamageMultiplier(15f, 0.3f, 0.3f, 0.6f, 1.1f);
        private uint lastPunch;
        private float lastEquip;
        private bool lastPrimary;
        private bool lastSecondary;

        [CodeReplace("punch", typeof(PlayerEquipment), BindingFlags.Instance | BindingFlags.NonPublic)]
        public void punch(EPlayerPunch mode)
        {
            if (Player.player.channel.isOwner)
            {
                Player.player.playSound((AudioClip)Resources.Load("Sounds/General/Punch"));
                Ray ray = new Ray(Player.player.look.aim.position, Player.player.look.aim.forward);
                RaycastInfo raycastInfo = DamageTool.raycast(ray, MP_Player.farPunch ? 12f : 1.75f, RayMasks.DAMAGE_CLIENT); //1.75 default
                if (raycastInfo.player != null && DAMAGE_PLAYER_MULTIPLIER.damage > 1f && (Player.player.channel.owner.playerID.group == CSteamID.Nil || raycastInfo.player.channel.owner.playerID.group != Player.player.channel.owner.playerID.group) && Provider.isPvP)
                {
                    PlayerUI.hitmark(0, raycastInfo.point, false, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                }
                else if ((raycastInfo.zombie != null && DAMAGE_ZOMBIE_MULTIPLIER.damage > 1f) || (raycastInfo.animal != null && DAMAGE_ANIMAL_MULTIPLIER.damage > 1f))
                {
                    PlayerUI.hitmark(0, raycastInfo.point, false, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
                }
                else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Barricade") && DAMAGE_BARRICADE > 1f)
                {
                    InteractableDoorHinge component = raycastInfo.transform.GetComponent<InteractableDoorHinge>();
                    if (component != null)
                    {
                        raycastInfo.transform = component.transform.parent.parent;
                    }
                    ushort id;
                    if (ushort.TryParse(raycastInfo.transform.name, out id))
                    {
                        ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id);
                        if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable)
                        {
                            PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
                        }
                    }
                }
                else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Structure") && DAMAGE_STRUCTURE > 1f)
                {
                    ushort id2;
                    if (ushort.TryParse(raycastInfo.transform.name, out id2))
                    {
                        ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id2);
                        if (itemStructureAsset != null && itemStructureAsset.isVulnerable)
                        {
                            PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
                        }
                    }
                }
                else if (raycastInfo.vehicle != null && !raycastInfo.vehicle.isDead && DAMAGE_VEHICLE > 1f)
                {
                    if (raycastInfo.vehicle.asset != null && raycastInfo.vehicle.asset.isVulnerable)
                    {
                        PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
                    }
                }
                else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Resource") && DAMAGE_RESOURCE > 1f)
                {
                    byte x;
                    byte y;
                    ushort index;
                    if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
                    {
                        ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
                        if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead)
                        {
                            PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
                        }
                    }
                }
                else if (raycastInfo.transform != null && DAMAGE_OBJECT > 1f)
                {
                    InteractableObjectRubble component2 = raycastInfo.transform.GetComponent<InteractableObjectRubble>();
                    if (component2 != null)
                    {
                        raycastInfo.section = component2.getSection(raycastInfo.collider.transform);
                        if (!component2.isSectionDead(raycastInfo.section))
                        {
                            PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
                        }
                    }
                }
                Player.player.input.sendRaycast(raycastInfo);
            }
            if (mode == EPlayerPunch.LEFT)
            {
                Player.player.animator.play("Punch_Left", false);
                if (Provider.isServer)
                {
                    Player.player.animator.sendGesture(EPlayerGesture.PUNCH_LEFT, false);
                }
            }
            else if (mode == EPlayerPunch.RIGHT)
            {
                Player.player.animator.play("Punch_Right", false);
                if (Provider.isServer)
                {
                    Player.player.animator.sendGesture(EPlayerGesture.PUNCH_RIGHT, false);
                }
            }
            if (Provider.isServer)
            {
                if (!Player.player.input.hasInputs())
                {
                    return;
                }
                InputInfo input = Player.player.input.getInput(true);
                if (input == null)
                {
                    return;
                }
                if ((input.point - Player.player.look.aim.position).sqrMagnitude > 36f)
                {
                    return;
                }
                DamageTool.impact(input.point, input.normal, input.material, input.type != ERaycastInfoType.NONE && input.type != ERaycastInfoType.OBJECT);
                EPlayerKill ePlayerKill = EPlayerKill.NONE;
                uint num = 0u;
                float num2 = 1f;
                num2 *= 1f + Player.player.channel.owner.player.skills.mastery(0, 0) * 0.5f;
                if (input.type == ERaycastInfoType.PLAYER)
                {
                    typeof(PlayerEquipment).GetField("lastPunching", BindingFlags.Instance | BindingFlags.Public).SetValue(null, Time.realtimeSinceStartup);
                    if (input.player != null && (Player.player.channel.owner.playerID.group == CSteamID.Nil || input.player.channel.owner.playerID.group != Player.player.channel.owner.playerID.group) && Provider.isPvP)
                    {
                        DamageTool.damage(input.player, EDeathCause.PUNCH, input.limb, Player.player.channel.owner.playerID.steamID, input.direction, DAMAGE_PLAYER_MULTIPLIER, num2, true, out ePlayerKill);
                    }
                }
                else if (input.type == ERaycastInfoType.ZOMBIE)
                {
                    if (input.zombie != null)
                    {
                        DamageTool.damage(input.zombie, input.limb, input.direction, DAMAGE_ZOMBIE_MULTIPLIER, num2, true, out ePlayerKill, out num);
                        if (Player.player.movement.nav != 255)
                        {
                            input.zombie.alert(base.transform.position, true);
                        }
                    }
                }
                else if (input.type == ERaycastInfoType.ANIMAL)
                {
                    typeof(PlayerEquipment).GetField("lastPunching", BindingFlags.Instance | BindingFlags.Public).SetValue(null, Time.realtimeSinceStartup);
                    if (input.animal != null)
                    {
                        DamageTool.damage(input.animal, input.limb, input.direction, DAMAGE_ANIMAL_MULTIPLIER, num2, out ePlayerKill, out num);
                        input.animal.alertPoint(base.transform.position, true);
                    }
                }
                else if (input.type == ERaycastInfoType.VEHICLE)
                {
                    typeof(PlayerEquipment).GetField("lastPunching", BindingFlags.Instance | BindingFlags.Public).SetValue(null, Time.realtimeSinceStartup);
                    if (input.vehicle != null && input.vehicle.asset != null && input.vehicle.asset.isVulnerable)
                    {
                        DamageTool.damage(input.vehicle, false, DAMAGE_VEHICLE, num2, true, out ePlayerKill);
                    }
                }
                else if (input.type == ERaycastInfoType.BARRICADE)
                {
                    typeof(PlayerEquipment).GetField("lastPunching", BindingFlags.Instance | BindingFlags.Public).SetValue(null, Time.realtimeSinceStartup);
                    ushort id3;
                    if (input.transform != null && input.transform.CompareTag("Barricade") && ushort.TryParse(input.transform.name, out id3))
                    {
                        ItemBarricadeAsset itemBarricadeAsset2 = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id3);
                        if (itemBarricadeAsset2 != null && itemBarricadeAsset2.isVulnerable)
                        {
                            DamageTool.damage(input.transform, false, DAMAGE_BARRICADE, num2, out ePlayerKill);
                        }
                    }
                }
                else if (input.type == ERaycastInfoType.STRUCTURE)
                {
                    typeof(PlayerEquipment).GetField("lastPunching", BindingFlags.Instance | BindingFlags.Public).SetValue(null, Time.realtimeSinceStartup);
                    ushort id4;
                    if (input.transform != null && input.transform.CompareTag("Structure") && ushort.TryParse(input.transform.name, out id4))
                    {
                        ItemStructureAsset itemStructureAsset2 = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id4);
                        if (itemStructureAsset2 != null && itemStructureAsset2.isVulnerable)
                        {
                            DamageTool.damage(input.transform, false, input.direction, DAMAGE_STRUCTURE, num2, out ePlayerKill);
                        }
                    }
                }
                else if (input.type == ERaycastInfoType.RESOURCE)
                {
                    typeof(PlayerEquipment).GetField("lastPunching", BindingFlags.Instance | BindingFlags.Public).SetValue(null, Time.realtimeSinceStartup);
                    byte x2;
                    byte y2;
                    ushort index2;
                    if (input.transform != null && input.transform.CompareTag("Resource") && ResourceManager.tryGetRegion(input.transform, out x2, out y2, out index2))
                    {
                        ResourceSpawnpoint resourceSpawnpoint2 = ResourceManager.getResourceSpawnpoint(x2, y2, index2);
                        if (resourceSpawnpoint2 != null && !resourceSpawnpoint2.isDead)
                        {
                            DamageTool.damage(input.transform, input.direction, DAMAGE_RESOURCE, num2, 1f, out ePlayerKill, out num);
                        }
                    }
                }
                else if (input.type == ERaycastInfoType.OBJECT && input.transform != null && input.section < 255)
                {
                    InteractableObjectRubble component3 = input.transform.GetComponent<InteractableObjectRubble>();
                    if (component3 != null && !component3.isSectionDead(input.section))
                    {
                        DamageTool.damage(input.transform, input.direction, input.section, DAMAGE_OBJECT, num2, out ePlayerKill, out num);
                    }
                }
                if (Level.info.type == ELevelType.HORDE)
                {
                    if (input.zombie != null)
                    {
                        if (input.limb == ELimb.SKULL)
                        {
                            Player.player.skills.askPay(10u);
                        }
                        else
                        {
                            Player.player.skills.askPay(5u);
                        }
                    }
                    if (ePlayerKill == EPlayerKill.ZOMBIE)
                    {
                        if (input.limb == ELimb.SKULL)
                        {
                            Player.player.skills.askPay(50u);
                        }
                        else
                        {
                            Player.player.skills.askPay(25u);
                        }
                    }
                }
                else
                {
                    if (ePlayerKill == EPlayerKill.PLAYER)
                    {
                        Player.player.sendStat(EPlayerStat.KILLS_PLAYERS);
                        if (Level.info.type == ELevelType.ARENA)
                        {
                            Player.player.skills.askPay(100u);
                        }
                    }
                    else if (ePlayerKill == EPlayerKill.ZOMBIE)
                    {
                        Player.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
                    }
                    else if (ePlayerKill == EPlayerKill.MEGA)
                    {
                        Player.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
                    }
                    else if (ePlayerKill == EPlayerKill.ANIMAL)
                    {
                        Player.player.sendStat(EPlayerStat.KILLS_ANIMALS);
                    }
                    else if (ePlayerKill == EPlayerKill.RESOURCE)
                    {
                        Player.player.sendStat(EPlayerStat.FOUND_RESOURCES);
                    }
                    if (num > 0u)
                    {
                        Player.player.skills.askPay(num);
                    }
                }
            }
        }
    }
#endif
}
