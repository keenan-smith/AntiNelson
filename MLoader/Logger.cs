using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLoader
{
    public class Logger
    {
        private static string path = Directory.GetCurrentDirectory() + "\MLoaderLogs.txt";
        private static bool inited = false;

        private static void initLogger()
        {
            if(File.Exists(path))
                File.Delete(path);

            File.Create(path);
            inited = true;
        }

        public static void log(string text)
        {
            if(!inited)
                initLogger();

            File.AppendAllText(path, text);
        }
    }
}
