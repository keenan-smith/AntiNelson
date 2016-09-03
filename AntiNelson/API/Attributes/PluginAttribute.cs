using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Attributes
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
        public string pluginName
        {
            get
            {
                return _pluginName;
            }
        }

        public string pluginCreator
        {
            get
            {
                return _pluginCreator;
            }
        }

        public bool isHidden
        {
            get
            {
                return _isHidden;
            }
        }

        public bool isPrivate
        {
            get
            {
                return _isPrivate;
            }
        }

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

        public PluginAttribute(string pluginName, string pluginCreator, bool isHidden, bool isPrivate)
        {
            _pluginName = pluginName;
            _pluginCreator = pluginCreator;
            _isHidden = isHidden;
            _isPrivate = isPrivate;
        }
    }
}
