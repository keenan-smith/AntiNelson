using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MLoader
{
    public class Loading
    {
        private static bool injected = false;
        private static Thread _thread;

        public static void executeLoad()
        {
            _thread = new Thread(new ThreadStart(runThread));
            _thread.Start();
        }

        private static void runThread()
        {
            while (!injected)
            {
                Thread.Sleep(200);
                Assembly asm = Assembly.Load(Directory.GetCurrentDirectory() + @"\Unturned_Data\Managed\ManPAD.dll");
                asm.GetTypes().Where(a => a.IsClass && a.Name == "Hook").First().GetMethod("callMeToHook", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[0]);
                injected = true;
            }
        }
    }
}
