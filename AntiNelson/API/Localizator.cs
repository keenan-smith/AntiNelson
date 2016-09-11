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
        public static bool exists(string path)
        {
            path = Variables.currentPath + "\\" + path;
            return ReadWrite.fileExists(path, false, false);
        }

        public static Local read(string path)
        {

            Console.WriteLine(Variables.currentPath + "\\" + path);

            if (exists(path))
            {
                path = Variables.currentPath + "\\" + path;
                return new Local(ReadWrite.readData(path, false, false));
            }
            return new Local();
        }
    }
}
