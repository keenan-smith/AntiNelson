using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;

namespace PointBlank.API
{
    public class Localizator
    {
        #region Functions
        /// <summary>
        /// Return if the localization file exist.
        /// </summary>
        /// <param name="path">The path from the workpath to the localization file.</param>
        /// <returns>If the file exists</returns>
        public static bool exists(string path)
        {
            path = Variables.currentPath + "\\" + path;
            return ReadWrite.fileExists(path, false, false);
        }

        /// <summary>
        /// Read the localization of the localization file.
        /// </summary>
        /// <param name="path">The path from the workpath to the localization file.</param>
        /// <returns>Localization</returns>
        public static Local read(string path)
        {
            if (exists(path))
            {
                path = Variables.currentPath + "\\" + path;
                return new Local(ReadWrite.readData(path, false, false));
            }
            return new Local();
        }
        #endregion
    }
}
