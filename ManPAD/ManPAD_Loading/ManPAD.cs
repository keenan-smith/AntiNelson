﻿using System;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_Library;
using ManPAD.ManPAD_Overridables;
using SDG.Unturned;

namespace ManPAD.ManPAD_Loading
{
    public class ManPAD : MonoBehaviour
    {
        public ManPAD()
        {
        }

        #region Mono Functions
        public void Start()
        {
            StartCoroutine(loadAsset());
            new MP_Config(Application.persistentDataPath + "\\ManPADConfig.dat");
            MP_GOLoader.library_addLibrary(typeof(lib_MethodReplacer));
            MP_GOLoader.library_addLibrary(typeof(lib_MainMenu));
            MP_GOLoader.library_addLibrary(typeof(lib_InfoUpdater));
#if DEBUG
            MP_GOLoader.library_addLibrary(typeof(lib_GoldExploits));
#endif
            MP_GOLoader.library_addLibrary(typeof(EBones));
#if DEBUG
            MP_GOLoader.library_addLibrary(typeof(lib_Console));
#endif

            Variables.LoadingUI_gameobject = new GameObject();
            Variables.LoadingUI_Script = Variables.LoadingUI_gameobject.AddComponent<OV_LoadingUI>();
            DontDestroyOnLoad(Variables.LoadingUI_gameobject);

            typeof(Provider).GetField("APP_NAME", BindingFlags.Public | BindingFlags.Static).SetValue(null, "ManPAD Alpha");
        }

        public void Update()
        {
        }

        public void OnGUI()
        {
        }

        public void OnDestroy()
        {
            MP_Config.instance.save();
            Hook.thread_hook.Abort();
            Hook.running = false;
        }
        #endregion

        #region Coroutines
        private IEnumerator loadAsset()
        {
            MP_Logging.Log("Loading asset bundle...");
            WWW www = new WWW("file://" + Application.dataPath + "/Manpad.assetbundle");
            yield return www;

            Variables.bundle = www.assetBundle;
            Variables.bundle_chams = AssetBundle.LoadFromFile(Application.dataPath + "\\Manpad_1.assetbundle", 0U);
            MP_Logging.Log("Asset bundle loaded!");
        }
        #endregion
    }
}
