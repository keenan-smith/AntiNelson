using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Internal_Injector
{
    class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwAccess, int bIHandle, uint dwProcID);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hObj);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, String lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(String lpModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProc, IntPtr lpAddr, IntPtr dwSize, uint flAllocType, uint flProt);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProcessMemory(IntPtr hProc, IntPtr lpAddr, byte[] buffer, uint size, int lpBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProc, IntPtr lpThreadAttribute, IntPtr dwStack, IntPtr lpAddr, IntPtr lpParm, uint dwCFlags, IntPtr lpID);


        static void Main(string[] args)
        {

            String ml = IntPtr.Size == 4 ? loadEmbeddedFile("Internal_Injector.Monoloaderx86.dll", "EAC1.tmp") : loadEmbeddedFile("Internal_Injector.Monoloaderx64.dll", "EAC2.tmp");
            Console.WriteLine("Running " + (IntPtr.Size == 4 ? "x86 " : "x64 "));

            loadEmbeddedFile("Internal_Injector.MonolandLoader.dll", ".-");

            LLAInject("Unturned", ml);

        }

        public static void LLAInject(String target, String dllPath)
        {

            if (!File.Exists(dllPath))
            {

                Console.WriteLine("Could not find hack!");

                Console.ReadKey();
                return;

            }

            uint pid = getPID(target);
            IntPtr rProc = OpenProcess(2 | 8 | 16 | 32 | 1024, 1, pid);

            if (rProc == IntPtr.Zero)
            {

                Console.WriteLine("Could not open process!");

                Console.ReadKey();
                return;

            }

            IntPtr LLA = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (LLA == IntPtr.Zero)
            {

                Console.WriteLine("Failed to get LLA address!");
                CloseHandle(rProc);

                Console.ReadKey();
                return;

            }

            IntPtr writeAddr = VirtualAllocEx(rProc, (IntPtr)null, (IntPtr)dllPath.Length, 4096 | 8192, 64);

            if (writeAddr == IntPtr.Zero)
            {

                Console.WriteLine("Failed to allocate memory!");
                CloseHandle(rProc);

                Console.ReadKey();
                return;

            }

            byte[] bArr = Encoding.ASCII.GetBytes(dllPath);

            if(WriteProcessMemory(rProc, writeAddr, bArr, (uint)bArr.Length, 0) == 0)
            {

                Console.WriteLine("Failed to write to memory!");
                CloseHandle(rProc);

                Console.ReadKey();
                return;

            }

            if(CreateRemoteThread(rProc, (IntPtr)null, IntPtr.Zero, LLA, writeAddr, 0, (IntPtr)null) == IntPtr.Zero)
            {

                Console.WriteLine("Failed to start remote thread!");
                CloseHandle(rProc);

                Console.ReadKey();
                return;

            }

            CloseHandle(rProc);

        }

        public static String loadEmbeddedFile(string embeddedResource, string outName)
        {

            byte[] ba = null;
            Assembly asm = null;

            using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResource))
            {

                if (stm == null)
                    throw new Exception(embeddedResource + " is not found in Embedded Resources.");

                ba = new byte[(int)stm.Length];
                stm.Read(ba, 0, (int)stm.Length);

                try
                {
                    asm = Assembly.Load(ba);
                }
                catch
                {

                }

            }

            String tempFile = Path.GetTempPath() + outName;

            File.WriteAllBytes(tempFile, ba);

            return tempFile;

        }

        public static uint getPID(String target)
        {

            Process[] procs = Process.GetProcessesByName(target);

            if (procs.Length > 0)
                return (uint)procs[0].Id;

            return 0;

        }

    }
}
