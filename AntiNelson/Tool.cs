using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PointBlank
{
    public class Tool
    {
        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }
    }
}
