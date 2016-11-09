using System;
using System.Collections;
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
            StartCoroutine(loadAsset());
            MP_GOLoader.library_addLibrary(typeof(lib_MethodReplacer));
            MP_GOLoader.library_addLibrary(typeof(lib_MainMenu));
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

        #region Coroutines
        private IEnumerator loadAsset()
        {
            MP_Logging.Log("Loading asset bundle...");
            WWW www = new WWW("file://" + Application.dataPath + "/Manpad.assetbundle");
            yield return www;

            Variables.bundle = www.assetBundle;
            MP_Logging.Log("Asset bundle loaded!");
        }
        #endregion
    }
}
