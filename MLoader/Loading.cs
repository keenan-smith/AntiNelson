using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
                try
                {
                    Assembly asm = Assembly.LoadFrom(Directory.GetCurrentDirectory() + "/ManPAD.dll");
                    asm.GetTypes().Where(a => a.IsClass && a.Name == "Hook").First().GetMethod("callMeToHook", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[0]);
                    injected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
