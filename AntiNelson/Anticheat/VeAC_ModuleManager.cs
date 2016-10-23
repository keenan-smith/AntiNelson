using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
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

        private DateTime _detectionUpdate;
        #endregion

        #region Mono Functions
        public void Start()
        {
            _object_Modules = new GameObject();
            DontDestroyOnLoad(_object_Modules);

            _detectionUpdate = DateTime.Now;
            loadModules();
        }

        public void OnDestory()
        {
            unloadModules();
            GameObject.Destroy(_object_Modules);
        }

        public void Update()
        {
            if ((DateTime.Now - _detectionUpdate).TotalMilliseconds >= 5000)
            {
                foreach (KeyValuePair<PBPlayer, Dictionary<string, int>> user in _detections)
                {
                    foreach (KeyValuePair<string, int> detection in user.Value)
                    {
                        if (detection.Value >= VeAC_Settings.max_detection)
                        {
                            if (VeAC_Settings.ban_user)
                                user.Key.ban(detection.Key, 0, true, CSteamID.Nil);
                            if (VeAC_Settings.kick_user)
                                user.Key.kick(detection.Key);
                            if (VeAC_Settings.warn_admins)
                                foreach (PBPlayer player in PBServer.players)
                                    if (player.steamPlayer.isAdmin || player.hasPermission("pointblank.showHackers"))
                                        player.sendChatMessage("Player: " + user.Key.playerID.playerName + " has been detected for cheating.", Color.magenta);
                            if (VeAC_Settings.warn_user)
                                user.Key.sendChatMessage("You have been detected as cheating!", Color.magenta);
                        }
                    }
                }
                _detectionUpdate = DateTime.Now;
            }
        }
        #endregion

        #region Functions
        public void addDetection(PBPlayer player, string detection)
        {
            if (_detections.ContainsKey(player))
            {
                if (_detections[player].ContainsKey(detection))
                    _detections[player][detection]++;
                else
                    _detections[player].Add(detection, 1);
            }
            else
            {
                Dictionary<string, int> dict = new Dictionary<string,int>();
                dict.Add(detection, 1);
                _detections.Add(player, dict);
            }
        }

        private void loadModules()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (t != null && t.IsClass && typeof(VeAC_Module).IsAssignableFrom(t) && t.Name != "VeAC_Module")
                    {
                        VeAC_Module module = _object_Modules.AddComponent(t) as VeAC_Module;
                        if (module == null || !module.check())
                        {
                            GameObject.Destroy(module);
                            continue;
                        }
                        PBLogging.log("Loaded: " + t.Name, false);
                        _modules.Add(module);
                    }
                }
            }
        }

        private void unloadModules()
        {
            foreach (VeAC_Module module in _modules)
            {
                GameObject.Destroy(module);
            }
            _modules.Clear();
        }
        #endregion
    }
}
