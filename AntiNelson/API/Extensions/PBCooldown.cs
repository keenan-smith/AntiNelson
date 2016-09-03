using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Extensions
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
        public PBCommand command
        {
            get
            {
                return _command;
            }
        }

        public bool cooldown
        {
            get
            {
                return ((DateTime.Now - lastExectue).Milliseconds < _msCooldown);
            }
        }
        #endregion

        public PBCooldown(PBCommand command, DateTime lastRun, int cooldown)
        {
            _command = command;
            _msCooldown = cooldown;
            lastExectue = lastRun;
            usage = 0;
        }
    }
}
