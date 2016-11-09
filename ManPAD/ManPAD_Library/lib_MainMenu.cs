using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using UnityEngine;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI;
using ManPAD.ManPAD_API.GUI.Attributes;

namespace ManPAD.ManPAD_Library
{
    public class lib_MainMenu : MonoBehaviour
    {
        #region Variables
        private Rect _rect_ListMenu;
        private GUISkin _skin;
        private bool _isOpen = false;
        private Texture _logo;
        #endregion

        #region Mono Functions
        public void Start()
        {
            MP_Logging.Log("Loading Main Menu...");
            _rect_ListMenu = new Rect(MP_MainMenu.menu_Start_X, MP_MainMenu.menu_Start_Y, Screen.width, MP_MainMenu.button_Height + 1f);

            findMenuOptions();
            MP_Logging.Log("Main Menu Loaded!");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.Escape)) // TEMPRORAY! REMOVE LATER!!!!!!
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (_isOpen && PlayerPauseUI.active)
                    {
                        PlayerPauseUI.active = false;
                        _isOpen = false;
                    }
                }
                else
                {
                    if (_isOpen)
                    {
                        PlayerPauseUI.active = false;
                        _isOpen = false;
                    }
                    else if(!PlayerPauseUI.active)
                    {
                        PlayerPauseUI.active = true;
                        _isOpen = true;
                    }
                }
            }

            if (!_isOpen || LoadingUI.isBlocked || !Provider.isConnected)
                return;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // Forward
            {
                if (_rect_ListMenu.Contains(MP_MainMenu.mouse_position))
                {
                    if (MP_MainMenu.attributes[MP_MainMenu.attributes.Length - 1].button.x + MP_MainMenu.attributes[MP_MainMenu.attributes.Length - 1].button.width > Screen.width - 1f)
                    {
                        float jump = 0f;
                        for (int i = 0; i < MP_MainMenu.attributes.Length; i++)
                        {
                            if (MP_MainMenu.attributes[i].button.x + MP_MainMenu.attributes[i].button.width > Screen.width - 1f)
                            {
                                jump = ((MP_MainMenu.attributes[i].button.x + MP_MainMenu.attributes[i].button.width) - Screen.width) + 1f;
                                break;
                            }
                        }

                        for (int i = 0; i < MP_MainMenu.attributes.Length; i++)
                            MP_MainMenu.attributes[i].UI_X -= jump;
                    }
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // Backward
            {
                if (_rect_ListMenu.Contains(MP_MainMenu.mouse_position))
                {
                    if (MP_MainMenu.attributes[0].button.x < 1f)
                    {
                        float jump = 0f;
                        for (int i = MP_MainMenu.attributes.Length - 1; i > -1; i--)
                        {
                            if (MP_MainMenu.attributes[i].button.x < 1f)
                            {
                                jump = -MP_MainMenu.attributes[i].button.x + 1f;
                                break;
                            }
                        }

                        for (int i = 0; i < MP_MainMenu.attributes.Length; i++)
                            MP_MainMenu.attributes[i].UI_X += jump;
                    }
                }
            }
        }

        public void OnGUI()
        {
            if (!_isOpen || LoadingUI.isBlocked || !Provider.isConnected)
                return;

            if (GUI.skin != _skin) // TEMPRORAY! REMOVE LATER!!!!!!
            {
                GUISkin skin = Variables.bundle.LoadAsset("Skins/White.guiskin") as GUISkin;
                _skin = skin;
                GUI.skin = skin;
                //_logo = Variables.bundle.LoadAsset("") as Texture;
            }

            //GUI.DrawTexture(new Rect((float)Math.Round((double)Screen.width / 2 - 50), (float)Math.Round((double)Screen.height / 2 - 50), 100f, 100f))
            GUI.Box(_rect_ListMenu, "");
            for (int i = 0; i < MP_MainMenu.attributes.Length; i++)
            {
                GUI.Button(MP_MainMenu.attributes[i].button, MP_MainMenu.attributes[i].text);

                if (MP_MainMenu.attributes[i].isActive)
                {
                    MP_MainMenu.options[i].open = true;
                    GUI.Box(MP_MainMenu.attributes[i].window, "");
                    GUILayout.BeginArea(MP_MainMenu.attributes[i].window);
                    MP_MainMenu.options[i].scrollPos = GUILayout.BeginScrollView(MP_MainMenu.options[i].scrollPos);
                    MP_MainMenu.options[i].runGUI();
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                }
                else
                {
                    MP_MainMenu.options[i].open = false;
                }
            }
        }
        #endregion

        #region Functions
        public void addMenuOption(Type t)
        {
            MenuOptionAttribute attrib = (MenuOptionAttribute)Attribute.GetCustomAttribute(t, typeof(MenuOptionAttribute));

            if (attrib == null)
                return;

            MP_MainMenu.addMenuOption(attrib, t);
        }

        public void findMenuOptions()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                if (t.IsClass)
                    addMenuOption(t);
        }
        #endregion
    }
}
