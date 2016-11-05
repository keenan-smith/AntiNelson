/*
The MIT License (MIT)
Copyright (c) 2015 Sebastian Schöner
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ManPAD.ManPAD_API
{
    public struct RedirectCallsState
    {
        public byte a, b, c, d, e;
        public ulong f;
    }

    public static class RedirectionHelper
    {
        public static RedirectCallsState RedirectCalls(MethodInfo from, MethodInfo to)
        {
            var fptr1 = from.MethodHandle.GetFunctionPointer();
            var fptr2 = to.MethodHandle.GetFunctionPointer();
            MP_Logging.Log("Patching " + fptr1 + " to " + fptr2);
            return PatchJumpTo(fptr1, fptr2);
        }

        public static void RevertRedirect(MethodInfo from, RedirectCallsState state)
        {
            var fptr1 = from.MethodHandle.GetFunctionPointer();
            MP_Logging.Log("Reverting " + fptr1 + "...");
            RevertJumpTo(fptr1, state);
        }

        private static RedirectCallsState PatchJumpTo(IntPtr site, IntPtr target)
        {
            RedirectCallsState state = new RedirectCallsState();

            unsafe
            {
                byte* sitePtr = (byte*)site.ToPointer();
                state.a = *sitePtr;
                state.b = *(sitePtr + 1);
                state.c = *(sitePtr + 10);
                state.d = *(sitePtr + 11);
                state.e = *(sitePtr + 12);
                state.f = *((ulong*)(sitePtr + 2));
            }
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
            return state;
        }

        private static void RevertJumpTo(IntPtr site, RedirectCallsState state)
        {
            unsafe
            {
                byte* sitePtr = (byte*)site.ToPointer();
                *sitePtr = state.a;
                *(sitePtr + 1) = state.b;
                *((ulong*)(sitePtr + 2)) = state.f;
                *(sitePtr + 10) = state.c;
                *(sitePtr + 11) = state.d;
                *(sitePtr + 12) = state.e;
            }
        }
    }
}