using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets;
using ManPAD.ManPAD_API.GUI;
using ManPAD.ManPAD_API.GUI.Attributes;

namespace ManPAD.ManPAD_Library
{
    public class lib_MainMenu : MonoBehaviour
    {
        #region Variables
        private Rect _rect_ListMenu;
        #endregion

        #region Mono Functions
        public void Start()
        {
            _rect_ListMenu = new Rect(MP_MainMenu.menu_Start_X, MP_MainMenu.menu_Start_Y, Screen.width, MP_MainMenu.button_Height + 1f);

            findMenuOptions();
        }

        public void Update()
        {
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
                        {
                            MP_MainMenu.attributes[i].UI_X -= jump;
                        }
                    }
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // Backward
            {

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
