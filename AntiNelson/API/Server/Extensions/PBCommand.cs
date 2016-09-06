using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.API.Server.Extensions
{
    public abstract class PBCommand : MonoBehaviour
    {
        #region Variables
        private Local _localization = null;
        #endregion

        #region Properties
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

        public virtual string command
        {
            get;
            set;
        }

        public virtual int cooldown
        {
            get;
            set;
        }

        public virtual string permission
        {
            get;
            set;
        }

        public bool hasMaxUsage
        {
            get
            {
                return (maxUsage != null);
            }
        }

        public virtual int maxUsage
        {
            get;
            set;
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

        public virtual string[] alias
        {
            get;
            set;
        }
        #endregion

        #region Abstract Functions
        public abstract void onCall(PBPlayer player, string[] args);
        #endregion

        #region Virtual Functions
        public virtual bool checkPermissions(PBPlayer player, string[] args)
        {
            string prm = permission;
            foreach (string arg in args)
            {
                prm = prm + "." + arg;
            }
            return (string.IsNullOrEmpty(permission) || player.hasPermission(prm));
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
            string[] sArgs = args.Split('/');

            if (checkPermissions(player, sArgs))
            {
                PBChat.sendChatToPlayer(player, localization.format("CommandPermission"), Color.red);
                return;
            }

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
            onCall(player, sArgs);
        }
        #endregion
    }
}
