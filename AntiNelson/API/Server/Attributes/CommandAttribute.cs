using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        #region Variables
        private string _pluginName;
        private string _commandName;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the plugin
        /// </summary>
        public string pluginName
        {
            get
            {
                return _pluginName;
            }
        }

        /// <summary>
        /// The name of the command
        /// </summary>
        public string commandName
        {
            get
            {
                return _commandName;
            }
        }
        #endregion

        /// <summary>
        /// Attribute for command classes. Any class with this attribute will be registered as a command.
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <param name="commandName">The name of the command.</param>
        public CommandAttribute(string pluginName, string commandName)
        {
            _pluginName = pluginName;
            _commandName = commandName;
        }
    }
}
