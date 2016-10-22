using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.Anticheat.Enumerables;

namespace PointBlank.Anticheat
{
    internal class VeAC_Settings
    {
        public static bool enabled = true;
        public static bool anti_ESP = true;
        public static EAntiESP esp_prevent = EAntiESP.WALL_BLOCK;
        public static float esp_distance = 2000f;
        public static bool anti_Aimbot = true;
        public static bool anti_Triggerbot = true;
        public static bool triggerbot_useMemory = true;
        public static float triggerbot_time = 1f;
        public static bool anti_VehicleFly = true;
        public static bool anti_NoWall = true;
        public static bool anti_AntiAim = true;
        public static bool anti_MapHack = true;
        public static bool anti_AntiScreenshot = true;
        public static bool antiscreenshot_usescreenshotdatabase = true;
        public static bool anti_InWallGlitch = true;
        public static bool anti_InstantDisconnect = true;
        public static bool instantdisconnect_keepinserver = true;
        public static bool anti_Reach = true;

        public static bool anti_client_HashBypass = true;
        public static bool anti_client_Execution = true;
        public static bool anti_client_SkinHack = true;

        public static int max_detection = 5;
        public static bool ban_user = true;
        public static bool kick_user = false;
        public static bool warn_admins = true;
        public static bool warn_user = false;
    }
}
