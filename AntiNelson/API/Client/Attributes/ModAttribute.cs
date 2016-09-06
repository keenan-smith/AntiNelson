using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModAttribute : Attribute
    {
        #region Variables
        private string _modName;
        private string _modCreator;
        private bool _isHidden;
        private bool _isPrivate;
        private int _modID = -1;
        #endregion

        #region Properties
        public string modName
        {
            get
            {
                return _modName;
            }
        }

        public string modCreator
        {
            get
            {
                return _modCreator;
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

        public int modID
        {
            get
            {
                return _modID;
            }
            set
            {
                if (_modID == -1)
                    _modID = value;
            }
        }
        #endregion

        public ModAttribute(string modName, string modCreator, bool isHidden, bool isPrivate)
        {
            _modName = modName;
            _modCreator = modCreator;
            _isHidden = isHidden;
            _isPrivate = isPrivate;
        }
    }
}
