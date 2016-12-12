using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteRetriver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("API_MonoLoader.dll"))
            {
                byte[] bArray = File.ReadAllBytes("API_MonoLoader.dll");
                string output = "";
                foreach (byte b in bArray)
                {
                    output += (int)b + ",\n";
                }
                File.WriteAllText("API_MonoLoader.txt", output);
            }

            if (File.Exists("ManPAD.dll"))
            {
                byte[] bArray = File.ReadAllBytes("ManPAD.dll");
                string output = "";
                foreach (byte b in bArray)
                {
                    output += (int)b + ",\n";
                }
                File.WriteAllText("ManPAD.txt", output);
            }
        }
    }
}
