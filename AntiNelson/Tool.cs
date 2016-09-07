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

        public static string getServerName()
        {

            String args = Environment.CommandLine;

            int sServer = args.ToLower().IndexOf("+secureserver");
            int iServer = args.ToLower().IndexOf("+insecureserver");
            int lServer = args.ToLower().IndexOf("+lanserver");

            if (sServer != -1)
                return args.Substring(sServer + 14, args.Length - sServer - 14);

            if (iServer != -1)
                return args.Substring(iServer + 16, args.Length - iServer - 16);

            if (lServer != -1)
                return args.Substring(lServer + 11, args.Length - lServer - 11);

            return "";

        }

    }
}
