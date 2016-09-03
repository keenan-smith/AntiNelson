using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        #region Variables
        private string _pluginName;
        private string _commandName;
        #endregion

        #region Propertys
        public string pluginName
        {
            get
            {
                return _pluginName;
            }
        }

        public string commandName
        {
            get
            {
                return _commandName;
            }
        }
        #endregion

        public CommandAttribute(string pluginName, string commandName)
        {
            _pluginName = pluginName;
            _commandName = commandName;
        }
    }
}
