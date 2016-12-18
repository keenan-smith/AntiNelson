using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API.GUI.Extensions;

namespace ManPAD.ManPAD_API
{
    public class MP_GOLoader : MonoBehaviour
    {
        #region Variables
        private static Dictionary<Type, MonoBehaviour> _list_hack = new Dictionary<Type, MonoBehaviour>();
        private static Dictionary<Type, MonoBehaviour> _list_library = new Dictionary<Type, MonoBehaviour>();
        private static Dictionary<Type, MonoBehaviour> _list_system = new Dictionary<Type, MonoBehaviour>();

        private static GameObject _go_hack = null;
        private static GameObject _go_library = null;
        private static GameObject _go_system = null;
        #endregion

        #region Functions - Hack
        private static void hack_createObject()
        {
            _go_hack = new GameObject();
            DontDestroyOnLoad(_go_hack);
        }

        public static MenuOption hack_getMenuOptionByType(Type t)
        {
            if (!_list_hack.ContainsKey(t))
                return null;

            return (MenuOption)_list_hack[t];
        }

        public static OverlayOption hack_getOverlayOptionByType(Type t)
        {
            if (!_list_hack.ContainsKey(t))
                return null;

            return (OverlayOption)_list_hack[t];
        }

        public static MenuOption hack_addMenuOption(Type t)
        {
            if (_list_hack.ContainsKey(t))
                return (MenuOption)_list_hack[t];
            if (_go_hack == null)
                hack_createObject();

            MenuOption inst = _go_hack.AddComponent(t) as MenuOption;

            _list_hack.Add(t, inst);
            return inst;
        }

        public static OverlayOption hack_addOverlayOption(Type t)
        {
            if (_list_hack.ContainsKey(t))
                return (OverlayOption)_list_hack[t];
            if (_go_hack == null)
                hack_createObject();

            OverlayOption inst = _go_hack.AddComponent(t) as OverlayOption;

            _list_hack.Add(t, inst);
            return inst;
        }

        public static void hack_removeMenuOptionByInstance(MenuOption inst)
        {
            if (!_list_hack.ContainsValue(inst))
                return;

            Type t = _list_hack.Where(a => a.Value == inst).Select(a => a.Key).First() as Type;

            if (t == null)
                return;

            GameObject.Destroy(_list_hack[t]);
            _list_hack.Remove(t);
        }

        public static void hack_removeOverlayOptionByInstance(OverlayOption inst)
        {
            if (!_list_hack.ContainsValue(inst))
                return;

            Type t = _list_hack.Where(a => a.Value == inst).Select(a => a.Key).First() as Type;

            if (t == null)
                return;

            GameObject.Destroy(_list_hack[t]);
            _list_hack.Remove(t);
        }

        public static void hack_removeHackByType(Type t)
        {
            if (!_list_hack.ContainsKey(t))
                return;

            GameObject.Destroy(_list_hack[t]);
            _list_hack.Remove(t);
        }
        #endregion

        #region Functions - Library
        private static void library_createObject()
        {
            _go_library = new GameObject();
            DontDestroyOnLoad(_go_library);
        }

        public static MonoBehaviour library_getByType(Type t)
        {
            if (!_list_library.ContainsKey(t))
                return null;

            return _list_library[t];
        }

        public static MonoBehaviour library_addLibrary(Type t)
        {
            if (_list_library.ContainsKey(t))
                return _list_library[t];
            if (_go_library == null)
                library_createObject();

            MonoBehaviour inst = _go_library.AddComponent(t) as MonoBehaviour;

            _list_library.Add(t, inst);
            return inst;
        }

        public static void library_removeLibraryByInstance(MonoBehaviour inst)
        {
            if (!_list_library.ContainsValue(inst))
                return;

            Type t = _list_library.Where(a => a.Value == inst).Select(a => a.Key).First() as Type;

            if (t == null)
                return;

            GameObject.Destroy(_list_library[t]);
            _list_library.Remove(t);
        }

        public static void library_removeLibraryByType(Type t)
        {
            if (!_list_library.ContainsKey(t))
                return;

            GameObject.Destroy(_list_library[t]);
            _list_library.Remove(t);
        }
        #endregion

        #region Functions - System
        private static void system_createObject()
        {
            _go_system = new GameObject();
            DontDestroyOnLoad(_go_system);
        }

        public static MonoBehaviour system_getByType(Type t)
        {
            if (_list_system.ContainsKey(t))
                return _list_system[t];

            return _list_system[t];
        }

        public static MonoBehaviour system_addSystem(Type t)
        {
            if (_list_system.ContainsKey(t))
                return _list_system[t];
            if (_go_system == null)
                system_createObject();

            MonoBehaviour inst = _go_system.AddComponent(t) as MonoBehaviour;

            _list_system.Add(t, inst);
            return inst;
        }

        public static void system_removeSystemByInstance(MonoBehaviour inst)
        {
            if (!_list_system.ContainsValue(inst))
                return;

            Type t = _list_system.Where(a => a.Value == inst).Select(a => a.Key).First() as Type;

            if (t == null)
                return;

            GameObject.Destroy(_list_system[t]);
            _list_system.Remove(t);
        }

        public static void system_removeSystemByType(Type t)
        {
            if (!_list_system.ContainsKey(t))
                return;

            GameObject.Destroy(_list_system[t]);
            _list_system.Remove(t);
        }
        #endregion
    }
}
