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
using ManPAD.ManPAD_API.GUI.Enumerables;

namespace ManPAD.ManPAD_Library
{
    public class lib_MainMenu : MonoBehaviour
    {
        #region Variables
        private Rect _rect_ListMenu;
        private GUISkin _skin;
        private bool _isOpen = false;
        private Texture _logo;
        private Texture _cursor;
        private Rect _cursorRect = new Rect(0f, 0f, 20f, 20f);
        private EThemes _theme;
        private Vector2 _scroll;
        private GUIStyle _gsLabelTitle;
        #endregion

        #region Mono Functions
        public void Start()
        {
            MP_Logging.Log("Loading Main Menu...");
            _rect_ListMenu = MP_MainMenu.mainMenu;

            findMenuOptions();
            MP_Logging.Log("Main Menu Loaded!");
        }

        public void Update()
        {
            if (_cursor == null || _logo == null || _theme != MP_Config.instance.getTheme())
            {
                _cursor = Resources.Load("UI/Cursor") as Texture;
                if (Variables.bundle != null)
                {
                    _logo = Variables.bundle.LoadAsset("ManpadLogo.png") as Texture;
                    _theme = MP_Config.instance.getTheme();
                    if (_theme == EThemes.WHITE)
                        _skin = Variables.bundle.LoadAsset("White.guiskin") as GUISkin;
                    else if (_theme == EThemes.INVERTED)
                        _skin = Variables.bundle.LoadAsset("Inverted.guiskin") as GUISkin;
                    else if (_theme == EThemes.AQUA)
                        _skin = Variables.bundle.LoadAsset("Aqua.guiskin") as GUISkin;
                    else if (_theme == EThemes.MAGIC)
                        _skin = Variables.bundle.LoadAsset("Magic.guiskin") as GUISkin;
                }
            }
            if (Input.GetKeyDown(MP_Config.instance.getKeybind("MainMenu")) || Input.GetKeyDown(KeyCode.Escape))
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
        }

        public void OnGUI()
        {
            if (!_isOpen || LoadingUI.isBlocked || !Provider.isConnected || Variables.isSpying)
                return;

            GUI.skin = _skin;
            Color sv = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, 0.8f);
            GUI.DrawTexture(new Rect((float)Math.Round((double)Screen.width / 2 - 150), (float)Math.Round((double)Screen.height / 2 - 150), 300f, 300f), _logo, ScaleMode.ScaleToFit);
            GUI.color = sv;
            GUI.Box(_rect_ListMenu, "");
            GUILayout.BeginArea(_rect_ListMenu);
            if (_gsLabelTitle == null)
            {
                _gsLabelTitle = new GUIStyle(GUI.skin.label);
                _gsLabelTitle.alignment = TextAnchor.MiddleCenter;
                _gsLabelTitle.fontSize = 30;

                GUI.skin.toggle.stretchHeight = false;
            }
            GUILayout.Label("ManPAD", _gsLabelTitle);
            _scroll = GUILayout.BeginScrollView(_scroll);
            for (int i = 0; i < MP_MainMenu.attributes.Length; i++)
            {
                if (MP_MainMenu.attributes[i].isActive)
                    GUI.skin.button.normal = GUI.skin.toggle.onNormal;
                else
                    GUI.skin.button.normal = GUI.skin.toggle.normal;
                if (GUILayout.Button(MP_MainMenu.attributes[i].text))
                {
                    MP_MainMenu.options[i].open = !MP_MainMenu.options[i].open;
                    for (int a = 0; a < MP_MainMenu.attributes.Length; a++)
                    {
                        if (a != i)
                            MP_MainMenu.options[a].open = false;
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            for (int i = 0; i < MP_MainMenu.attributes.Length; i++)
            {
                if (MP_MainMenu.attributes[i].isActive)
                {
                    GUI.Box(MP_MainMenu.attributes[i].window, "");
                    GUILayout.BeginArea(MP_MainMenu.attributes[i].window);
                    MP_MainMenu.options[i].scrollPos = GUILayout.BeginScrollView(MP_MainMenu.options[i].scrollPos);
                    MP_MainMenu.options[i].runGUI();
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                }
            }
            if (_cursor != null)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.8f);
                _cursorRect.x = Input.mousePosition.x;
                _cursorRect.y = Screen.height - Input.mousePosition.y;
                GUI.DrawTexture(_cursorRect, _cursor);
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
            for(int i = 0; i < Assembly.GetExecutingAssembly().GetTypes().Length; i++)
            {
                Type t = Assembly.GetExecutingAssembly().GetTypes()[i];
                if (t.IsClass)
                    addMenuOption(t);
            }
        }
        #endregion
    }
}
