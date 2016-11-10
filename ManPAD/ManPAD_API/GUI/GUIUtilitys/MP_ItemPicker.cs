using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_API.GUI.GUIUtilitys
{
    public class MP_ItemPicker
    {
        #region Variables
        public Dictionary<EUseableType, bool> filter = new Dictionary<EUseableType, bool>();

        private bool shown = false;
        #endregion

        public MP_ItemPicker(EUseableType[] filter)
        {
            foreach (EUseableType itemType in Enum.GetValues(typeof(EUseableType)))
                this.filter.Add(itemType, (filter.Contains(itemType)));
        }

        #region Functions
        public void draw(string text, string save = "")
        {
            GUILayout.Label(text);
            shown = GUILayout.Toggle(shown, (shown ? "Close" : "Open") + " Item Menu");
            if (shown)
            {
                if (!string.IsNullOrEmpty(save))
                    if (GUILayout.Button("Save Items"))
                        MP_Config.instance.setItemType(save, filter.Where(a => a.Value).Select(a => a.Key).ToArray());

                Color col = UnityEngine.GUI.color;

                foreach (KeyValuePair<EUseableType, bool> pair in filter)
                {
                    if (pair.Value)
                        UnityEngine.GUI.color = Color.green;
                    else
                        UnityEngine.GUI.color = Color.red;
                    if (GUILayout.Button(pair.Key.ToString()))
                        filter[pair.Key] = !filter[pair.Key];
                }
                UnityEngine.GUI.color = col;
            }
        }
        #endregion
    }
}
