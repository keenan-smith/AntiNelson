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
        public static void executeLoad()
        {
            byte[] ba = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\ManPAD.dll");

            Type t = Assembly.Load(ba).GetType("ManPAD.ManPAD_Loading.Hook");
            t.GetMethod("callMeToHook").Invoke(Activator.CreateInstance(t), null);
        }
    }
}
