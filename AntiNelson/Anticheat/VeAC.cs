using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using PointBlank.Anticheat.Enumerables;

namespace PointBlank.Anticheat
{
    [Plugin("Velocitiy Anticheat", "Kunii & AtiLion", false, false)]
    internal class VeAC : PBPlugin
    {
        public override void onLoad()
        {
            PBLogging.logImportant("Loading Velocity Anticheat...");

            string config_path = PB.getWorkingDirectory() + "\\Settings\\VeAC.dat";

            if (ReadWrite.fileExists(config_path, false, false))
            {
                PBConfig config = new PBConfig(config_path);
                config_Load(config);
            }
            else
            {
                PBConfig config = new PBConfig();
                config_Save(config, config_path);
            }

            PBLogging.logImportant("Velocity Anticheat loaded!");
        }

        #region Config Functions
        private void config_Load(PBConfig config)
        {
            VeAC_Settings.enabled = (config.getText("enabled") == "true");
            VeAC_Settings.anti_ESP = (config.getText("anti_ESP") == "true");
            VeAC_Settings.esp_prevent = (EAntiESP)Enum.Parse(typeof(EAntiESP), config.getText("esp_prevent"));
            VeAC_Settings.esp_distance = float.Parse(config.getText("esp_distance"));
            VeAC_Settings.anti_Aimbot = (config.getText("anti_Aimbot") == "true");
            VeAC_Settings.aimbot_antismooth = (config.getText("aimbot_antismooth") == "true");
            VeAC_Settings.anti_Triggerbot = (config.getText("anti_Triggerbot") == "true");
            VeAC_Settings.triggerbot_useMemory = (config.getText("triggerbot_useMemory") == "true");
            VeAC_Settings.triggerbot_time = float.Parse(config.getText("triggerbot_time"));
            VeAC_Settings.anti_VehicleFly = (config.getText("anti_VehicleFly") == "true");
            VeAC_Settings.anti_NoWall = (config.getText("anti_NoWall") == "true");
            VeAC_Settings.anti_AntiAim = (config.getText("anti_AntiAim") == "true");
            VeAC_Settings.anti_MapHack = (config.getText("anti_MapHack") == "true");
            VeAC_Settings.anti_AntiScreenshot = (config.getText("anti_AntiScreenshot") == "true");
            VeAC_Settings.antiscreenshot_usescreenshotdatabase = (config.getText("antiscreenshot_usescreenshotdatabase") == "true");
            VeAC_Settings.anti_InWallGlitch = (config.getText("anti_InWallGlitch") == "true");
            VeAC_Settings.anti_InstantDisconnect = (config.getText("anti_InstantDisconnect") == "true");
            VeAC_Settings.instantdisconnect_keepinserver = (config.getText("instantdisconnect_keepinserver") == "true");
            VeAC_Settings.anti_client_HashBypass = (config.getText("anti_client_HashBypass") == "true");
            VeAC_Settings.anti_client_Execution = (config.getText("anti_client_Execution") == "true");
            VeAC_Settings.anti_client_SkinHack = (config.getText("anti_client_SkinHack") == "true");
        }

        private void config_Save(PBConfig config, string path)
        {
            config.addTextElement("enabled", VeAC_Settings.enabled.ToString());
            config.addTextElement("anti_ESP", VeAC_Settings.anti_ESP.ToString());
            config.addTextElement("esp_prevent", VeAC_Settings.esp_prevent.ToString());
            config.addTextElement("esp_distance", VeAC_Settings.esp_distance.ToString());
            config.addTextElement("anti_Aimbot", VeAC_Settings.anti_Aimbot.ToString());
            config.addTextElement("aimbot_antismooth", VeAC_Settings.aimbot_antismooth.ToString());
            config.addTextElement("anti_Triggerbot", VeAC_Settings.anti_Triggerbot.ToString());
            config.addTextElement("triggerbot_useMemory", VeAC_Settings.triggerbot_useMemory.ToString());
            config.addTextElement("triggerbot_time", VeAC_Settings.triggerbot_time.ToString());
            config.addTextElement("anti_VehicleFly", VeAC_Settings.anti_VehicleFly.ToString());
            config.addTextElement("anti_NoWall", VeAC_Settings.anti_NoWall.ToString());
            config.addTextElement("anti_AntiAim", VeAC_Settings.anti_AntiAim.ToString());
            config.addTextElement("anti_MapHack", VeAC_Settings.anti_MapHack.ToString());
            config.addTextElement("anti_AntiScreenshot", VeAC_Settings.anti_AntiScreenshot.ToString());
            config.addTextElement("antiscreenshot_usescreenshotdatabase", VeAC_Settings.antiscreenshot_usescreenshotdatabase.ToString());
            config.addTextElement("anti_InWallGlitch", VeAC_Settings.anti_InWallGlitch.ToString());
            config.addTextElement("anti_InstantDisconnect", VeAC_Settings.anti_InstantDisconnect.ToString());
            config.addTextElement("instantdisconnect_keepinserver", VeAC_Settings.instantdisconnect_keepinserver.ToString());
            config.addTextElement("anti_client_HashBypass", VeAC_Settings.anti_client_HashBypass.ToString());
            config.addTextElement("anti_client_Execution", VeAC_Settings.anti_client_Execution.ToString());
            config.addTextElement("anti_client_SkinHack", VeAC_Settings.anti_client_SkinHack.ToString());
            config.save(path);
        }
        #endregion
    }
}
