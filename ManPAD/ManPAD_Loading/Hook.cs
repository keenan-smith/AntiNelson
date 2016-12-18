using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_Loading
{
    public class Hook : MonoBehaviour
    {
        public static GameObject go_manpad = null;
        public static ManPAD instance_manpad = null;
        public static Thread thread_hook = null;
        public static bool running = true;

        public static void callMeToHook()
        {
            if (go_manpad == null || instance_manpad == null)
            {
                go_manpad = new GameObject();
                instance_manpad = go_manpad.AddComponent<ManPAD>();
                DontDestroyOnLoad(go_manpad);
            }
        }

        public static void callMeToHookThread()
        {
            thread_hook = new Thread(new ThreadStart(hookThread));
            thread_hook.Start();
        }

        private static void hookThread()
        {
            while (running)
            {
                Thread.Sleep(1000);
                if (go_manpad == null || instance_manpad == null)
                {
                    go_manpad = new GameObject();
                    instance_manpad = go_manpad.AddComponent<ManPAD>();
                    DontDestroyOnLoad(go_manpad);
                }
                Thread.Sleep(5000);
            }
        }
    }
}
