using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Extensions;
using PointBlank.API.Attributes;
using PointBlank.PB_Library;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.PB_Library
{
    public class lib_CodeReplacer
    {
        public lib_CodeReplacer()
        {
            loadCodes(AppDomain.CurrentDomain);
            loadCodes(lib_PluginManager.pluginDomain);
        }

        #region Functions
        public bool loadCodes(AppDomain domain)
        {
            try
            {
                foreach (Assembly asm in domain.GetAssemblies())
                {
                    foreach (Type t in asm.GetTypes())
                    {
                        if (t.IsClass)
                        {
                            foreach (MethodInfo mi in t.GetMethods())
                            {
                                ReplaceCodeAttribute rca = (ReplaceCodeAttribute)Attribute.GetCustomAttribute(mi, typeof(ReplaceCodeAttribute));
                                if (rca != null)
                                {
                                    if (rca.useIL)
                                        RedirectionHelper.RedirectCallIL(rca.method, mi);
                                    else
                                        RedirectionHelper.RedirectCalls(rca.method, mi);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while trying to load codes!", ex);
                return false;
            }
        }

        public bool loadCodes(Assembly asm)
        {
            try
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsClass)
                    {
                        foreach (MethodInfo mi in t.GetMethods())
                        {
                            ReplaceCodeAttribute rca = (ReplaceCodeAttribute)Attribute.GetCustomAttribute(mi, typeof(ReplaceCodeAttribute));
                            if (rca != null)
                            {
                                if (rca.useIL)
                                    RedirectionHelper.RedirectCallIL(rca.method, mi);
                                else
                                    RedirectionHelper.RedirectCalls(rca.method, mi);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while trying to load codes!", ex);
                return false;
            }
        }
        #endregion
    }
}
