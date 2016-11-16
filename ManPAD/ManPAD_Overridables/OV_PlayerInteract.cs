using System;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;

namespace ManPAD.ManPAD_Overridables
{
    internal class OV_PlayerInteract : MonoBehaviour
    {
        public float salvageTime
        {
            [CodeReplace("get_salvageTime", typeof(PlayerInteract), BindingFlags.Instance | BindingFlags.NonPublic)]
            get
            {
                return 0.5f;
            }
        }
    }
}
