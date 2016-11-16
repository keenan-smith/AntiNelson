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
        public static Zombie[] zombies;
        public static SteamPlayer[] players;
        public static InteractableItem[] items;
        public static InteractableVehicle[] vehicles;
        public static InteractableStorage[] storages;
        public static InteractableSentry[] sentrys;
        public static Animal[] animals;
    }
}
