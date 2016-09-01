using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Extensions;

namespace PointBlank.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        #region Variables
        private string _localFile;
        private Local _localization = null;
        #endregion

        #region Propertys
        public string localFile
        {
            get
            {
                return _localFile;
            }
        }

        public bool hasLocalization
        {
            get
            {
                return (_localization != null);
            }
        }

        public Local localization
        {
            get
            {
                return _localization;
            }
        }
        #endregion

        public CommandAttribute(/*string localizationFile, */)
        {
            /*_localFile = localizationFile;
            if (Localizator.exists(localizationFile))
                _localization = Localizator.read(localizationFile);*/
        }
    }
}
