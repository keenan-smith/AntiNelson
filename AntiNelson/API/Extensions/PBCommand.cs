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
        private int _cooldown;
        private string _permission;
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

        public int cooldown
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

        public string help
        {
            get
            {
                return localization.format("Help");
            }
        }

        public string usage
        {
            get
            {
                return localization.format("Usage");
            }
        }
        #endregion

        public PBCommand(string command, Local language, int cooldown = 1, string permission = "", int maxUsage = -1)
        {
            _command = command;
            _cooldown = cooldown;
            _permission = permission;
            localization = language;
            _maxUsage = maxUsage;
        }

        #region Abstract Functions
        public abstract void onCall(PBPlayer player, string[] args) {}
        #endregion

        #region Virtual Functions
        public virtual bool checkPermissions(PBPlayer player)
        {
            return (string.IsNullOrEmpty(permission) || player.hasPermission(permission));
        }

        public virtual bool hasCooldown(PBPlayer player)
        {
            bool chk1 = player.hasPermission("no-command-cooldown");
            bool chk2 = !player.hasCooldown(this);

            return (chk1 || chk2);
        }

        public virtual bool hasReachedLimit(PBPlayer player)
        {
            bool chk1 = player.hasPermission("no-command-limit");
            bool chk2 = !player.hasReachedLimit(this, maxUsage);

            return (chk1 || chk2);
        }

        public virtual void execute(PBPlayer player, string args) // NOT DONE!
        {
            if (hasReachedLimit(player))
            {
                // Tell player that the command limit is reached!
                return;
            }

            if (hasCooldown(player))
            {
                // Tell player that the command is in cooldown!
                return;
            }

            PBCooldown cDown = Array.Find(player.cooldowns.ToArray(), a => a.command == this);
            if (cDown == null)
            {
                cDown = new PBCooldown(this, DateTime.Now, cooldown);
            }
            if (maxUsage > -1)
                cDown.usage++;
            onCall(player, args.Split('/'));
        }
        #endregion
    }
}
