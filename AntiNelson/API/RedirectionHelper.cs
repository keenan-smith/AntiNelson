﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PointBlank.API
{

    public static class RedirectionHelper
    {
        [DllImport("mono.dll", CallingConvention = CallingConvention.FastCall, EntryPoint = "mono_domain_get")]
        private static extern IntPtr mono_domain_get();

        [DllImport("mono.dll", CallingConvention = CallingConvention.FastCall, EntryPoint = "mono_method_get_header")]
        private static extern IntPtr mono_method_get_header(IntPtr method);

        public static void RedirectCalls(MethodInfo from, MethodInfo to)
        {
            var fptr1 = from.MethodHandle.GetFunctionPointer();
            var fptr2 = to.MethodHandle.GetFunctionPointer();
            PBLogging.log("Patching " + fptr1 + " to " + fptr2, false);
            PatchJumpTo(fptr1, fptr2);
        }

        public static void RedirectCall(MethodInfo from, MethodInfo to)
        {
            IntPtr methodPtr1 = from.MethodHandle.Value;
            IntPtr methodPtr2 = to.MethodHandle.Value;
            from.MethodHandle.GetFunctionPointer();
            to.MethodHandle.GetFunctionPointer();

            IntPtr domain = mono_domain_get();
            unsafe
            {
                byte* jitCodeHash = ((byte*)domain.ToPointer() + 0xE8);
                long** jitCodeHashTable = *(long***)(jitCodeHash + 0x20);
                uint tableSize = *(uint*)(jitCodeHash + 0x18);

                void* jitInfoFrom = null, jitInfoTo = null;

                long mptr1 = methodPtr1.ToInt64();
                uint index1 = ((uint)mptr1) >> 3;
                for (long* value = jitCodeHashTable[index1 % tableSize];
                    value != null;
                    value = *(long**)(value + 1))
                {
                    if (mptr1 == *value)
                    {
                        jitInfoFrom = value;
                        break;
                    }
                }

                long mptr2 = methodPtr2.ToInt64();
                uint index2 = ((uint)mptr2) >> 3;
                for (long* value = jitCodeHashTable[index2 % tableSize];
                    value != null;
                    value = *(long**)(value + 1))
                {
                    if (mptr2 == *value)
                    {
                        jitInfoTo = value;
                        break;
                    }
                }
                if (jitInfoFrom == null || jitInfoTo == null)
                {
                    Debug.Log("Could not find methods");
                    return;
                }


                ulong* fromPtr, toPtr;
                fromPtr = (ulong*)jitInfoFrom;
                toPtr = (ulong*)jitInfoTo;
                *(fromPtr + 2) = *(toPtr + 2);
                *(fromPtr + 3) = *(toPtr + 3);
            }
        }

        private static void PatchJumpTo(IntPtr site, IntPtr target)
        {
            if (IntPtr.Size == 4)
            {
                unsafe
                {
                    byte* sitePtr = (byte*)site.ToPointer();
                    *(sitePtr + 1) = 0xBB;
                    *((uint*)(sitePtr + 2)) = (uint)target.ToInt32();
                    *(sitePtr + 11) = 0xFF;
                    *(sitePtr + 12) = 0xE3;
                }
            }
            else if (IntPtr.Size == 8)
            {
                unsafe
                {
                    byte* sitePtr = (byte*)site.ToPointer();
                    *sitePtr = 0x49;
                    *(sitePtr + 1) = 0xBB;
                    *((ulong*)(sitePtr + 2)) = (ulong)target.ToInt64();
                    *(sitePtr + 10) = 0x41;
                    *(sitePtr + 11) = 0xFF;
                    *(sitePtr + 12) = 0xE3;
                }
            }
        }

        public static void RedirectCallIL(MethodInfo from, MethodInfo to)
        {
            IntPtr methodPtr1 = from.MethodHandle.Value;
            IntPtr methodPtr2 = to.MethodHandle.Value;

            mono_method_get_header(methodPtr2);

            unsafe
            {
                byte* monoMethod1 = (byte*)methodPtr1.ToPointer();
                byte* monoMethod2 = (byte*)methodPtr2.ToPointer();
                *((IntPtr*)(monoMethod1 + 40)) = *((IntPtr*)(monoMethod2 + 40));
            }
        }
    }
}