using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API.Enumerables;
using SDG.Unturned;

namespace ManPAD.ManPAD_API.Types
{
    public class GOUpdate
    {
        #region Variable
        public object instance;
        public GameObject gameObject;
        public EGOUpdate type;

        public string text;
        public float distance;

        public Rect box;
        public Color color;
        public Vector3 screenPosition;
        #endregion

        #region Constructors
        public GOUpdate(SteamPlayer player)
        {
            this.instance = player;
            this.gameObject = player.player.gameObject;
            this.type = EGOUpdate.PLAYER;
        }

        public GOUpdate(Zombie zombie)
        {
            this.instance = zombie;
            this.gameObject = zombie.gameObject;
            this.type = EGOUpdate.ZOMBIE;
        }

        public GOUpdate(InteractableItem item)
        {
            this.instance = item;
            this.gameObject = item.gameObject;
            this.type = EGOUpdate.ITEM;
        }

        public GOUpdate(InteractableVehicle vehicle)
        {
            this.instance = vehicle;
            this.gameObject = vehicle.gameObject;
            this.type = EGOUpdate.VEHICLE;
        }

        public GOUpdate(Animal animal)
        {
            this.instance = animal;
            this.gameObject = animal.gameObject;
            this.type = EGOUpdate.ANIMAL;
        }

        public GOUpdate(InteractableStorage storage)
        {
            this.instance = storage;
            this.gameObject = storage.gameObject;
            this.type = EGOUpdate.STORAGE;
        }

        public GOUpdate(InteractableSentry sentry)
        {
            this.instance = sentry;
            this.gameObject = sentry.gameObject;
            this.type = EGOUpdate.SENTRY;
        }
        #endregion
    }
}
