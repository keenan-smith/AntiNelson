using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank
{
    internal class Tool
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

        public static byte[] getResource(string name, Assembly asm)
        {
            try
            {
                foreach(string rName in asm.GetManifestResourceNames())
                {
                    //Stream st = asm.GetManifestResourceStream(pluginNamespace + ".Properties.Resources.resources");
                    Stream st = asm.GetManifestResourceStream(rName);
                    using (ResourceSet set = new ResourceSet(st))
                    {
                        return (byte[])set.GetObject(name, true);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object runMethod(Type loc, string name, object[] args)
        {
            return loc.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, args);
        }
    }
}
