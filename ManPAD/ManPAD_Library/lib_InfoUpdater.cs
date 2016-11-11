using System;
using System.Threading;
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
        #region Variables
        private bool running = true;
        #endregion

        #region Mono Functions
        public void Start()
        {
            StartCoroutine(reloadInfo());
        }

        public void OnDestroy()
        {
            running = false;
        }
        #endregion

        #region Coroutines
        private IEnumerator reloadInfo()
        {
            while (running)
            {
                Variables.isInGame = (!LoadingUI.isBlocked && Provider.isConnected);
                if (!Variables.isInGame)
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                try
                {
                    Variables.players = Provider.clients.ToArray();
                    List<Zombie> temp = new List<Zombie>();
                    for (int i = 0; i < ZombieManager.regions.Length; i++)
                    {
                        temp.AddRange(ZombieManager.regions[i].zombies);
                    }
                    Variables.zombies = temp.ToArray();
                    Variables.items = UnityEngine.Object.FindObjectsOfType(typeof(InteractableItem)) as InteractableItem[];
                    Variables.vehicles = VehicleManager.vehicles.ToArray();
                    Variables.storages = UnityEngine.Object.FindObjectsOfType(typeof(InteractableStorage)) as InteractableStorage[];
                    Variables.sentrys = UnityEngine.Object.FindObjectsOfType(typeof(InteractableSentry)) as InteractableSentry[];
                    Variables.animals = AnimalManager.animals.ToArray();
                }
                catch (Exception ex) {}

                yield return new WaitForSeconds(1f);
            }
        }
        #endregion
    }
}
