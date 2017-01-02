using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
#if DEBUG
    [MenuOption(12, "Keybinds", 300f)]
    public class MP_Keybinds : MenuOption
    {
        #region Variables
        public static KeyCode key_Aimbot = MP_Config.instance.getKeybind("Aimbot");
        public static KeyCode key_SilentAim = MP_Config.instance.getKeybind("SilentAim");
        public static KeyCode key_MainMenu = MP_Config.instance.getKeybind("MainMenu");
        public static KeyCode key_AimKey = MP_Config.instance.getKeybind("AimKey");
        public static KeyCode key_ESP = MP_Config.instance.getKeybind("ESP");
        public static KeyCode key_InstantDisconnect = MP_Config.instance.getKeybind("InstantDisconnect");
        public static KeyCode key_Overlay = MP_Config.instance.getKeybind("Overlay");

        private bool _selecting = false;
        private string _key = "";
        #endregion

        #region Mono Functions
        public void Update()
        {
            if (_selecting && Event.current.type == EventType.KeyDown)
            {
                if (_key == "MainMenu")
                    key_MainMenu = Event.current.keyCode;
                else if (_key == "Aimbot")
                    key_Aimbot = Event.current.keyCode;
                MP_Config.instance.setKeybind(_key, Event.current.keyCode);
                _key = "";
                _selecting = false;
            }
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            if (GUILayout.Button("Main Menu: " + (_key == "MainMenu" ? "Select Key" : key_MainMenu.ToString())))
            {
                if (_selecting)
                {
                    _selecting = false;
                    _key = "";
                    key_MainMenu = MP_Config.instance.getKeybind("MainMenu");
                }
                else
                {
                    _selecting = true;
                    _key = "MainMenu";
                    key_MainMenu = KeyCode.None;
                }
            }
            if (GUILayout.Button("Aimbot: " + (_key == "Aimbot" ? "Select Key" : key_Aimbot.ToString())))
            {
                if (_selecting)
                {
                    _selecting = false;
                    _key = "";
                    key_Aimbot = MP_Config.instance.getKeybind("Aimbot");
                }
                else
                {
                    _selecting = true;
                    _key = "Aimbot";
                    key_Aimbot = KeyCode.None;
                }
            }
        }
        #endregion
    }
#endif
}
