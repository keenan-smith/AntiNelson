using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.API.Server.Extensions
{
    public class PBPlugin : MonoBehaviour
    {
        #region Variables
        private GameObject _pluginObject = null;
        #endregion

        #region Properties
        /// <summary>
        /// The gameobject of the plugin.
        /// </summary>
        public GameObject pluginObject
        {
            get
            {
                return _pluginObject;
            }
            set
            {
                if (_pluginObject == null)
                    _pluginObject = value;
            }
        }
        #endregion

        #region Virtual Functions
        /// <summary>
        /// Called when the plugin is loaded.
        /// </summary>
        public virtual void onLoad() { }
        #endregion
    }
}
