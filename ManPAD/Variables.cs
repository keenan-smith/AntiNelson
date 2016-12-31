using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_Overridables;

namespace ManPAD
{
    public class Variables
    {
        public static AssetBundle bundle;
        public static AssetBundle bundle_chams;

        public static Player LocalPlayer;
        public static SteamPlayer LocalSteamPlayer;
        public static bool isInGame = false;
        public static bool isSpying = false;
        public static GameObject LoadingUI_gameobject;
        public static OV_LoadingUI LoadingUI_Script;
        public static Zombie[] zombies;
        public static SteamPlayer[] players;
        public static InteractableItem[] items;
        public static InteractableVehicle[] vehicles;
        public static InteractableStorage[] storages;
        public static InteractableSentry[] sentrys;
        public static Animal[] animals;

        public static Dictionary<string, string> songs = new Dictionary<string, string>()
        {
            {"Pegboard Nerds - Hero", "PBN_Hero.wav"},
            {"EnV - Pneumatic Tokyo", "EnV_Pneumatic_Tokyo.wav"},
            {"NIVIRO - Nandaka", "NIVIRO_Nandaka.wav"},
        };
    }
}
