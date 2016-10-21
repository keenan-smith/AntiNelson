using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Attributes;
using PointBlank.API.Server.Extensions;

namespace PrefixAndSuffix
{
    [Plugin("Prefix And Suffix", "AtiLion", false, false)]
    public class Main : PBPlugin
    {
        public override void onLoad()
        {
            PBLogging.logImportant("Loading Prefix And Suffix by AtiLion...");

            string path = PB.getWorkingDirectory() + "\\Settings\\PrefixAndSuffix.dat";

            if (ReadWrite.fileExists(path, false, false))
            {
                PBConfig config = new PBConfig(path);

                Settings.usePrefixes = (config.getText("usePrefixes") == "true");
                Settings.useSuffixes = (config.getText("useSuffixes") == "true");
                Settings.ignoreAdminTag = (config.getText("ignoreAdminTag") == "true");
                Settings.ignoreProTag = (config.getText("ignoreProTag") == "true");
                foreach (string group in config.getChildNodesText("ignoreGroupList"))
                    Settings.ignoreGroupList.Add(group);
            }
            else
            {
                PBConfig config = new PBConfig();

                config.addTextElement("usePrefixes", "true");
                config.addTextElement("useSuffixes", "false");
                config.addTextElement("ignoreAdminTag", "false");
                config.addTextElement("ignoreProTag", "true");
                config.addTextElements("ignoreGroupList", "ignoredGroup", Settings.ignoreGroupList.ToArray());

                config.save(path);
            }

            PBServer.OnPreClientConnect += new PBServer.PreClientConnect(Event_ClientPreConnect);

            PBLogging.logImportant("Loaded Prefix And Suffix by AtiLion");
        }

        public void Event_ClientPreConnect(SteamPlayerID playerID, Vector3 point, byte angle, bool isPro, bool isAdmin, int channel, byte face, byte hair, byte beard, Color skin, Color color, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, EPlayerSkillset skillset)
        {
            try
            {
                PBPlayerSaveInfo saveInfo = PBServer.playerSave.loadPlayer(playerID.steamID.m_SteamID);
                string prefixes = "";
                string suffixes = "";

                if (saveInfo == null)
                {
                    foreach (PBGroup group in PBServer.groups)
                    {
                        if (group.isDefault)
                        {
                            if (Settings.ignoreGroupList.Contains(group.name))
                                continue;
                            if (Settings.usePrefixes)
                                prefixes += "[" + group.name + "]";
                            if (Settings.useSuffixes)
                                suffixes += "[" + group.name + "]";
                        }
                    }
                }
                else
                {
                    foreach (PBGroup group in saveInfo.groups)
                    {
                        if (Settings.ignoreGroupList.Contains(group.name))
                            continue;
                        if (Settings.usePrefixes)
                            prefixes += "[" + group.name + "]";
                        if (Settings.useSuffixes)
                            suffixes += "[" + group.name + "]";
                    }
                }
                if (isAdmin && !Settings.ignoreAdminTag)
                {
                    if (Settings.usePrefixes)
                        prefixes += "[Admin]";
                    if (Settings.useSuffixes)
                        suffixes += "[Admin]";
                }
                if (isPro && !Settings.ignoreProTag)
                {
                    if (Settings.usePrefixes)
                        prefixes += "[PRO]";
                    if (Settings.useSuffixes)
                        suffixes += "[PRO]";
                }

                typeof(SteamPlayerID).GetField("_characterName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(playerID, prefixes + playerID.characterName + suffixes);
                typeof(SteamPlayerID).GetField("_playerName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(playerID, prefixes + playerID.characterName + suffixes);
            }
            catch (Exception ex)
            {
                PBLogging.logError("Error in PrefixAndSuffix", ex);
            }
        }
    }
}
