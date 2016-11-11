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
        public Dictionary<EItemType, bool> filter = new Dictionary<EItemType, bool>();

        private bool shown = false;
        #endregion

        public MP_ItemPicker(EItemType[] filter)
        {
            foreach (EItemType itemType in Enum.GetValues(typeof(EItemType)))
                this.filter.Add(itemType, (filter.Contains(itemType)));
        }

        #region Functions
        public void draw(string text, string save = "")
        {
            shown = GUILayout.Toggle(shown, text);
            if (shown)
            {
                if (!string.IsNullOrEmpty(save))
                    if (GUILayout.Button("Save Items"))
                        MP_Config.instance.setItemType(save, filter.Where(a => a.Value).Select(a => a.Key).ToArray());

                Color col = UnityEngine.GUI.color;
                List<EItemType> toChg = new List<EItemType>();

                foreach (KeyValuePair<EItemType, bool> pair in filter)
                {
                    if (pair.Value)
                        UnityEngine.GUI.color = Color.green;
                    else
                        UnityEngine.GUI.color = Color.red;
                    if (GUILayout.Button(pair.Key.ToString()))
                        toChg.Add(pair.Key);
                }
                foreach (EItemType t in toChg)
                    filter[t] = !filter[t];
                UnityEngine.GUI.color = col;
            }
        }
        #endregion
    }
}
