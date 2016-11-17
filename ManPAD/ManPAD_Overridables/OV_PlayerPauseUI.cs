using System;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using ManPAD.ManPAD_Hacks.MainMenu;

namespace ManPAD.ManPAD_Overridables
{
    public class OV_PlayerPauseUI : MonoBehaviour
    {
        [CodeReplace("onClickedExitButton", typeof(PlayerPauseUI), BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)]
        public static void onClickedExitButton(SleekButton button)
        {
            if(!MP_Server.instantDisconnect)
                if (!Provider.isServer && Provider.isPvP && Provider.clients.Count > 1 && !Player.player.movement.isSafe && !Player.player.life.isDead && Time.realtimeSinceStartup - PlayerPauseUI.lastLeave < Provider.modeConfigData.Gameplay.Timer_Exit)
                    return;
            Provider.disconnect();
        }
    }
}
