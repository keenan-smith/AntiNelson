using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using System.Reflection;
using ManPAD.ManPAD_API.Attributes;
using ManPAD.ManPAD_Hacks.MainMenu;

namespace ManPAD.ManPAD_Overridables
{
    public class OV_UseableBarricade : MonoBehaviour
    {
        public bool checkSpace()
        {
            return true;
        }

        public void build()
        {
            typeof(UseableBarricade).GetField("startedUse", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableBarricade)Player.player.equipment.useable, Time.realtimeSinceStartup);
            typeof(UseableBarricade).GetField("isUsing", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableBarricade)Player.player.equipment.useable, true);
            typeof(UseableBarricade).GetField("isBuilding", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableBarricade)Player.player.equipment.useable, true);
        }
    }
    public class OV_UseableStructure : MonoBehaviour
    {
        public bool checkSpace()
        {
            return true;
        }

        public void construct()
        {
            typeof(UseableStructure).GetField("startedUse", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableStructure)Player.player.equipment.useable, Time.realtimeSinceStartup);
            typeof(UseableStructure).GetField("isUsing", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableStructure)Player.player.equipment.useable, true);
            typeof(UseableStructure).GetField("isConstructing", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((UseableStructure)Player.player.equipment.useable, true);
        }
    }
}
