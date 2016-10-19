using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Net;
using System.Threading;

namespace Patcher
{
    class Program
    {


        public static AssemblyDefinition ACS, UE, PB, patcher = AssemblyDefinition.ReadAssembly(Assembly.GetExecutingAssembly().Location);
        public static String prefix = "";
        public static String version = "";
        public static byte[] pbArr = null;

        static void Main(string[] args)
        {

#if DEBUG

            prefix = @"D:\Steam\steamapps\common\Unturned\Unturned_Data\Managed\";
            //PB = AssemblyDefinition.ReadAssembly(prefix + "PointBlank.dll");

#endif
            ACS = AssemblyDefinition.ReadAssembly(prefix + "Assembly-CSharp.dll");
            UE = AssemblyDefinition.ReadAssembly(prefix + "UnityEngine.dll");

            checkVersion();

            TypeDefinition patched = null;
            if ((patched = isPatched()) != null)
            {

                if (version == patched.Fields.ToArray()[0].Name.Split('_')[1])
                {

                    Console.WriteLine("Already patched!");
                    Console.ReadLine();

                    return;

                }
                else
                {

                    Console.WriteLine("Running old version! (" + patched.Fields.ToArray()[0].Name + ")");
                    Console.WriteLine("Fetching new version...");

                    if (downloadPB())
                    {

                        using (FileStream fs = new FileStream(prefix + "PointBlank.dll", FileMode.Create))
                        {

                            using (BinaryWriter bw = new BinaryWriter(fs))
                            {
                                bw.Write(pbArr);
                                bw.Dispose();
                            }

                            fs.Dispose();
                        }

                        patched = isPatched();
                        patched.Fields.Clear();
                        patched.Fields.Add(new FieldDefinition("VERSION_" + version, Mono.Cecil.FieldAttributes.Static, UE.MainModule.Import(typeof(bool))));
                        UE.Write(prefix + "UnityEngine.dll");

                        Console.WriteLine("Done!");
                        Console.ReadKey();
                        

                    }

                    return;

                }

            }

            TypeDefinition IP = new TypeDefinition("", "IsPatched", Mono.Cecil.TypeAttributes.Class);
            IP.Fields.Add(new FieldDefinition("VERSION_" + version, Mono.Cecil.FieldAttributes.Static, UE.MainModule.Import(typeof(bool))));
            UE.MainModule.Types.Add(IP);

            if (downloadPB())
            {

                using (FileStream fs = new FileStream(prefix + "PointBlank.dll", FileMode.Create))
                {

                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(pbArr);
                        bw.Dispose();
                    }

                    fs.Dispose();
                }

                PB = AssemblyDefinition.ReadAssembly(prefix + "PointBlank.dll");

                TypeDefinition EP = getClass(PB, "EntryPoint");
                MethodDefinition launch = getMethod(EP, "Launch");

                TypeDefinition UE_Module = getClass(UE, "<Module>");
                MethodDefinition cctor = createStaticConstructor(UE_Module);
                cctor.Body.GetILProcessor().Append(Instruction.Create(OpCodes.Call, UE.MainModule.Import(launch)));
                cctor.Body.GetILProcessor().Append(Instruction.Create(OpCodes.Ret));

                UE.Write(prefix + "UnityEngine.dll");

                Console.WriteLine("Done!");
                Console.ReadKey();

            }

        }

        public static bool fileLocked()
        {

            try
            {

                PB = AssemblyDefinition.ReadAssembly(prefix + "PointBlank.dll");
                return false;

            }
            catch (Exception) { return true; }

        }

        public static bool checkVersion()
        {

            try
            {

                Console.WriteLine("Checking PointBlank version...");
                version = new WebClient().DownloadString("https://raw.githubusercontent.com/Kunii/PBData/master/VERSION");

            }
            catch (Exception e)
            {

                Console.WriteLine("Couldn\'t check version! Aborting!");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(e);
                Console.ReadKey();

                return false;

            }

            Console.WriteLine("Version: " + version);
            return true;

        }

        public static bool downloadPB()
        {

            try
            {

                Console.WriteLine("Donwloading PointBlank...");
                pbArr = decompress(new WebClient().DownloadString("https://raw.githubusercontent.com/Kunii/PBData/master/DATA"));
                Console.WriteLine("Size: " + pbArr.Length);
            }
            catch (Exception e)
            {

                Console.WriteLine("Couldn\'t download PB! Aborting!");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(e);
                Console.ReadKey();

                return false;
            }

            return true;

        }

        public static byte[] decompress(String input)
        {

            using (MemoryStream ms = new MemoryStream())
            {

                using (BufferedStream bs = new BufferedStream(new DeflateStream(new MemoryStream(Convert.FromBase64String(input)), CompressionMode.Decompress)))
                {
                    bs.CopyTo(ms);
                    bs.Dispose();
                }

                return ms.ToArray();

            }


        }

        public static String compress(byte[] input)
        {

            using (MemoryStream ms = new MemoryStream())
            {

                using (BufferedStream bs = new BufferedStream(new DeflateStream(ms, CompressionMode.Compress)))
                {
                    bs.Write(input, 0, input.Length);
                }

                return Convert.ToBase64String(ms.ToArray());

            }

        }

        public static TypeDefinition isPatched()
        {

            foreach (TypeDefinition type in UE.MainModule.Types)
                if (type.Name == "IsPatched")
                    return type;

            return null;

        }

        public static TypeDefinition getClass(AssemblyDefinition ad, String _class)
        {

            foreach (TypeDefinition td in ad.MainModule.Types)
                if (td.Name == _class)
                    return td;

            return null;

        }

        public static MethodDefinition createStaticConstructor(TypeDefinition type)
        {

            MethodDefinition md = new MethodDefinition(".cctor", Mono.Cecil.MethodAttributes.Private | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName | Mono.Cecil.MethodAttributes.Static, type.Module.Import(typeof(void)));
            type.Methods.Add(md);

            return md;

        }

        public static MethodDefinition getMethod(TypeDefinition td, String method)
        {

            foreach (MethodDefinition md in td.Methods)
                if (md.Name == method)
                    return md;

            return null;

        }

    }

}
