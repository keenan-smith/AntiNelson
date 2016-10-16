using System;
using Shell32;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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

        public static byte[] getResource(string name)
        {
            Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("PointBlank.Properties.Resources.resources");
            using (ResourceSet set = new ResourceSet(st))
            {
                return (byte[])set.GetObject(name, true);
            }
        }

        public static bool IsShortcut(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string file = Path.GetFileName(path);

            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder folder = shell.NameSpace(directory);
            Shell32.FolderItem folderItem = folder.ParseName(file);

            if (folderItem != null)
            {
                return folderItem.IsLink;
            }

            return false;
        }

        public static string ResolveShortcut(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string file = Path.GetFileName(path);

            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder folder = shell.NameSpace(directory);
            Shell32.FolderItem folderItem = folder.ParseName(file);

            Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;

            return link.Path;
        }
    }
}
