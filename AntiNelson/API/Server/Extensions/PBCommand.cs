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
        /// <summary>
        /// The localization of a command.
        /// </summary>
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

        /// <summary>
        /// The chat command.
        /// </summary>
        public virtual string command
        {
            get;
            set;
        }

        /// <summary>
        /// The command's cooldown.
        /// </summary>
        public virtual int cooldown
        {
            get;
            set;
        }

        /// <summary>
        /// The command's required permissions to execute it.
        /// </summary>
        public virtual string permission
        {
            get;
            set;
        }

        /// <summary>
        /// Does the command have max usage.
        /// </summary>
        public bool hasMaxUsage
        {
            get
            {
                return (maxUsage != null);
            }
        }

        /// <summary>
        /// The max usage of the command.
        /// </summary>
        public virtual int maxUsage
        {
            get;
            set;
        }

        /// <summary>
        /// The help text of the command.
        /// </summary>
        public string help
        {
            get
            {
                return localization.format("Help");
            }
        }

        /// <summary>
        /// The usage text of the command.
        /// </summary>
        public string usage
        {
            get
            {
                return localization.format("Usage");
            }
        }

        /// <summary>
        /// The aliases of the command.
        /// </summary>
        public virtual string[] alias
        {
            get;
            set;
        }
        #endregion

        #region Abstract Functions
        /// <summary>
        /// Gets ran whenever the command is called.
        /// </summary>
        /// <param name="player">The player that ran the command.</param>
        /// <param name="args">The args of the command.</param>
        public abstract void onCall(PBPlayer player, string[] args);
        #endregion

        #region Virtual Functions
        /// <summary>
        /// Checks if the player has enough permissions to run the command.
        /// </summary>
        /// <param name="player">The executing player.</param>
        /// <param name="args">The arguments of the command.</param>
        /// <returns>If the player has enough permissions to run the command.</returns>
        public virtual bool checkPermissions(PBPlayer player, string[] args)
        {
            string prm = permission;
            foreach (string arg in args)
            {
                prm = prm + "." + arg;
            }
            return (string.IsNullOrEmpty(permission) || player.hasPermission(prm));
        }

        /// <summary>
        /// Checks if the player has a cooldown on the command.
        /// </summary>
        /// <param name="player">The executing player.</param>
        /// <returns>If a player has cooldown on the command.</returns>
        public virtual bool hasCooldown(PBPlayer player)
        {
            bool chk1 = !player.hasPermission("no-command-cooldown");
            bool chk2 = player.hasCooldown(this);

            return (chk1 && chk2);
        }

        /// <summary>
        /// Checks if the player has reached max limit on the command execution.
        /// </summary>
        /// <param name="player">The executing player.</param>
        /// <returns>If the player has reached command execution limit.</returns>
        public virtual bool hasReachedLimit(PBPlayer player)
        {
            bool chk1 = !player.hasPermission("no-command-limit");
            bool chk2 = player.hasReachedLimit(this, maxUsage);

            return (chk1 && chk2);
        }

        /// <summary>
        /// Does all the checks needed and executes the command.
        /// </summary>
        /// <param name="player">The executing player.</param>
        /// <param name="args">The arguments of the command.</param>
        public virtual void execute(PBPlayer player, string args)
        {
            string[] sArgs = args.Split('/');

            if (string.IsNullOrEmpty(sArgs[0]))
                sArgs = new string[0];

            if (player == null)
                return;

            if (!player.steamPlayer.isAdmin)
            {
                if (!checkPermissions(player, sArgs))
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
