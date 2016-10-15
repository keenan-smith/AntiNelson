using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Server.Extensions
{
    public class PBCooldown
    {
        #region Variables
        private PBCommand _command;
        private int _msCooldown;
        public DateTime lastExectue;
        public int usage;
        #endregion

        #region Properties
        /// <summary>
        /// Command instance.
        /// </summary>
        public PBCommand command
        {
            get
            {
                return _command;
            }
        }

        /// <summary>
        /// Has cooldown.
        /// </summary>
        public bool cooldown
        {
            get
            {
                return ((DateTime.Now - lastExectue).Milliseconds < _msCooldown);
            }
        }
        #endregion

        /// <summary>
        /// The cooldown for the commands.
        /// </summary>
        /// <param name="command">What command has the cooldown.</param>
        /// <param name="lastRun">The last time the command was ran.</param>
        /// <param name="cooldown">The cooldown time of the command.</param>
        public PBCooldown(PBCommand command, DateTime lastRun, int cooldown)
        {
            _command = command;
            _msCooldown = cooldown;
            lastExectue = lastRun;
            usage = 0;
        }
    }
}
