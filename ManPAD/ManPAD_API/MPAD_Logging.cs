using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_API
{
    public class MPAD_Logging
    {
        #region Unmanaged Functions
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        #endregion

        #region Variables
        private static bool _enabledConsole = false;
        #endregion

        #region Functions
        public static void enableConsole()
        {
            AllocConsole();
            _enabledConsole = true;
            Console.Title = "ManPAD Debug Console";
            Console.WriteLine("Welcome to ManPAD Debug Console. Do NOT close this window!");
        }

        public static void Log(object text)
        {
            if(_enabledConsole)
                Console.WriteLine(text);
            Debug.Log(text);
        }
        #endregion
    }
}
