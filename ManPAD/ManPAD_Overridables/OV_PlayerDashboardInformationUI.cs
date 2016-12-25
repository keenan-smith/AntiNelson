using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using ManPAD.ManPAD_Hacks.MainMenu;
using System.Reflection;

namespace ManPAD.ManPAD_Overridables
{
    public class OV_PlayerDashboardInformationUI : MonoBehaviour
    {
        [CodeReplace("refreshMap", typeof(PlayerDashboardInformationUI), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)]
        public void refreshMap(int view)
        {
            SleekImageTexture mapImage = (SleekImageTexture)typeof(PlayerDashboardInformationUI).GetField("mapImage", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Player.player);
            Texture2D mapTexture = (Texture2D)typeof(PlayerDashboardInformationUI).GetField("mapTexture", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Player.player);
            Texture2D chartTexture = (Texture2D)typeof(PlayerDashboardInformationUI).GetField("chartTexture", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Player.player);
            Texture2D staticTexture = (Texture2D)typeof(PlayerDashboardInformationUI).GetField("staticTexture", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Player.player);
            SleekLabel noLabel = (SleekLabel)typeof(PlayerDashboardInformationUI).GetField("noLabel", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Player.player);
            bool hasChart = (bool)typeof(PlayerDashboardInformationUI).GetField("hasChart", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Player.player);
            bool hasGPS = (bool)typeof(PlayerDashboardInformationUI).GetField("hasGPS", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Player.player);
            mapImage.remove();
            if (mapImage.texture != null && mapImage.shouldDestroyTexture)
            {
                UnityEngine.Object.Destroy(mapImage.texture);
                mapImage.texture = null;
            }
            if (view == 0)
            {
                if (chartTexture != null && (hasChart || Provider.modeConfigData.Gameplay.Chart || Level.info.type != ELevelType.SURVIVAL))
                {
                    mapImage.texture = chartTexture;
                    noLabel.isVisible = false;
                }
                else
                {
                    mapImage.texture = staticTexture;
                    noLabel.text = PlayerDashboardInformationUI.localization.format("No_Chart");
                    noLabel.isVisible = true;
                }
            }
            else if (mapTexture != null && (hasGPS || Level.info.type != ELevelType.SURVIVAL) || MP_Visual.GPS)
            {
                mapImage.texture = mapTexture;
                noLabel.isVisible = false;
            }
            else
            {
                mapImage.texture = staticTexture;
                noLabel.text = PlayerDashboardInformationUI.localization.format("No_GPS");
                noLabel.isVisible = true;
            }
            if (!noLabel.isVisible)
            {
                for (int i = 0; i < LevelNodes.nodes.Count; i++)
                {
                    Node node = LevelNodes.nodes[i];
                    if (node.type == ENodeType.LOCATION)
                    {
                        SleekLabel sleekLabel = new SleekLabel();
                        sleekLabel.positionOffset_X = -200;
                        sleekLabel.positionOffset_Y = -30;
                        sleekLabel.positionScale_X = node.point.x / (float)(Level.size - Level.border * 2) + 0.5f;
                        sleekLabel.positionScale_Y = 0.5f - node.point.z / (float)(Level.size - Level.border * 2);
                        sleekLabel.sizeOffset_X = 400;
                        sleekLabel.sizeOffset_Y = 60;
                        sleekLabel.text = ((LocationNode)node).name;
                        sleekLabel.foregroundTint = ESleekTint.FONT;
                        mapImage.add(sleekLabel);
                    }
                }
                if (Provider.modeConfigData.Gameplay.Group_Map)
                {
                    if (LevelManager.levelType == ELevelType.ARENA)
                    {
                        SleekImageTexture sleekImageTexture = new SleekImageTexture((Texture2D)PlayerDashboardInformationUI.icons.load("Area"));
                        sleekImageTexture.positionScale_X = LevelManager.arenaCenter.x / (float)(Level.size - Level.border * 2) + 0.5f - LevelManager.arenaRadius / (float)(Level.size - Level.border * 2);
                        sleekImageTexture.positionScale_Y = 0.5f - LevelManager.arenaCenter.z / (float)(Level.size - Level.border * 2) - LevelManager.arenaRadius / (float)(Level.size - Level.border * 2);
                        sleekImageTexture.sizeScale_X = LevelManager.arenaRadius * 2f / (float)(Level.size - Level.border * 2);
                        sleekImageTexture.sizeScale_Y = LevelManager.arenaRadius * 2f / (float)(Level.size - Level.border * 2);
                        mapImage.add(sleekImageTexture);
                        SleekImageTexture sleekImageTexture2 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
                        sleekImageTexture2.positionScale_Y = sleekImageTexture.positionScale_Y;
                        sleekImageTexture2.sizeScale_X = sleekImageTexture.positionScale_X;
                        sleekImageTexture2.sizeScale_Y = sleekImageTexture.sizeScale_Y;
                        mapImage.add(sleekImageTexture2);
                        SleekImageTexture sleekImageTexture3 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
                        sleekImageTexture3.positionScale_X = sleekImageTexture.positionScale_X + sleekImageTexture.sizeScale_X;
                        sleekImageTexture3.positionScale_Y = sleekImageTexture.positionScale_Y;
                        sleekImageTexture3.sizeScale_X = 1f - sleekImageTexture.positionScale_X - sleekImageTexture.sizeScale_X;
                        sleekImageTexture3.sizeScale_Y = sleekImageTexture.sizeScale_Y;
                        mapImage.add(sleekImageTexture3);
                        SleekImageTexture sleekImageTexture4 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
                        sleekImageTexture4.sizeScale_X = 1f;
                        sleekImageTexture4.sizeScale_Y = sleekImageTexture.positionScale_Y;
                        mapImage.add(sleekImageTexture4);
                        SleekImageTexture sleekImageTexture5 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
                        sleekImageTexture5.positionScale_Y = sleekImageTexture.positionScale_Y + sleekImageTexture.sizeScale_Y;
                        sleekImageTexture5.sizeScale_X = 1f;
                        sleekImageTexture5.sizeScale_Y = 1f - sleekImageTexture.positionScale_Y - sleekImageTexture.sizeScale_Y;
                        mapImage.add(sleekImageTexture5);
                    }
                    if (MP_Visual.showPlayersOnMap)
                    {
                        for (int j = 0; j < Provider.clients.Count; j++)
                        {
                            SteamPlayer steamPlayer = Provider.clients[j];
                            if (steamPlayer.playerID.steamID != Provider.client && steamPlayer.model != null)
                            {
                                SleekImageTexture sleekImageTexture6 = new SleekImageTexture();
                                sleekImageTexture6.positionOffset_X = -10;
                                sleekImageTexture6.positionOffset_Y = -10;
                                sleekImageTexture6.positionScale_X = steamPlayer.player.transform.position.x / (float)(Level.size - Level.border * 2) + 0.5f;
                                sleekImageTexture6.positionScale_Y = 0.5f - steamPlayer.player.transform.position.z / (float)(Level.size - Level.border * 2);
                                sleekImageTexture6.sizeOffset_X = 20;
                                sleekImageTexture6.sizeOffset_Y = 20;
                                if (!OptionsSettings.streamer)
                                {
                                    sleekImageTexture6.texture = Provider.provider.communityService.getIcon(steamPlayer.playerID.steamID);
                                }
                                if (string.IsNullOrEmpty(steamPlayer.playerID.nickName))
                                {
                                    sleekImageTexture6.addLabel(steamPlayer.isAdmin ? "[ADMIN]" : "" + steamPlayer.playerID.characterName, ESleekSide.RIGHT);
                                }
                                else
                                {
                                    sleekImageTexture6.addLabel(steamPlayer.isAdmin ? "[ADMIN]" : "" + steamPlayer.playerID.nickName, ESleekSide.RIGHT);
                                }
                                sleekImageTexture6.shouldDestroyTexture = true;
                                mapImage.add(sleekImageTexture6);
                            }
                        }
                    }
                    else
                    {
                        if (Characters.active.group != CSteamID.Nil)
                        {
                            for (int j = 0; j < PlayerGroupUI.groups.Count; j++)
                            {
                                SteamPlayer steamPlayer = Provider.clients[j];
                                if (steamPlayer.playerID.steamID != Provider.client && steamPlayer.playerID.group == Characters.active.group && steamPlayer.model != null)
                                {
                                    SleekImageTexture sleekImageTexture6 = new SleekImageTexture();
                                    sleekImageTexture6.positionOffset_X = -10;
                                    sleekImageTexture6.positionOffset_Y = -10;
                                    sleekImageTexture6.positionScale_X = steamPlayer.player.transform.position.x / (float)(Level.size - Level.border * 2) + 0.5f;
                                    sleekImageTexture6.positionScale_Y = 0.5f - steamPlayer.player.transform.position.z / (float)(Level.size - Level.border * 2);
                                    sleekImageTexture6.sizeOffset_X = 20;
                                    sleekImageTexture6.sizeOffset_Y = 20;
                                    if (!OptionsSettings.streamer)
                                    {
                                        sleekImageTexture6.texture = Provider.provider.communityService.getIcon(steamPlayer.playerID.steamID);
                                    }
                                    if (string.IsNullOrEmpty(steamPlayer.playerID.nickName))
                                    {
                                        sleekImageTexture6.addLabel(steamPlayer.playerID.characterName, ESleekSide.RIGHT);
                                    }
                                    else
                                    {
                                        sleekImageTexture6.addLabel(steamPlayer.playerID.nickName, ESleekSide.RIGHT);
                                    }
                                    sleekImageTexture6.shouldDestroyTexture = true;
                                    mapImage.add(sleekImageTexture6);
                                }
                            }
                        }
                    }
                    if (Player.player != null)
                    {
                        SleekImageTexture sleekImageTexture7 = new SleekImageTexture();
                        sleekImageTexture7.positionOffset_X = -10;
                        sleekImageTexture7.positionOffset_Y = -10;
                        sleekImageTexture7.positionScale_X = Player.player.transform.position.x / (float)(Level.size - Level.border * 2) + 0.5f;
                        sleekImageTexture7.positionScale_Y = 0.5f - Player.player.transform.position.z / (float)(Level.size - Level.border * 2);
                        sleekImageTexture7.sizeOffset_X = 20;
                        sleekImageTexture7.sizeOffset_Y = 20;
                        sleekImageTexture7.isAngled = true;
                        sleekImageTexture7.angle = Player.player.transform.rotation.eulerAngles.y;
                        sleekImageTexture7.texture = (Texture2D)PlayerDashboardInformationUI.icons.load("Player");
                        sleekImageTexture7.backgroundTint = ESleekTint.FOREGROUND;
                        if (string.IsNullOrEmpty(Characters.active.nick))
                        {
                            sleekImageTexture7.addLabel(Characters.active.name, ESleekSide.RIGHT);
                        }
                        else
                        {
                            sleekImageTexture7.addLabel(Characters.active.nick, ESleekSide.RIGHT);
                        }
                        mapImage.add(sleekImageTexture7);
                    }
                }
            }
        }
    }
}
