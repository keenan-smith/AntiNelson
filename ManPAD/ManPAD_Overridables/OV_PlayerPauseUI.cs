using System;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;

namespace ManPAD.ManPAD_Overridables
{
    internal class OV_PlayerPauseUI : MonoBehaviour
    {
        [CodeReplace("onClickedExitButton", typeof(PlayerPauseUI), BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)]
        public static void onClickedExitButton(SleekButton button)
        {
            Provider.disconnect();
        }
    }
}
