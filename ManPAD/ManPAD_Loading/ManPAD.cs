using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_Library;

namespace ManPAD.ManPAD_Loading
{
    public class ManPAD : MonoBehaviour
    {
        public ManPAD()
        {
            MP_Logging.enableConsole();
        }

        #region Mono Functions
        public void Start()
        {
            MP_GOLoader.library_addLibrary(typeof(lib_MethodReplacer));
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
