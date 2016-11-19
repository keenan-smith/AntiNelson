using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Library
{
    public class lib_InfoUpdater : MonoBehaviour
    {
        #region Mono Functions
        public void Start()
        {
            InvokeRepeating("reloadInfoInvoke", 5f, 5f);
        }
        #endregion

        #region Functions
        private void reloadInfoInvoke()
        {
            try
            {
                Variables.isInGame = (!LoadingUI.isBlocked && !Provider.isLoading && Provider.isConnected && Provider.clients != null && Provider.clients.Count > 0);
                if (!Variables.isInGame)
                    return;
                Variables.players = Provider.clients.ToArray();
                List<Zombie> temp = new List<Zombie>();
                lock (ZombieManager.regions)
                    for (int i = 0; i < ZombieManager.regions.Length; i++)
                        temp.AddRange(ZombieManager.regions[i].zombies);
                Variables.zombies = temp.ToArray();
                Variables.items = UnityEngine.Object.FindObjectsOfType(typeof(InteractableItem)) as InteractableItem[];
                Variables.vehicles = VehicleManager.vehicles.ToArray();
                Variables.storages = UnityEngine.Object.FindObjectsOfType(typeof(InteractableStorage)) as InteractableStorage[];
                Variables.sentrys = UnityEngine.Object.FindObjectsOfType(typeof(InteractableSentry)) as InteractableSentry[];
                Variables.animals = AnimalManager.animals.ToArray();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        #endregion
    }
}
