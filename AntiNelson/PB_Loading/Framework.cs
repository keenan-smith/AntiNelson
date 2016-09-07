using PointBlank.PB_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.PB_Loading
{
    public class Framework : MonoBehaviour
    {

        public void _Start()
        {

            lib_PluginManager pm = new lib_PluginManager();
            pm.loadPlugins();

        }

        public void _Update()
        {

        }

        public void _OnGUI()
        {

        }

    }
}
