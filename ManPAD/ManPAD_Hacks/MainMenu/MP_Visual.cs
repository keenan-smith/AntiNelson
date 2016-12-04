using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(7, "Visuals", 300f)]
    public class MP_Visual : MenuOption
    {
        #region Variables
        private Material _mat;
        private float _prevSealLevel = 0f;
        private float _prevSnowLevel = 0f;
        private Vector2 _horizontalDraw_Start;
        private Vector2 _horizontalDraw_End;
        private Vector2 _verticalDraw_Start;
        private Vector2 _verticalDraw_End;

        public static bool customCrosshair = false;
        public static bool noRain = false;
        public static bool noSnow = false;
        public static bool noWater = false;
        #endregion

        #region Mono Functions
        public void Start()
        {
            _mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            _mat.hideFlags = HideFlags.HideAndDontSave;
            _mat.SetInt("_SrcBlend", 5);
            _mat.SetInt("_DstBlend", 10);
            _mat.SetInt("_Cull", 0);
            _mat.SetInt("_ZWrite", 0);

            _horizontalDraw_Start = new Vector2((Screen.width / 2f) - 15f, Screen.height / 2f);
            _horizontalDraw_End = new Vector2((Screen.width / 2f) + 15f, Screen.height / 2f);
            _verticalDraw_Start = new Vector2(Screen.width / 2f, (Screen.height / 2f) - 15f);
            _verticalDraw_End = new Vector2(Screen.width / 2f, (Screen.height / 2f) + 15f);
        }

        public void Update()
        {
            if (!Variables.isInGame)
                return;

            #region No Rain
            if (noRain)
                LevelLighting.rainyness = ELightingRain.NONE;
            #endregion

            #region No Snow
            if (noSnow)
            {
                if (LevelLighting.snowLevel != 0f)
                    _prevSnowLevel = LevelLighting.snowLevel;

                LevelLighting.snowLevel = 0f;
            }
            else
            {
                if (_prevSnowLevel != 0f && LevelLighting.snowLevel != _prevSnowLevel)
                {
                    LevelLighting.snowLevel = _prevSnowLevel;
                    _prevSnowLevel = 0f;
                }
            }
            #endregion

            #region No Water
            if (noWater)
            {
                if (LevelLighting.seaLevel != 0f)
                    _prevSealLevel = LevelLighting.seaLevel;

                LevelLighting.seaLevel = 0f;
            }
            else
            {
                if (_prevSealLevel != 0f && LevelLighting.seaLevel != _prevSealLevel)
                {
                    LevelLighting.seaLevel = _prevSealLevel;
                    _prevSealLevel = 0f;
                }
            }
            #endregion
        }

        public void OnGUI()
        {
            if (!Variables.isInGame || Variables.isSpying)
                return;

            #region Custom Crosshair
            if (customCrosshair)
            {
                if (!MP_Aimbot.FovCrosshair)
                {
                    Drawing.DrawLine(_horizontalDraw_Start, _horizontalDraw_End, Color.red, 2f);
                    Drawing.DrawLine(_verticalDraw_Start, _verticalDraw_End, Color.red, 2f);
                }
                else
                {
                    GL.PushMatrix();
                    _mat.SetPass(0);
                    GL.Begin(GL.LINES);
                    GL.Color(Color.red);

                    for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.01f)
                    {
                        Vector3 ci = (new Vector3(Mathf.Cos(theta) * MP_Aimbot.FOV + Screen.width / 2, Mathf.Sin(theta) * MP_Aimbot.FOV + Screen.height / 2, 0));
                        GL.Vertex3(ci.x, ci.y, 0);
                    }
                    GL.End();
                }
            }
            #endregion
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            customCrosshair = GUILayout.Toggle(customCrosshair, "Custom Crosshair");
            noRain = GUILayout.Toggle(noRain, "No Rain");
            noSnow = GUILayout.Toggle(noSnow, "No Snow");
            noWater = GUILayout.Toggle(noWater, "No Water");
            GUILayout.Label("Time: " + LightingManager.time);
            LightingManager.time = (uint)Math.Round(GUILayout.HorizontalSlider((float)LightingManager.time, (float)0u, (float)3600u));
        }
        #endregion
    }
}
