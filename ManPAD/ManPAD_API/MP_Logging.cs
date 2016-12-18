using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_API
{
    public class MP_Logging
    {
        #region Unmanaged Functions
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        #endregion

        #region Variables
        private static bool _console_Enabled = false;
        #endregion

        #region Functions
        public static void enableConsole()
        {
            AllocConsole();
            _console_Enabled = true;
            Console.Title = "ManPAD Debug Console";
            Console.WriteLine("This is the debug console. Do NOT close this!");
        }

        public static void Log(object data)
        {
            if (_console_Enabled)
                Console.WriteLine(data);
            Debug.Log(data);
        }
        #endregion
    }
}
