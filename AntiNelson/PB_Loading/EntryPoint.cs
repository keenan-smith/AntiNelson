using System;
using System.Threading;
using UnityEngine;

namespace PointBlank.PB_Loading
{
    public class EntryPoint : MonoBehaviour
    {
        public static EntryPoint instance = null;
        public static Framework hackInstance = null;

        public static bool waitForRelaunch = false;
        public static bool hackCrashed = false;

        public static Thread runThread = null;

        public static GameObject mainObject = null;

        public static void Launch()
        {

            if (AppDomain.CurrentDomain.Id != 0)
                return;

            try
            {

                Thread runThread = new Thread(new ThreadStart(doHook));
                runThread.Start();

            }
            catch (Exception ex)
            {

                Debug.LogException(ex);

            }

            Variables.isLoaded = true;

        }

        private static void doHook()
        {
            try
            {
                while (true)
                {
                    if (asmLoaded())
                    {
                        Thread.Sleep(1000);
                        if (mainObject == null || instance == null)
                        {
                            mainObject = new GameObject();
                            instance = mainObject.AddComponent<EntryPoint>();
                            DontDestroyOnLoad(mainObject);
                        }

                        if (!waitForRelaunch)
                            Thread.Sleep(5000);

                        waitForRelaunch = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static void killAndRelaunch()
        {
            DestroyImmediate(mainObject);
            runThread.Abort();
            instance = null;
            hackInstance = new Framework();

            Launch();
        }

        public void Start()
        {

            hackInstance = new Framework();
            hackInstance._Start();
            DontDestroyOnLoad(transform.gameObject);

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

                GUILayout.Label("CRASHED");

                GUI.skin.label.fontSize = size;
                GUI.skin.label.fixedHeight = height;
                GUI.color = Color.white;

                if (GUILayout.Button("Relaunch"))
                    killAndRelaunch();

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        private static bool asmLoaded()
        {
            return Array.Exists(AppDomain.CurrentDomain.GetAssemblies(), a => a.FullName.ToLower().Contains("csharp") && !a.FullName.ToLower().Contains("firstpass"));
        }
    }
}
