using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_Loading
{
    public class Hook : MonoBehaviour
    {
        private static GameObject go_manpad = null;
        private static ManPAD instance_manpad = null;

        public static void callMeToHook()
        {
            if (go_manpad == null || instance_manpad == null)
            {
                go_manpad = new GameObject();
                instance_manpad = go_manpad.AddComponent<ManPAD>();
                DontDestroyOnLoad(go_manpad);
            }
        }
    }
}
