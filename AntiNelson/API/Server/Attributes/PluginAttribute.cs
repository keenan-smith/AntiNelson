using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginAttribute : Attribute
    {
        #region Variables
        private string _pluginName;
        private string _pluginCreator;
        private bool _isHidden;
        private bool _isPrivate;
        private int _pluginID = -1;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        public string pluginName
        {
            get
            {
                return _pluginName;
            }
        }

        /// <summary>
        /// The creator of the plugin.
        /// </summary>
        public string pluginCreator
        {
            get
            {
                return _pluginCreator;
            }
        }

        /// <summary>
        /// Is the plugin hidden from unturned.
        /// </summary>
        public bool isHidden
        {
            get
            {
                return _isHidden;
            }
        }

        /// <summary>
        /// Is the plugin private.
        /// </summary>
        public bool isPrivate
        {
            get
            {
                return _isPrivate;
            }
        }

        /// <summary>
        /// The plugin ID.
        /// </summary>
        public int pluginID
        {
            get
            {
                return _pluginID;
            }
            set
            {
                if (_pluginID == -1)
                    _pluginID = value;
            }
        }
        #endregion

        /// <summary>
        /// Any class with the plugin attribute will be registered as a plugin. These classes are ran first.
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <param name="pluginCreator">The creator of the plugin.</param>
        /// <param name="isHidden">If the plugin is hidden from unturned.</param>
        /// <param name="isPrivate">If the plugin is private for the server.</param>
        public PluginAttribute(string pluginName, string pluginCreator, bool isHidden, bool isPrivate)
        {
            _pluginName = pluginName;
            _pluginCreator = pluginCreator;
            _isHidden = isHidden;
            _isPrivate = isPrivate;
        }
    }
}
