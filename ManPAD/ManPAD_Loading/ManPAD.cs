using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ManPAD.ManPAD_Loading
{
    public class ManPAD : MonoBehaviour
    {
        #region Unmanaged Functions
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        #endregion

        public ManPAD()
        {
            AllocConsole();
        }

        #region Mono Functions
        public void Start()
        {
        }

        public void Update()
        {
        }

        public void OnGUI()
        {
        }

        public void OnDestroy()
        {
        }
        #endregion
    }
}
