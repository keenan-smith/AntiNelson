using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API;

namespace ManPAD.ManPAD_Loading
{
    public class Hook : MonoBehaviour
    {
        #region Variables
        public ManPAD _instance_ManPAD = null;
        public bool _crashed = false;

        public static GameObject _gameobject_Hook = null;
        public static Thread _thread_Hook = null;
        public static bool _shuttingdown = false;
        #endregion

        #region Properties
        private static bool hooked
        {
            get
            {
                return _gameobject_Hook != null;
            }
            set
            {
                if (value == false)
                    GameObject.Destroy(_gameobject_Hook);
                else
                    _gameobject_Hook = new GameObject();
            }
        }
        #endregion

        public Hook()
        {
            try
            {
                Debug.Log("Starting up ManPAD...");
                if (_instance_ManPAD != null)
                    return;

                _instance_ManPAD = new ManPAD();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                _crashed = true;
            }
        }

        #region Hooking
        public static void callMeToHook()
        {
            if (hooked && _thread_Hook != null)
                return;

            MPAD_Logging.enableConsole();
            _shuttingdown = false;
            try
            {
                _thread_Hook = new Thread(new ThreadStart(hookLoop));

                _thread_Hook.Start();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static void hookLoop()
        {
            try
            {
                while (!_shuttingdown)
                {
                    Thread.Sleep(1000);
                    if (!hooked)
                    {
                        MPAD_Logging.Log("Hooking...");
                        _gameobject_Hook = new GameObject();
                        _gameobject_Hook.AddComponent<Hook>();
                        DontDestroyOnLoad(_gameobject_Hook);
                    }
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        #endregion

        #region Mono Functions
        void Start()
        {
            try
            {
                if (_instance_ManPAD == null)
                    _instance_ManPAD = new ManPAD();

                _instance_ManPAD._Start();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                _crashed = true;
            }
        }

        void Update()
        {
            try
            {
                if (_instance_ManPAD == null)
                    return;

                _instance_ManPAD._Update();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                _crashed = true;
            }
        }

        void OnGUI()
        {
            try
            {
                errorCheck();

                if (_instance_ManPAD == null)
                    return;

                _instance_ManPAD._OnGUI();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                _crashed = true;
            }
        }

        void OnApplicationQuit()
        {
            try
            {
                ShutdownThread();

                if (_instance_ManPAD == null)
                    return;

                _instance_ManPAD._OnDestroy();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                _crashed = true;
            }
        }
        #endregion

        #region Error Managment
        private void ShutdownThread()
        {
            _shuttingdown = true;
            _thread_Hook.Abort();
        }

        private void RestartHook()
        {
            try
            {
                ShutdownThread();
                _thread_Hook = null;
                callMeToHook();
                _crashed = false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Restart();
            }
        }

        private void RestartFramework()
        {
            try
            {
                if (_instance_ManPAD != null)
                    _instance_ManPAD._OnDestroy();

                _instance_ManPAD = null;
                hooked = false;
                _crashed = false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Restart();
            }
        }

        private void Restart()
        {
            try
            {
                if (_instance_ManPAD != null)
                    _instance_ManPAD._OnDestroy();

                _instance_ManPAD = null;
                hooked = false;
                ShutdownThread();
                _thread_Hook = null;
                callMeToHook();
                _crashed = false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Shutdown();
            }
        }

        private void Shutdown()
        {
            try
            {
                if (_instance_ManPAD != null)
                    _instance_ManPAD._OnDestroy();

                ShutdownThread();
                _instance_ManPAD = null;
                hooked = false;
                _thread_Hook = null;
                _crashed = false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                ShutdownThread();
                Application.Quit();
            }
        }

        private void errorCheck()
        {
            if (!_crashed)
                return;

            showMenu();
        }

        private void showMenu()
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

            if (GUILayout.Button("Restart Hook"))
                RestartHook();
            if (GUILayout.Button("Restart Framework"))
                RestartFramework();
            if (GUILayout.Button("Complete Restart"))
                Restart();
            if (GUILayout.Button("Shutdown"))
                Shutdown();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        #endregion
    }
}
