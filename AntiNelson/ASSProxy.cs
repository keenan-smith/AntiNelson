using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace PointBlank
{
    public class ASSProxy : MarshalByRefObject
    {
        public Assembly GetAssembly(string path)
        {
            try
            {
                return Assembly.Load(File.ReadAllBytes(path));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
