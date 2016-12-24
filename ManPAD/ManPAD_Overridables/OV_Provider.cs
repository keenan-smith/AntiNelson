using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using SDG.Unturned;
using UnityEngine;
using SDG.Framework.Translations;
using SDG.Framework.Modules;

namespace ManPAD.ManPAD_Overridables
{
    public class OV_Provider : MonoBehaviour
    {
        public static bool isPro
        {
            [CodeReplace("get_isPro", typeof(Provider), BindingFlags.Public | BindingFlags.Static)]
            get
            {
                return true;
            }
        }

        //[CodeReplace("onLevelLoaded", typeof(Provider), BindingFlags.NonPublic | BindingFlags.Static)]
        public static void onLevelLoaded(int level)
        {
            if (level == 2)
            {
                MethodInfo addPlayer = typeof(Provider).GetMethod("addPlayer", BindingFlags.NonPublic | BindingFlags.Static);
                Provider.isLoadingUGC = false;
                if (Provider.isConnected)
                {
                    if (Provider.isServer)
                    {
                        if (Provider.isClient)
                        {
                            SteamPlayerID steamPlayerID = new SteamPlayerID(Provider.client, Characters.selected, Provider.clientName, Characters.active.name, Characters.active.nick, Characters.active.group);
                            Vector3 point = Vector3.zero;
                            byte angle;
                            if (PlayerSavedata.fileExists(steamPlayerID, "/Player/Player.dat") && Level.info.type == ELevelType.SURVIVAL)
                            {
                                Block block = PlayerSavedata.readBlock(steamPlayerID, "/Player/Player.dat", 1);
                                point = block.readSingleVector3() + new Vector3(0f, 0.5f, 0f);
                                angle = block.readByte();
                            }
                            else
                            {
                                PlayerSpawnpoint spawn = LevelPlayers.getSpawn(false);
                                point = spawn.point + new Vector3(0f, 0.5f, 0f);
                                angle = (byte)(spawn.angle / 2f);
                            }
                            int inventoryItem = Provider.provider.economyService.getInventoryItem(Characters.active.packageShirt);
                            int inventoryItem2 = Provider.provider.economyService.getInventoryItem(Characters.active.packagePants);
                            int inventoryItem3 = Provider.provider.economyService.getInventoryItem(Characters.active.packageHat);
                            int inventoryItem4 = Provider.provider.economyService.getInventoryItem(Characters.active.packageBackpack);
                            int inventoryItem5 = Provider.provider.economyService.getInventoryItem(Characters.active.packageVest);
                            int inventoryItem6 = Provider.provider.economyService.getInventoryItem(Characters.active.packageMask);
                            int inventoryItem7 = Provider.provider.economyService.getInventoryItem(Characters.active.packageGlasses);
                            int[] array = new int[Characters.packageSkins.Count];
                            for (int i = 0; i < array.Length; i++)
                            {
                                array[i] = Provider.provider.economyService.getInventoryItem(Characters.packageSkins[i]);
                            }
                            addPlayer.Invoke(null, new object[] { steamPlayerID, point, angle, Provider.isPro, true, Provider.channels, Characters.active.face, Characters.active.hair, Characters.active.beard, Characters.active.skin, Characters.active.color, Characters.active.hand, inventoryItem, inventoryItem2, inventoryItem3, inventoryItem4, inventoryItem5, inventoryItem6, inventoryItem7, array, Characters.active.skillset, Translator.language });
                            if (Provider.onServerConnected != null)
                            {
                                Provider.onServerConnected(steamPlayerID.steamID);
                            }
                        }
                    }
                    else
                    {
                        List<SDG.Framework.Modules.Module> critMods = (List<SDG.Framework.Modules.Module>)typeof(Provider).GetField("critMods", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                        StringBuilder modBuilder = (StringBuilder)typeof(Provider).GetField("modBuilder", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                        byte b = 1;
                        critMods.Clear();
                        modBuilder.Length = 0;
                        ModuleHook.getRequiredModules(critMods);
                        for (int j = 0; j < critMods.Count; j++)
                        {
                            modBuilder.Append(critMods[j].config.Name);
                            modBuilder.Append(",");
                            modBuilder.Append(critMods[j].config.Version_Internal);
                            if (j < critMods.Count - 1)
                            {
                                modBuilder.Append(";");
                            }
                        }
                        int size;
                        bool pro = (bool)typeof(Provider).GetField("_isPro", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                        byte[] bytes = SteamPacker.getBytes(0, out size, new object[]
				        {
					        (byte)2,
					        (pro ? (Characters.selected > 5 ? 0 : Characters.selected) : 0),
					        Provider.clientName,
					        Provider.clientName,
					        (byte[])typeof(Provider).GetField("_serverPasswordHash", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null),
					        Level.hash,
					        ReadWrite.appOut(),
					        b,
					        Provider.APP_VERSION,
					        pro,
					        (float)Provider.currentServerInfo.ping / 1000f,
					        Characters.active.nick,
					        Characters.active.group,
					        Characters.active.face,
					        Characters.active.hair,
					        Characters.active.beard,
					        Characters.active.skin,
					        Characters.active.color,
					        Characters.active.hand,
					        Characters.active.packageShirt,
					        Characters.active.packagePants,
					        Characters.active.packageHat,
					        Characters.active.packageBackpack,
					        Characters.active.packageVest,
					        Characters.active.packageMask,
					        Characters.active.packageGlasses,
					        Characters.packageSkins.ToArray(),
					        (byte)Characters.active.skillset,
					        modBuilder.ToString(),
					        Translator.language
				        });
                        Provider.send(Provider.server, ESteamPacket.CONNECT, bytes, size, 0);
                    }
                }
            }
        }
    }
}
