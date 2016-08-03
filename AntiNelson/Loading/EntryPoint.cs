using System;
using System.Threading;
using UnityEngine;

namespace AntiNelson
{
    public class EntryPoint : MonoBehaviour
    {

        public static EntryPoint instance = null;
        public static Hack hackInstance = null;

        public static bool waitForRelaunch = false;
        public static bool hackCrashed = false;

        public static Thread runThread = null;

        public static void Launch()
        {

            runThread = new Thread(Hack.reHook);
            runThread.Start();

        }

        public static void killAndRelaunch()
        {

            DestroyImmediate(Hack.hookObj);
            runThread.Abort();
            hackInstance = new Hack();
            instance = null;

            Launch();

        }

        public void Update()
        {

            if (!waitForRelaunch && !hackCrashed)
                try
                {
                    hackInstance._Update();
                }
                catch (Exception e)
                {

                    Debug.LogException(e);

                    if (!e.ToString().ToLower().Contains("repaint"))
                        hackCrashed = true;

                }

        }

        public void OnGUI()
        {

            if (!waitForRelaunch && !hackCrashed)
                try
                {
                    hackInstance._OnGUI();
                }
                catch (Exception e)
                {

                    Debug.LogException(e);

                    if (!e.ToString().ToLower().Contains("repaint"))
                        hackCrashed = true;

                }

            if (hackCrashed)
            {

                GUILayout.BeginArea(new Rect(10F, 10F, 200F, Screen.height));
                GUILayout.BeginVertical();

                int size = GUI.skin.label.fontSize;
                float height = GUI.skin.label.fixedHeight;
                GUI.skin.label.fontSize += 25;
                GUI.skin.label.fixedHeight = 50;
                GUI.color = Color.red;

                GUILayout.Label("HACK CRASHED");

                GUI.skin.label.fontSize = size;
                GUI.skin.label.fixedHeight = height;
                GUI.color = Color.white;

                if (GUILayout.Button("Relaunch"))
                    killAndRelaunch();

                GUILayout.EndVertical();
                GUILayout.EndArea();

            }

        }

    }
}
