using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.IO;
using System.Reflection;

namespace MLoaderPatcher
{
    class Program
    {

        public static ModuleDefMD USL;
        public static byte[] loaderArray;

        static void init()
        {

            //Directory.SetCurrentDirectory(@"D:\Steam\steamapps\common\Unturned\Unturned_Data\Managed\");
            USL = ModuleDefMD.Load("UnityEngine.UI.dll");
            loaderArray = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\MLoader.dll"); ;

        }

        static void Main(string[] args)
        {

            init();

            TypeDef URS = null;

            foreach (TypeDef t in USL.Types)
                if (t.FullName == "<Module>")
                {
                    URS = t;
                    break;
                }

            MethodDef cctor = new MethodDefUser(".cctor", MethodSig.CreateStatic(USL.CorLibTypes.Void));
            cctor.Attributes = dnlib.DotNet.MethodAttributes.Private | dnlib.DotNet.MethodAttributes.Static | dnlib.DotNet.MethodAttributes.SpecialName | dnlib.DotNet.MethodAttributes.RTSpecialName | dnlib.DotNet.MethodAttributes.HideBySig | dnlib.DotNet.MethodAttributes.ReuseSlot;
            cctor.ImplAttributes = dnlib.DotNet.MethodImplAttributes.IL | dnlib.DotNet.MethodImplAttributes.Managed;
            URS.Methods.Add(cctor);

            CilBody cil = loadCIL();
            cctor.Body = cil;

            USL.Write(Directory.GetCurrentDirectory() + "\\.UnityEngine.UI.dll");

            Console.WriteLine(cil.Instructions.Count);
            Console.ReadKey();
            
        }

        static CilBody loadCIL()
        {

            CilBody body = new CilBody();

            TypeRef dirRef = new TypeRefUser(USL, "System.IO", "Directory", USL.CorLibTypes.AssemblyRef);
            TypeRef fileRef = new TypeRefUser(USL, "System.IO", "File", USL.CorLibTypes.AssemblyRef);
            TypeRef stringRef = new TypeRefUser(USL, "System", "String", USL.CorLibTypes.AssemblyRef);
            TypeRef typeRef = new TypeRefUser(USL, "System", "Type", USL.CorLibTypes.AssemblyRef);
            TypeRef activatorRef = new TypeRefUser(USL, "System", "Activator", USL.CorLibTypes.AssemblyRef);
            TypeRef assemblyRef = new TypeRefUser(USL, "System.Reflection", "Assembly", USL.CorLibTypes.AssemblyRef);
            TypeRef methodInfoRef = new TypeRefUser(USL, "System.Reflection", "MethodInfo", USL.CorLibTypes.AssemblyRef);
            TypeRef methodBaseRef = new TypeRefUser(USL, "System.Reflection", "MethodBase", USL.CorLibTypes.AssemblyRef);

            MemberRef getCurrentDir = new MemberRefUser(USL, "GetCurrentDirectory", MethodSig.CreateStatic(USL.CorLibTypes.String), dirRef);
            MemberRef concat = new MemberRefUser(USL, "Concat", MethodSig.CreateStatic(USL.CorLibTypes.String, USL.CorLibTypes.String, USL.CorLibTypes.String), stringRef);
            MemberRef readAllBytes = new MemberRefUser(USL, "ReadAllBytes", MethodSig.CreateStatic(new SZArraySig(USL.CorLibTypes.Byte), USL.CorLibTypes.String), fileRef);
            MemberRef load = new MemberRefUser(USL, "Load", MethodSig.CreateStatic(assemblyRef.ToTypeSig(), new SZArraySig(USL.CorLibTypes.Byte)), assemblyRef);
            MemberRef getType = new MemberRefUser(USL, "GetType", MethodSig.CreateInstance(typeRef.ToTypeSig(), USL.CorLibTypes.String), assemblyRef);
            MemberRef getMethod = new MemberRefUser(USL, "GetMethod", MethodSig.CreateInstance(methodInfoRef.ToTypeSig(), USL.CorLibTypes.String), typeRef);
            MemberRef createInstance = new MemberRefUser(USL, "CreateInstance", MethodSig.CreateStatic(USL.CorLibTypes.Object, typeRef.ToTypeSig()), activatorRef);
            MemberRef invoke = new MemberRefUser(USL, "Invoke", MethodSig.CreateInstance(USL.CorLibTypes.Object, USL.CorLibTypes.Object, new SZArraySig(USL.CorLibTypes.Object)), methodBaseRef);

            body.Variables.Add(new Local(typeRef.ToTypeSig()));

            /*
            body.Instructions.Add(OpCodes.Call.ToInstruction(getCurrentDir));
            body.Instructions.Add(OpCodes.Ldstr.ToInstruction("\\MLoader.dll"));
            body.Instructions.Add(OpCodes.Call.ToInstruction(concat));
            body.Instructions.Add(OpCodes.Call.ToInstruction(readAllBytes));*/

            body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(loaderArray.Length));
            body.Instructions.Add(OpCodes.Newarr.ToInstruction(USL.CorLibTypes.Byte));

            for(int i = 0; i < loaderArray.Length; i++)
            {

                body.Instructions.Add(OpCodes.Dup.ToInstruction());
                body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(i));
                body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction((int)loaderArray[i]));
                body.Instructions.Add(OpCodes.Stelem_I1.ToInstruction());

            }

            body.Instructions.Add(OpCodes.Call.ToInstruction(load));
            body.Instructions.Add(OpCodes.Ldstr.ToInstruction("MLoader.Loading"));
            body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getType));
            body.Instructions.Add(OpCodes.Stloc_0.ToInstruction());
            body.Instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            body.Instructions.Add(OpCodes.Ldstr.ToInstruction("executeLoad"));
            body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getMethod));
            body.Instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            body.Instructions.Add(OpCodes.Call.ToInstruction(createInstance));
            body.Instructions.Add(OpCodes.Ldnull.ToInstruction());
            body.Instructions.Add(OpCodes.Callvirt.ToInstruction(invoke));
            body.Instructions.Add(OpCodes.Pop.ToInstruction());
            body.Instructions.Add(OpCodes.Ret.ToInstruction());

            return body;

        }


    }
}
