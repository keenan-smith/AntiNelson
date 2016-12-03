using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;

namespace ManPAD.ManPAD_API.GUI
{
    public class MP_MainMenu
    {
        #region Variables
        private static Dictionary<MenuOptionAttribute, MenuOption> _menuOptions = new Dictionary<MenuOptionAttribute, MenuOption>();
        #endregion

        #region Properties
        public static Rect mainMenu
        {
            get
            {
                return new Rect(1f, 1f, (float)Math.Floor((double)((Screen.width / 2) / 4)), Screen.height - 2f);
            }
        }

        public static float option_Max_Width
        {
            get
            {
                return Screen.height - 100f;
            }
        }

        public static Vector2 mouse_position
        {
            get
            {
                return new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            }
        }

        public static MenuOptionAttribute[] attributes
        {
            get
            {
                return _menuOptions.Keys.ToArray().OrderBy(a => a.ID).ToArray();
            }
        }

        public static MenuOption[] options
        {
            get
            {
                return _menuOptions.Values.ToArray().OrderBy(a => a.attrib.ID).ToArray();
            }
        }
        #endregion

        #region Functions
        public static MenuOption addMenuOption(MenuOptionAttribute attrib, Type t)
        {
            if (_menuOptions.ContainsKey(attrib))
                return _menuOptions[attrib];

            MenuOption option = MP_GOLoader.hack_addMenuOption(t);
            option.attrib = attrib;
            attrib.option = option;

            _menuOptions.Add(attrib, option);
            return option;
        }
        #endregion
    }
}
