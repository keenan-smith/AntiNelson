using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace AntiNelson
{
    public class Hack : MonoBehaviour
    {
        public static Camera cam = null;

        public static bool showGUI = true;

        public void _Update()
        {

            if (isKeyDown(KeyCode.F1))
                showGUI = !showGUI;

        }

        public void _OnGUI()
        {
            if (showGUI)
            {
                GUI.color = Color.white;

                GUILayout.BeginArea(new Rect(Screen.width - 170F, 10F, 160F, Screen.height));
                GUILayout.BeginVertical();

                GUILayout.Label("Anti Nelson");

                if (Camera.main != null)
                    if (cam != null)
                    {
                        Vector3 _cam = cam.transform.position;
                        GUILayout.Label("Position: " + (int)_cam.x + ", " + (int)_cam.y + ", " + (int)_cam.z);
                    }
                    else cam = Camera.main;

                GUILayout.EndVertical();
                GUILayout.EndArea();

            }
        }

        public bool isKeyDown(KeyCode key)
        {

            return key != KeyCode.None && Input.GetKeyDown(key);

        }

    }
}
