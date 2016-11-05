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
        }
        #endregion

        #region Functions
        private void doMethods(Type t)
        {
            foreach (MethodInfo mi in t.GetMethods())
            {
                CodeReplaceAttribute cra = (CodeReplaceAttribute)Attribute.GetCustomAttribute(mi, typeof(CodeReplaceAttribute));

                if (cra == null)
                    continue;

                cra.callState = MP_Redirector.RedirectCalls(cra.method, mi);
            }
        }

        public void doTypes()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.IsClass)
                    doMethods(t);
            }
        }
        #endregion
    }
}
