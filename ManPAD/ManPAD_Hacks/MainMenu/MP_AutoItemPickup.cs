using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.GUIUtilitys;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.Types;
using SDG.Unturned;
using UnityEngine;
using System.Diagnostics;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
#if DEBUG
    [MenuOption(7, "Auto Item Pickup", 200f)]
    public class MP_AutoItemPickup : MenuOption
    {
        #region Variables
        public static bool AutoItemPickupEnabled = false;
        public static bool IgnoreEmpty = true;
        public static float AIP_Update = 200;
        public static Coroutine AIP_Thread;
        #endregion

        #region Mono Functions
        public void Start()
        {
            AIP_Thread = StartCoroutine(AutoItemPickupCoroutine());
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            AutoItemPickupEnabled = GUILayout.Toggle(AutoItemPickupEnabled, "Auto Item Pickup Enabled");
            IgnoreEmpty = GUILayout.Toggle(IgnoreEmpty, "Ignore Empty");
            GUILayout.Label("Refresh rate: " + AIP_Update + "ms");
            AIP_Update = GUILayout.HorizontalSlider(Mathf.Round(AIP_Update), 50, 2000);
        }
        #endregion

        #region Coroutines
        [DebuggerHidden()]
        private IEnumerator AutoItemPickupCoroutine()
        {
            while (true)
            {
                if (!AutoItemPickupEnabled || !Provider.isConnected || Provider.isLoading || LoadingUI.isBlocked || Player.player == null)
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }

                Collider[] array = Physics.OverlapSphere(Camera.main.transform.position, 19f, RayMasks.ITEM);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != null && array[i].gameObject != null && array[i].GetComponent<InteractableItem>() != null && array[i].GetComponent<InteractableItem>().asset != null)
                    {
                        InteractableItem item = array[i].GetComponent<InteractableItem>();
                        bool on = false;
                        if (MP_ESP.ESP_Items_Types.filter.TryGetValue(item.asset.type, out on))
                        {
                            if (on)
                            {
                                if (IgnoreEmpty && item.item.amount > 0)
                                    item.use();
                                else if (!IgnoreEmpty)
                                    item.use();
                            }  
                        }
                    }

                }

                yield return new WaitForSeconds(AIP_Update / 1000);
            }
        }
        #endregion
    }
#endif
}
