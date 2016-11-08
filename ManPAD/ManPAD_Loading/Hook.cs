using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ManPAD.ManPAD_Loading
{
    public class Hook : MonoBehaviour
    {
        private static GameObject go_manpad = null;
        private static ManPAD instance_manpad = null;

        public static Thread thr;

        public static void callMeToHook()
        {
            if (go_manpad == null || instance_manpad == null)
            {
                go_manpad = new GameObject();
                instance_manpad = go_manpad.AddComponent<ManPAD>();
                DontDestroyOnLoad(go_manpad);
            }
        }

        private static void _hook()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (go_manpad == null || instance_manpad == null)
                    {
                        go_manpad = new GameObject();
                        instance_manpad = go_manpad.AddComponent<ManPAD>();
                        DontDestroyOnLoad(instance_manpad);
                    }
                    Thread.Sleep(5000);
                }
            }
            catch (Exception x)
            {
                Debug.Log(x);
            }
        }

        public static void startThread()
        {
            thr = new Thread(new ThreadStart(_hook));
            thr.Start();
        }
    }
}
