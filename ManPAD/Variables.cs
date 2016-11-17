using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD
{
    public class Variables
    {
        public static AssetBundle bundle;

        public static bool isInGame = false;
        public static bool isSpying = false;
        public static bool silentAim = false; // i dont know where 2 store global variables pls help
        public static bool fovBased = false;
        public static float aimFov = 20;
        public static Zombie[] zombies;
        public static SteamPlayer[] players;
        public static InteractableItem[] items;
        public static InteractableVehicle[] vehicles;
        public static InteractableStorage[] storages;
        public static InteractableSentry[] sentrys;
        public static Animal[] animals;
    }
}
