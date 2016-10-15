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
        /// <summary>
        /// Name of the group.
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Permissions of the group.
        /// </summary>
        public List<string> permissions
        {
            get
            {
                return _permissions;
            }
        }

        /// <summary>
        /// Is it the default group.
        /// </summary>
        public bool isDefault
        {
            get
            {
                return _isDefault;
            }
        }
        #endregion

        /// <summary>
        /// The group instance of pointblank.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="isDefault">Is it the default group.</param>
        /// <param name="permissions">The permissions of the group.</param>
        public PBGroup(string name, bool isDefault, string[] permissions)
        {
            _name = name;
            _isDefault = isDefault;
            _permissions.AddRange(permissions);
        }
    }
}
