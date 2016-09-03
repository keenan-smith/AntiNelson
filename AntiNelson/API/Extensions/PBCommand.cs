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
        private Local _localization = null;
        #endregion

        #region Propertys
        public Local localization
        {
            get
            {
                return _localization;
            }
            set
            {
                if(_localization == null)
                    _localization = value;
            }
        }

        public abstract string command
        {
            get;
        }

        public virtual int cooldown
        {
            get
            {
                return 1;
            }
        }

        public abstract string permission
        {
            get;
        }

        public bool hasMaxUsage
        {
            get
            {
                return (maxUsage < 0);
            }
        }

        public virtual int maxUsage
        {
            get
            {
                return -1;
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

        public abstract string[] alias
        {
            get;
        }
        #endregion

        #region Abstract Functions
        public abstract void onCall(PBPlayer player, string[] args);
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

        public virtual void execute(PBPlayer player, string args)
        {
            if (hasReachedLimit(player))
            {
                PBChat.sendChatToPlayer(player, localization.format("CommandLimit"), Color.red);
                return;
            }

            if (hasCooldown(player))
            {
                PBChat.sendChatToPlayer(player, localization.format("CommandCooldown"), Color.red);
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
