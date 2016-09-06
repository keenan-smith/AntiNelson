using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Server.Extensions
{
    public class PBGroup
    {
        #region Variables
        private string _name;
        private List<string> _permissions = new List<string>();
        private bool _isDefault;
        #endregion

        #region Properties
        public string name
        {
            get
            {
                return _name;
            }
        }

        public List<string> permissions
        {
            get
            {
                return _permissions;
            }
        }

        public bool isDefault
        {
            get
            {
                return _isDefault;
            }
        }
        #endregion

        public PBGroup(string name, bool isDefault, string[] permissions)
        {
            _name = name;
            _isDefault = isDefault;
            _permissions.AddRange(permissions);
        }
    }
}
