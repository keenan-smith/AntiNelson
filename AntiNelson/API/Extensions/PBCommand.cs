using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.API.Extensions
{
    public abstract class PBCommand
    {
        #region Variables
        private string _command;
        private float _cooldown;
        private string _permission;
        private string _argumentPermission;
        private int _maxUsage;
        protected Local localization;
        #endregion

        #region Propertys
        public string command
        {
            get
            {
                return _command;
            }
        }

        public float cooldown
        {
            get
            {
                return _cooldown;
            }
        }

        public string permission
        {
            get
            {
                return _permission;
            }
        }

        public bool hasArgumentPermission
        {
            get
            {
                return !string.IsNullOrEmpty(_argumentPermission);
            }
        }

        public string argumentPermission
        {
            get
            {
                return _argumentPermission;
            }
        }

        public bool hasMaxUsage
        {
            get
            {
                return (_maxUsage < 0);
            }
        }

        public int maxUsage
        {
            get
            {
                return _maxUsage;
            }
        }
        #endregion

        public PBCommand()
        {
        }

        #region Abstract Functions
        public abstract void onCall() {}
        #endregion

        #region Virtual Functions
        public virtual bool checkPermissions() // NOT DONE!
        {
            return false;
        }

        public virtual bool hasCooldown() // NOT DONE!
        {
            return false;
        }

        public virtual bool hasReachedLimit() // NOT DONE!
        {
            return false;
        }

        public virtual void execute() // NOT DONE!
        {
            return;
        }
        #endregion
    }
}
