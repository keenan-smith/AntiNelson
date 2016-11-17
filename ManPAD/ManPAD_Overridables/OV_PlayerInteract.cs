using System;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using ManPAD.ManPAD_Hacks.MainMenu;

namespace ManPAD.ManPAD_Overridables
{
    public class OV_PlayerInteract : MonoBehaviour
    {
        public float salvageTime
        {
            [CodeReplace("get_salvageTime", typeof(PlayerInteract), BindingFlags.Instance | BindingFlags.NonPublic)]
            get
            {
                return MP_Server.salvageSpeed;
            }
        }
    }
}
