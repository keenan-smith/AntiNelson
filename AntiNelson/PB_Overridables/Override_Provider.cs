using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.Analytics;
using Steamworks;
using PointBlank.API.Attributes;
using PointBlank.API.Server;

namespace PointBlank.PB_Overridables
{
    internal class Override_Provider
    {
        [ReplaceCode(typeof(Provider), "addPlayer", BindingFlags.Static | BindingFlags.NonPublic)]
        public static void addPlayer(SteamPlayerID playerID, Vector3 point, byte angle, bool isPro, bool isAdmin, int channel, byte face, byte hair, byte beard, Color skin, Color color, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, EPlayerSkillset skillset)
        {
            PBServer.preJoinServer(playerID, point, angle, isPro, isAdmin, channel, face, hair, beard, skin, color, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, skillset);
            if (!Dedicator.isDedicated && playerID.steamID != Provider.client)
            {
                SteamFriends.SetPlayedWith(playerID.steamID);
            }
            if (playerID.steamID == Provider.client)
            {
                string value = skillset.ToString();
                int num = 0;
                int num2 = 0;
                if (shirtItem != 0)
                {
                    num++;
                    if (Provider.provider.economyService.getInventoryMythicID(shirtItem) != 0)
                    {
                        num2++;
                    }
                }
                if (pantsItem != 0)
                {
                    num++;
                    if (Provider.provider.economyService.getInventoryMythicID(pantsItem) != 0)
                    {
                        num2++;
                    }
                }
                if (hatItem != 0)
                {
                    num++;
                    if (Provider.provider.economyService.getInventoryMythicID(hatItem) != 0)
                    {
                        num2++;
                    }
                }
                if (backpackItem != 0)
                {
                    num++;
                    if (Provider.provider.economyService.getInventoryMythicID(backpackItem) != 0)
                    {
                        num2++;
                    }
                }
                if (vestItem != 0)
                {
                    num++;
                    if (Provider.provider.economyService.getInventoryMythicID(vestItem) != 0)
                    {
                        num2++;
                    }
                }
                if (maskItem != 0)
                {
                    num++;
                    if (Provider.provider.economyService.getInventoryMythicID(maskItem) != 0)
                    {
                        num2++;
                    }
                }
                if (glassesItem != 0)
                {
                    num++;
                    if (Provider.provider.economyService.getInventoryMythicID(glassesItem) != 0)
                    {
                        num2++;
                    }
                }
                int num3 = skinItems.Length;
                for (int i = 0; i < skinItems.Length; i++)
                {
                    if (Provider.provider.economyService.getInventoryMythicID(skinItems[i]) != 0)
                    {
                        num2++;
                    }
                }
                Dictionary<string, object> eventData = new Dictionary<string, object>
		        {
			        {
				        "Ability",
				        value
			        },
			        {
				        "Cosmetics",
				        num
			        },
			        {
				        "Mythics",
				        num2
			        },
			        {
				        "Skins",
				        num3
			        }
		        };
                Analytics.CustomEvent("Character", eventData);
            }
            Transform transform;
            if (Dedicator.isDedicated)
            {
                transform = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/Player_Dedicated"))).transform;
            }
            else if (playerID.steamID == Provider.client)
            {
                transform = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/Player_Server"))).transform;
            }
            else
            {
                transform = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Characters/Player_Client"))).transform;
            }
            transform.position = point;
            transform.rotation = Quaternion.Euler(0f, (float)(angle * 2), 0f);
            Provider.clients.Add(new SteamPlayer(playerID, transform, isPro, isAdmin, channel, face, hair, beard, skin, color, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, skillset));
            if (Provider.onEnemyConnected != null)
            {
                Provider.onEnemyConnected(Provider.clients[Provider.clients.Count - 1]);
            }
        }
    }
}
