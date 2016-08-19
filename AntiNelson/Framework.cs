using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank
{
    public class Framework : MonoBehaviour
    {
        public Framework()
        {
            if (Dedicator.isDedicated || Provider.isServer)
            {
                try
                {
                    if (!Directory.Exists(Variables.modsPath))
                        Directory.CreateDirectory(Variables.modsPath);
                    if (!Directory.Exists(Variables.pluginsPath))
                        Directory.CreateDirectory(Variables.pluginsPath);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            else
            {
                try
                {
                    if (!Directory.Exists(Variables.modsPath))
                        Directory.CreateDirectory(Variables.modsPath);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        public void _Update()
        {
        }

        public void _OnGUI()
        {
        }
    }
}
