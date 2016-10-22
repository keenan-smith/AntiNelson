using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.Anticheat.Extensions;
using PointBlank.API;
using PointBlank.API.Server;

namespace PointBlank.Anticheat
{
    internal class VeAC_ModuleManager : MonoBehaviour
    {
        #region Variables
        private GameObject _object_Modules;
        private List<VeAC_Module> _modules = new List<VeAC_Module>();
        private Dictionary<PBPlayer, Dictionary<string, int>> _detections = new Dictionary<PBPlayer, Dictionary<string, int>>();
        #endregion

        #region Mono Functions
        public void Start()
        {
            _object_Modules = new GameObject();
            DontDestroyOnLoad(_object_Modules);

            loadModules();
        }

        public void Update()
        {

        }
        #endregion

        #region Functions
        private void loadModules()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.IsClass && typeof(VeAC_Module).IsAssignableFrom(t))
                {
                    VeAC_Module module = _object_Modules.AddComponent(t) as VeAC_Module;
                    if (!module.check())
                    {
                        GameObject.Destroy(module);
                        continue;
                    }
                    PBLogging.log("Loaded: " + t.Name);
                    _modules.Add(module);
                }
            }
        }
        #endregion
    }
}
