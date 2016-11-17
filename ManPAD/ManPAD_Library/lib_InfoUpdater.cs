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
        public bool running = true;
        public Thread t;
        #endregion

        #region Mono Functions
        public void Start()
        {
            t = new Thread(new ThreadStart(reloadInfo));
            t.Start();
        }

        public void OnDestroy()
        {
            t.Abort();
            running = false;
        }
        #endregion

        #region Threads
        private void reloadInfo()
        {
            while (running)
            {
                try
                {
                    Variables.isInGame = (!LoadingUI.isBlocked && !Provider.isLoading && Provider.isConnected && Provider.clients != null && Provider.clients.Count > 0);
                    if (!Variables.isInGame)
                        continue;
                    Variables.players = Provider.clients.ToArray();
                    List<Zombie> temp = new List<Zombie>();
                    lock (ZombieManager.regions)
                    {
                        for (int i = 0; i < ZombieManager.regions.Length; i++)
                        {
                            temp.AddRange(ZombieManager.regions[i].zombies);
                        }
                    }
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
                Thread.Sleep(1000);
            }
        }
        #endregion
    }
}
