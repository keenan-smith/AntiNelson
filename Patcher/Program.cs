using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Patcher
{
    class Program
    {


        public static AssemblyDefinition ACS, UE, PB, patcher = AssemblyDefinition.ReadAssembly(Assembly.GetExecutingAssembly().Location);
        public static String prefix = "";

        static void Main(string[] args)
        {

#if DEBUG

            prefix = @"D:\Steam\steamapps\common\Unturned\Unturned_Data\Managed\";
            PB = AssemblyDefinition.ReadAssembly(prefix + "PointBlank.dll");

#endif
            ACS = AssemblyDefinition.ReadAssembly(prefix + "Assembly-CSharp.dll");
            UE = AssemblyDefinition.ReadAssembly(prefix + "UnityEngine.dll");

            if (isPatched())
            {

                Console.WriteLine("Already patched!");
                Console.ReadLine();

                return;

            }

            UE.MainModule.Types.Add(new TypeDefinition("", "IsPatched", Mono.Cecil.TypeAttributes.Class));

            TypeDefinition EP = getClass(PB, "EntryPoint");
            MethodDefinition launch = getMethod(EP, "Launch");

            TypeDefinition type = getClass(UE, "<Module>");
            MethodDefinition cctor = createStaticConstructor(type);
            cctor.Body.GetILProcessor().Append(Instruction.Create(OpCodes.Call, UE.MainModule.Import(launch)));
            cctor.Body.GetILProcessor().Append(Instruction.Create(OpCodes.Ret));

            UE.Write(prefix + "UnityEngine2.dll"); //TODO: Temp fix

            Console.WriteLine("Done!");
            Console.ReadKey();

        }


        public static bool isPatched()
        {

            foreach (TypeDefinition type in UE.MainModule.Types)
                if (type.Name == "IsPatched")
                    return true;

            return false;

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
