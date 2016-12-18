using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;

namespace ManPAD.ManPAD_Library
{
    public class lib_MethodReplacer : MonoBehaviour
    {
        #region Mono Functions
        public void Start()
        {
            MP_Logging.Log("Loading Method Replacer....");

            doTypes();
            MP_Logging.Log("Method Replacer Loaded!");
        }
        #endregion

        #region Functions
        private void doMethods(Type t)
        {
            for(int i = 0; i < t.GetMethods().Length; i++)
            {
                MethodInfo mi = t.GetMethods()[i];
                CodeReplaceAttribute cra = (CodeReplaceAttribute)Attribute.GetCustomAttribute(mi, typeof(CodeReplaceAttribute));

                if (cra == null)
                    continue;

                cra.callState = MP_Redirector.RedirectCalls(cra.method, mi);
            }
        }

        public void doTypes()
        {
            for(int i = 0; i < Assembly.GetExecutingAssembly().GetTypes().Length; i++)
            {
                Type t = Assembly.GetExecutingAssembly().GetTypes()[i];
                if (t.IsClass)
                    doMethods(t);
            }
        }
        #endregion
    }
}
